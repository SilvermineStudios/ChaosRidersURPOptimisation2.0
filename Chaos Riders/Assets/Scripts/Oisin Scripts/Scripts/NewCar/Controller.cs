using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Photon.Pun;

public class Controller : MonoBehaviour
{
    enum CarClass { Braker, Shredder };
    public Rigidbody rb;

    #region Cars
    [Header("Car Data")]
    [SerializeField] CarClass currentCarClass;
    Vehicle carData;
    [SerializeField] Vehicle[] VehicleData;
    [SerializeField] GameObject[] Models;
    #endregion

    #region Bools
    [Header("Bools")]
    [SerializeField] private bool displayGui = false;
    public bool boost;
    public bool brake, braking;
    public bool drifting;
    #endregion

    #region Floats
    [Header("Floats")]
    private float horizontalInput;
    public float verticalInput;
    private float steeringAngle;
    float oldRot;
    private float currentTorque;
    private float boostTorque;
    public float currentSpeed { get { return rb.velocity.magnitude * 2.23693629f; } private set { } }
    float num = 0.002f;
    #endregion

    #region Wheels
    [Header("Wheels")]
    [SerializeField] private WheelCollider[] wheelColliders = new WheelCollider[4];
    [SerializeField] private Transform[] wheelMeshes = new Transform[4];
    #endregion

    #region Camera
    [Header("Camera")]
    private CinemachineVirtualCamera cineCamera;
    CinemachineTransposer cineCamTransposer;
    #endregion

    #region Scripts
    Skidmarks skidmarksController;
    PhotonView pv;
    Health healthScript;
    [HideInInspector] public TurretTester ShooterAttached; //Does this need to still be here?
    [HideInInspector] public GameObject Shooter;
    #endregion

    #region Skidmarks
    [Header("Skidmarks")]
    Skidmarks[] skidmarks = new Skidmarks[4];
    int[] lastSkid = new int[4];
    WheelFrictionCurve drift;
    WheelFrictionCurve normal;
    #endregion

    #region Sound
    [Header("Sound")]
    FMOD.Studio.Bus SkidBus;
    FMOD.Studio.EventInstance skidSound;
    public float Revs { get; private set; }
    private int gearNum;
    private float gearFactor;
    #endregion

    
   

    private void Awake()
    {
        skidSound = FMODUnity.RuntimeManager.CreateInstance("event:/CarFX/All/Skid");
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(skidSound, transform, rb);
        skidSound.start();
        skidSound.setVolume(0);
    }

    


    private void Start()
    {
        if(currentCarClass == CarClass.Braker)
        {
            carData = VehicleData[0];
        }
        else if (currentCarClass == CarClass.Shredder)
        {
            carData = VehicleData[1];
        }

        skidmarksController = FindObjectOfType<Skidmarks>();
        pv = GetComponent<PhotonView>();
        healthScript = GetComponent<Health>();

        skidmarks[0] = skidmarksController;
        skidmarks[1] = skidmarksController;
        skidmarks[2] = skidmarksController;
        skidmarks[3] = skidmarksController;


        normal.extremumSlip = 0.2f;
        normal.extremumValue = 1;
        normal.asymptoteSlip = 0.8f;
        normal.asymptoteValue = 0.5f;
        normal.stiffness = 1;


        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = carData.centerOfMass;
        rb.mass = carData.vehicleMass;
        rb.drag = carData.vehicleDrag;
        rb.angularDrag = carData.vehicleAngularDrag;
        currentTorque = carData.fullTorqueOverAllWheels - (carData.tractionControl * carData.fullTorqueOverAllWheels);
        boostTorque = carData.boostFullTorqueOverAllWheels;
        cineCamera = gameObject.transform.GetChild(0).gameObject.GetComponent<CinemachineVirtualCamera>();
        cineCamTransposer = cineCamera.GetCinemachineComponent<CinemachineTransposer>();
        cineCamTransposer.m_FollowOffset = carData.stationaryCamOffset;
    }

    private void FixedUpdate()
    {
        if (healthScript.isDead)
        {
            GetComponent<Rigidbody>().drag = 5;
            return;
        }
        else
        {
            GetComponent<Rigidbody>().drag = 0;
        }

        if (pv.IsMine && IsThisMultiplayer.Instance.multiplayer|| !IsThisMultiplayer.Instance.multiplayer)
        {
            StartRace();
            Brake();
            AdjustStiffness();
            Accelerate();
            CalculateRevs();
            GearChanging();
            AddDownForce();
            Skid();
            //Drift();
            TractionControl();
            Steer();
            HelpSteer();
            CapSpeed();
            UpdateWheelPoses();
            ChangeFOV();
            
        }
    }
    
    private void StartRace()
    {
        if(!MasterClientRaceStart.Instance.countdownTimerStart)
        {
            ApplyBrake(carData.brakeTorque);
        }
        else
        {
            ReleaseBrake();
        }
    }
    
    private void Update()
    {
        GetInput();
        if (Input.GetKeyDown(KeyCode.R))
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + 2f, transform.position.z - 1f);
            transform.rotation = Quaternion.Euler(0, transform.localRotation.y, 0);

            if (ShooterAttached != null)
            {
                ShooterAttached.ResetPos();
            }
        }
        
    }

    private void CalculateRevs()
    {
        // calculate engine revs (for display / sound)
        // (this is done in retrospect - revs are not used in force/power calculations)
        CalculateGearFactor();
        var gearNumFactor = gearNum / (float)carData.numOfGears;
        var revsRangeMin = ULerp(0f, carData.revRangeBoundary, CurveFactor(gearNumFactor));
        var revsRangeMax = ULerp(carData.revRangeBoundary, 1f, gearNumFactor);
        Revs = ULerp(revsRangeMin, revsRangeMax, gearFactor);
    }

    private void CalculateGearFactor()
    {
        float f = (1 / (float)carData.numOfGears);
        // gear factor is a normalised representation of the current speed within the current gear's range of speeds.
        // We smooth towards the 'target' gear factor, so that revs don't instantly snap up or down when changing gear.
        var targetGearFactor = Mathf.InverseLerp(f * gearNum, f * (gearNum + 1), Mathf.Abs(currentSpeed / carData.topSpeed));
        gearFactor = Mathf.Lerp(gearFactor, targetGearFactor, Time.deltaTime * 5f);
    }

    // unclamped version of Lerp, to allow value to exceed the from-to range
    private static float ULerp(float from, float to, float value)
    {
        return (1.0f - value) * from + value * to;
    }

    private void GearChanging()
    {
        float f = Mathf.Abs(currentSpeed / carData.topSpeed);
        float upgearlimit = (1 / (float)carData.numOfGears) * (gearNum + 1);
        float downgearlimit = (1 / (float)carData.numOfGears) * gearNum;

        if (gearNum > 0 && f < downgearlimit)
        {
            gearNum--;
        }

        if (f > upgearlimit && (gearNum < (carData.numOfGears - 1)))
        {
            gearNum++;
        }
    }

    // simple function to add a curved bias towards 1 for a value in the 0-1 range
    private static float CurveFactor(float factor)
    {
        return 1 - (1 - factor) * (1 - factor);
    }

    //=========================================================================================================================


    private void GetInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        if(!braking)
        {
            brake = Input.GetKey(KeyCode.T);
            //Debug.Log(98989898989898989);
        }
        if(Input.GetKeyUp(KeyCode.T))
        {
            brake = false;
            ReleaseBrake();
            //Debug.Log(234234324);
        }

        //boost = Input.GetKey(KeyCode.LeftShift);
    }

    private void Steer()
    {
        if(horizontalInput != 0)
        {
            steeringAngle = carData.maxSteerAngle * horizontalInput * 0.75f;
        }
        else
        {
            steeringAngle = 0;
        }
        wheelColliders[0].steerAngle = steeringAngle;
        wheelColliders[1].steerAngle = steeringAngle;
    }
    
    private void Accelerate()
    {
        if(verticalInput > 0)
        {
            //Debug.Log(1);
        }
        else
        {
            //Debug.Log(2);
        }



        //rb.drag = 0;
        if (wheelColliders[0].brakeTorque > 0 && !braking)
        {
            foreach(WheelCollider wheel in wheelColliders)
            {
                wheel.brakeTorque = 0;
            }
        }

        float thrustTorque;

        if (boost)
        {
            thrustTorque = -verticalInput * (boostTorque / 4f);
        }
        else if (currentSpeed > 10 && verticalInput == 0)
        {
            thrustTorque = -verticalInput * (currentTorque  / 4f * a);
        }
        else 
        {
            thrustTorque = -verticalInput * (currentTorque  / 4f * a);
            
        }
        //Debug.Log(thrustTorque);
        if (wheelColliders[0].brakeTorque == 0 && currentSpeed < carData.topSpeed - 5)
        {
            for (int i = 0; i < 4; i++)
            {
                wheelColliders[i].motorTorque = thrustTorque;
            }
        }


        if(rb.velocity.magnitude > carData.topSpeed)
        {
            Debug.Log(12313);
        }
    }
    float a;

    private void AdjustStiffness()
    {
        if(braking)
        {
            //return;
        }

        float speed = rb.velocity.magnitude;
        a = currentSpeed / carData.topSpeed;
        a = 1 - a;
        
        normal.stiffness = a;

        if (verticalInput == 0)
        {
            normal.stiffness = 0.75f; ;
            rb.drag = 0.2f;
        }

        else if (verticalInput < 0)
        {
            normal.stiffness = 1;
            rb.drag = 0.5f;
            if(currentSpeed >= 100)
            {
                normal.stiffness = 2;
            }
        }
        else
        {
            rb.drag = 0.05f;
        }
        foreach (WheelCollider wheel in wheelColliders)
        {
            wheel.forwardFriction = normal;
        }
    }


    private void TractionControl()
    {
        WheelHit wheelHit;
        for (int i = 0; i < 4; i++)
        {
            wheelColliders[i].GetGroundHit(out wheelHit);

            AdjustTorque(wheelHit.forwardSlip);
        }
        
    }


    // Debug GUI.
    void OnGUI()
    {
        
        if(displayGui)
        {
            //GUILayout.Label("FOV: " + cineCamera.m_Lens.FieldOfView);
            GUILayout.Label("Speed: " + currentSpeed);

            /*
            GUILayout.Label("currentTorque: " + currentTorque);
            GUILayout.Label("boostTorque: " + boostTorque);
            GUILayout.Label("brakeTorque: " + wheelColliders[0].brakeTorque);
            GUILayout.Label("rpm: " + wheelColliders[0].rpm);
            GUILayout.Label("rb vel: " + rb.velocity.magnitude);
            GUILayout.Label("rpm slip in radians per second: " + wheelColliders[0].rpm * 0.10472f * wheelColliders[0].radius);
            GUILayout.Label("stiffeness " + wheelColliders[0].forwardFriction.stiffness);
            */
        }
    }

    private void Brake()
    {
        if (MasterClientRaceStart.Instance.countdownTimerStart)
        {
            if (brake && !braking)
            {
                ApplyBrake(carData.brakeTorque);
            }
            else if (!brake && !braking)
            {
                ReleaseBrake();
            }
        }
    }
    
    public void ApplyBrake(float brakeAmount)
    {
        braking = true;
        normal.stiffness = 1;
        foreach (WheelCollider wheel in wheelColliders)
        {
            wheel.brakeTorque = brakeAmount;
            wheel.forwardFriction = normal;
        }
    }

    public void ReleaseBrake()
    {
        braking = false;
        foreach (WheelCollider wheel in wheelColliders)
        {
            wheel.brakeTorque = 0;
        }
    }


    private void AdjustTorque(float forwardSlip)
    {
        if(boost)
        {
            if (forwardSlip >= carData.slipLimit && boostTorque >= 0)
            {
                boostTorque -= 10 * carData.tractionControl;
            }
            else
            {
                boostTorque += 10 * carData.tractionControl;
                if (boostTorque > carData.boostFullTorqueOverAllWheels)
                {
                    boostTorque = carData.boostFullTorqueOverAllWheels;
                }
            }
        }
        else
        {
            if (forwardSlip >= carData.slipLimit && currentTorque >= 0)
            {
                currentTorque -= 10 * carData.tractionControl;
            }
            else
            {
                currentTorque += 10 * carData.tractionControl;
                if (currentTorque > carData.fullTorqueOverAllWheels)
                {
                    currentTorque = carData.fullTorqueOverAllWheels;
                }
            }
        }
    }


    private void CapSpeed()
    {
        float speed = rb.velocity.magnitude;

        speed *= 2.23693629f;
        if (boost)
        {
            if (speed > carData.boostSpeed)
            {
                rb.velocity = carData.boostSpeed / 2.23693629f * rb.velocity.normalized;
            }
        }
        else
        {
            if (speed > carData.topSpeed)
            {
                rb.velocity = carData.topSpeed / 2.23693629f * rb.velocity.normalized;
            }
        }

    }

    private void UpdateWheelPoses()
    {
        for(int i = 0; i< 4; i++)
        {
            UpdateWheelPose(wheelColliders[i], wheelMeshes[i]);
        }
    }

    private void UpdateWheelPose(WheelCollider collider, Transform transform)
    {
        Vector3 pos = transform.position;
        Quaternion quat = transform.rotation;

        collider.GetWorldPose(out pos, out quat);

        transform.position = pos;
        transform.rotation = quat;
    }



    

    private void Skid()
    {
        float amount = 0;
        WheelHit wheelHit;
        for(int i = 0; i<4;i++)
        {
            wheelColliders[i].GetGroundHit(out wheelHit);
            //Debug.Log(wheelHit.sidewaysSlip);

            if (((wheelHit.forwardSlip > carData.forwardSkidLimit || wheelHit.forwardSlip < -carData.forwardSkidLimit) && skidmarks[i] != null))
            {
                //Not quite ready yet, check back later
                //Vector3 skidPoint = new Vector3(wheelColliders[i].transform.position.x, wheelHit.point.y, wheelColliders[i].transform.position.z) + (rb.velocity * Time.deltaTime);
                //lastSkid[i] = skidmarksController.AddSkidMark(skidPoint, wheelHit.normal, 0.5f, lastSkid[i]);
                //amount += 0.25f;
            }
            else if (( (wheelHit.sidewaysSlip > carData.sideSkidLimit || wheelHit.sidewaysSlip < -carData.sideSkidLimit)) && skidmarks[i] != null)
            {
                Vector3 skidPoint = new Vector3(wheelColliders[i].transform.position.x, wheelHit.point.y, wheelColliders[i].transform.position.z) + (rb.velocity * Time.deltaTime);
                lastSkid[i] = skidmarksController.AddSkidMark(skidPoint, wheelHit.normal, 0.5f, lastSkid[i]);
                amount += 0.25f;
            }
            else
            {
                
                lastSkid[i] = -1;
            }
            
        }
        if(braking)
        {
            //amount = 0.75f;
        }
        if (pv.IsMine && IsThisMultiplayer.Instance.multiplayer) //|| !IsThisMultiplayer.Instance.multiplayer)
        {
            pv.RPC("SetSkidVolume",  RpcTarget.All, amount);
        }
        else
        {
            skidSound.setVolume(amount);
        }
        
    }
    
    [PunRPC]
    void SetSkidVolume(float amount)
    {
        skidSound.setVolume(amount);
    }

    private void ChangeFOV()
    {
        //Debug.Log(currentSpeed);
        if(currentSpeed > cineCamera.m_Lens.FieldOfView )
        {
            cineCamera.m_Lens.FieldOfView = Mathf.Lerp(cineCamera.m_Lens.FieldOfView, currentSpeed, Time.deltaTime);


            if (cineCamera.m_Lens.FieldOfView > carData.maxFOV)
            {
                cineCamera.m_Lens.FieldOfView = carData.maxFOV;
            }
            
        }
        else if (currentSpeed < 40 && cineCamera.m_Lens.FieldOfView > 60)
        {
            cineCamera.m_Lens.FieldOfView = Mathf.Lerp(cineCamera.m_Lens.FieldOfView, 60, Time.deltaTime * 1.5f);
        }
        else if(currentSpeed < cineCamera.m_Lens.FieldOfView - 10)
        {
            cineCamera.m_Lens.FieldOfView = Mathf.Lerp(cineCamera.m_Lens.FieldOfView, 60, Time.deltaTime * 0.1f);
        }

        if(boost)
        {
            cineCamTransposer.m_FollowOffset = Vector3.Lerp(cineCamTransposer.m_FollowOffset, carData.boostCamOffset, Time.deltaTime);
        }
        else if(currentSpeed < 50)
        {
            cineCamTransposer.m_FollowOffset = Vector3.Lerp(cineCamTransposer.m_FollowOffset, carData.stationaryCamOffset, Time.deltaTime);
        }
        else
        {
            cineCamTransposer.m_FollowOffset = Vector3.Lerp(cineCamTransposer.m_FollowOffset, carData.movingCamOffset, currentSpeed * carData.fovChangeSpeed * Time.deltaTime);
        }

    }

   
    

    

    private void AddDownForce()
    {
       rb.AddForce(-transform.up * carData.downforce * rb.velocity.magnitude);
    }

   

    private void Drift()
    {
        
        if (Input.GetKey(KeyCode.Space))
        {
            
            wheelColliders[2].sidewaysFriction = drift;
            wheelColliders[3].sidewaysFriction = drift;
            drifting = true;
        }
        else
        {

            wheelColliders[2].sidewaysFriction = normal;
            wheelColliders[3].sidewaysFriction = normal;
            drifting = false;
        }
    }



    private void HelpSteer()
    {

        for (int i = 0; i < 4; i++)
        {
            WheelHit wheelhit;
            wheelColliders[i].GetGroundHit(out wheelhit);


            // wheels arent on the ground so dont realign the rigidbody velocity
            if (wheelhit.normal == Vector3.zero)
            {
                return;
            }
        }

        // this if is needed to avoid gimbal lock problems that will make the car suddenly shift direction
        if (Mathf.Abs(oldRot - transform.eulerAngles.y) < 10f)
        {
            if(drifting)
            {
                var turnadjust = (transform.eulerAngles.y - oldRot) * carData.steerHelperDrift;
                Quaternion velRotation = Quaternion.AngleAxis(turnadjust, Vector3.up);
                rb.velocity = velRotation * rb.velocity;
            }
            else
            {
                var turnadjust = (transform.eulerAngles.y - oldRot) * carData.steerHelper;
                Quaternion velRotation = Quaternion.AngleAxis(turnadjust, Vector3.up);
                rb.velocity = velRotation * rb.velocity;
            }
        }
        oldRot = transform.eulerAngles.y;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "shooter")
        {
            Shooter = other.gameObject;
        }
    }
}
