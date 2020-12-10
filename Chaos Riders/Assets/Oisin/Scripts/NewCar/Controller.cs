using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


public class Controller : MonoBehaviour
{
    [SerializeField] private WheelCollider[] wheelColliders = new WheelCollider[4];
    [SerializeField] private Transform[] wheelMeshes = new Transform[4];
    [SerializeField] private Vector3 centerOfMass;
    [Range(0, 1)] [SerializeField] private float steerHelper;
    [SerializeField] private float tractionControl = 0;
    [SerializeField] private float fullTorqueOverAllWheels;
    [SerializeField] private float boostFullTorqueOverAllWheels;
    [SerializeField] private float brakeTorque;
    [SerializeField] private float reverseTorque;
    [SerializeField] private float topSpeed;
    [SerializeField] private float boostSpeed;
    [SerializeField] private float downforce;
    [SerializeField] float maxSteerAngle = 30;
    [SerializeField] float maxFOV;
    [SerializeField] float boostFOV;
    [SerializeField] private Vector3 stationaryCamOffset;
    [SerializeField] private Vector3 movingCamOffset;
    [SerializeField] private Vector3 boostCamOffset;
    private CinemachineVirtualCamera cineCamera;
    Skidmarks skidmarksController;
    private float horizontalInput;
    private float verticalInput;
    private float steeringAngle;
    public bool boost;
    public bool brake, braking;

    Rigidbody rb;
    float oldRot;
    float steerAngle;
    private float currentTorque;
    private float boostTorque;
    public float currentSpeed { get { return rb.velocity.magnitude * 2.23693629f; } private set { } }
    float slipLimit = 0.3f;
    float skidLimit = 0.5f;
    Skidmarks[] skidmarks = new Skidmarks[4];
    int[] lastSkid = new int[4];
    CinemachineTransposer cineCamTransposer;
    WheelFrictionCurve drift;
    WheelFrictionCurve normal;
    float num = 0.002f;

    private void Awake()
    {
        skidmarksController = FindObjectOfType<Skidmarks>();
    }

    private void Start()
    {

        skidmarks[0] = skidmarksController;
        skidmarks[1] = skidmarksController;
        skidmarks[2] = skidmarksController;
        skidmarks[3] = skidmarksController;

        /*

        drift.extremumSlip = 0.1f;
        drift.extremumValue = 1;
        drift.asymptoteSlip = 0.5f;
        drift.asymptoteValue = 0.75f;
        drift.stiffness = 0.8f;

        normal.extremumSlip = 0.1f;
        normal.extremumValue = 1;
        normal.asymptoteSlip = 0.5f;
        normal.asymptoteValue = 0.75f;
        normal.stiffness = 1;

        */

        cineCamera = gameObject.transform.GetChild(1).gameObject.GetComponent<CinemachineVirtualCamera>();
        cineCamTransposer = cineCamera.GetCinemachineComponent<CinemachineTransposer>();
        cineCamTransposer.m_FollowOffset = stationaryCamOffset;
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = centerOfMass;
        currentTorque = fullTorqueOverAllWheels - (tractionControl * fullTorqueOverAllWheels);
        boostTorque = boostFullTorqueOverAllWheels;
    }

    private void FixedUpdate()
    {
        GetInput();
        Brake();
        Accelerate();
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



    private void GetInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        //brake = Input.GetKey(KeyCode.T);
        //boost = Input.GetKey(KeyCode.LeftShift);
    }

    private void Steer()
    {
        if(horizontalInput != 0)
        {
            steeringAngle = maxSteerAngle * horizontalInput;
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
        if(wheelColliders[0].brakeTorque > 0 && !braking)
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
        else
        {
            thrustTorque = -verticalInput * (currentTorque / 4f);
        }

        if (wheelColliders[0].brakeTorque == 0)
        {
            for (int i = 0; i < 4; i++)
            {
                wheelColliders[i].motorTorque = thrustTorque;
            }
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

    

    private void Brake()
    {
        if(brake && !braking)
        {
            ApplyBrake(brakeTorque);
        }
        else if(!brake && !braking)
        {
            ReleaseBrake();
        }
    }
    
    public void ApplyBrake(float brakeAmount)
    {
        braking = true;
        foreach (WheelCollider wheel in wheelColliders)
        {
            wheel.brakeTorque = brakeAmount;
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
            if (forwardSlip >= slipLimit && boostTorque >= 0)
            {
                boostTorque -= 10 * tractionControl;
            }
            else
            {
                boostTorque += 10 * tractionControl;
                if (boostTorque > boostFullTorqueOverAllWheels)
                {
                    boostTorque = boostFullTorqueOverAllWheels;
                }
            }
        }
        else
        {
            if (forwardSlip >= slipLimit && currentTorque >= 0)
            {
                currentTorque -= 10 * tractionControl;
            }
            else
            {
                currentTorque += 10 * tractionControl;
                if (currentTorque > fullTorqueOverAllWheels)
                {
                    currentTorque = fullTorqueOverAllWheels;
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
            if (speed > boostSpeed)
            {
                rb.velocity = boostSpeed / 2.23693629f * rb.velocity.normalized;
            }
        }
        else
        {
            if (speed > topSpeed)
            {
                rb.velocity = topSpeed / 2.23693629f * rb.velocity.normalized;
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
        WheelHit wheelHit;
        for(int i = 0; i<4;i++)
        {
            wheelColliders[i].GetGroundHit(out wheelHit);
            //Debug.Log(wheelHit.sidewaysSlip);

            if (((wheelHit.forwardSlip > skidLimit || wheelHit.forwardSlip < -skidLimit) || (wheelHit.sidewaysSlip > skidLimit || wheelHit.sidewaysSlip < -skidLimit)) && skidmarks[i] != null)
            {
                Vector3 skidPoint = new Vector3(wheelColliders[i].transform.position.x, wheelHit.point.y, wheelColliders[i].transform.position.z) + (rb.velocity * Time.deltaTime);
                lastSkid[i] = skidmarksController.AddSkidMark(skidPoint, wheelHit.normal, 0.5f, lastSkid[i]);
            }
            else
            {
                lastSkid[i] = -1;
            }
            
        }
    }

    private void ChangeFOV()
    {
        //Debug.Log(currentSpeed);
        if(currentSpeed > cineCamera.m_Lens.FieldOfView )
        {
            cineCamera.m_Lens.FieldOfView = Mathf.Lerp(cineCamera.m_Lens.FieldOfView, currentSpeed, Time.deltaTime);


            if (cineCamera.m_Lens.FieldOfView > maxFOV)
            {
                cineCamera.m_Lens.FieldOfView = maxFOV;
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
            cineCamTransposer.m_FollowOffset = Vector3.Lerp(cineCamTransposer.m_FollowOffset, boostCamOffset, Time.deltaTime);
        }
        else if(currentSpeed < 50)
        {
            cineCamTransposer.m_FollowOffset = Vector3.Lerp(cineCamTransposer.m_FollowOffset, stationaryCamOffset, Time.deltaTime);
        }
        else
        {
            cineCamTransposer.m_FollowOffset = Vector3.Lerp(cineCamTransposer.m_FollowOffset, movingCamOffset, currentSpeed * num * Time.deltaTime);
        }

    }

   

    // Debug GUI.
    void OnGUI()
    {
        GUILayout.Label("FOV: " + cineCamera.m_Lens.FieldOfView);
        GUILayout.Label("Speed: " + currentSpeed);
        GUILayout.Label("currentTorque: " + currentTorque);
        GUILayout.Label("boostTorque: " + boostTorque);
        GUILayout.Label("brakeTorque: " + wheelColliders[0].brakeTorque);
        
    }


    private void AddDownForce()
    {
       rb.AddForce(-transform.up * downforce * rb.velocity.magnitude);
    }

   

    private void Drift()
    {
        
        if (Input.GetKey(KeyCode.Space))
        {
            
            wheelColliders[2].sidewaysFriction = drift;
            wheelColliders[3].sidewaysFriction = drift;
            steerHelper = 1;
        }
        else
        {

            wheelColliders[2].sidewaysFriction = normal;
            wheelColliders[3].sidewaysFriction = normal;
            steerHelper = 0.662f;
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
            var turnadjust = (transform.eulerAngles.y - oldRot) * steerHelper;
            Quaternion velRotation = Quaternion.AngleAxis(turnadjust, Vector3.up);
            rb.velocity = velRotation * rb.velocity;
        }
        oldRot = transform.eulerAngles.y;
    }

    
}
