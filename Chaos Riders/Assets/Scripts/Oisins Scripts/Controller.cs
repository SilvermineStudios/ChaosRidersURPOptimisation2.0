using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using Photon.Pun;
using UnityEngine.VFX;

public class Controller : MonoBehaviour
{
    #region Cars
    [Header("Car Data")]
    public CarClass currentCarClass;
    CarClass oldCarClass;
    Vehicle carData;
    [SerializeField] Vehicle[] VehicleData;
    [SerializeField] GameObject[] Models;
    GameObject model;
    #endregion

    #region Bools
    [Header("Bools")]
    [SerializeField] bool displayGui = false;
    [SerializeField] bool canChangeCar = false;
    [SerializeField] bool lockAbilitiesToCar = true;
    [HideInInspector] public bool boost;
    public bool braking { get; private set; }
    public bool drifting { get; private set; }
    private bool brake;
    [SerializeField] private bool canUseEquipment = false, canUseAbility = false;
    private bool useOverCharge; //Currently unused
    bool usingEquipment;
    bool usingUltimate;
    bool usingUltimatePrep;
    #endregion

    #region Floats
    [Header("Floats")]
    private float horizontalInput;
    private float verticalInput;
    private float steeringAngle;
    private float oldRot;
    private float currentTorque;
    private float boostTorque;
    public float currentSpeed { get { return rb.velocity.magnitude * 2.23693629f; } private set { } }
    #endregion

    #region Wheels
    [Header("Wheels")]
    [SerializeField] private WheelCollider[] wheelColliders = new WheelCollider[4];
    private Transform[] wheelMeshes = new Transform[4];
    #endregion

    #region Camera
    [Header("Camera")]
    private CinemachineVirtualCamera cineCamera;
    CinemachineTransposer cineCamTransposer;
    #endregion

    #region Scripts
    Skidmarks skidmarksController;
    DriverAbilities driverAbilities;
    PhotonView pv;
    //Health healthScript;
    Target healthScript;
    ShredUltimate shredUltimate;
    //public TurretTester ShooterAttached; 
     public GameObject Shooter;
    protected PlayerInputs playerInputs;
    #endregion

    #region Skidmarks
    [Header("Skidmarks")]
    Skidmarks[] skidmarks = new Skidmarks[4];
    int[] lastSkid = new int[4];
    WheelFrictionCurve drift;
    WheelFrictionCurve defaultForwardFrictionCurve;
    WheelFrictionCurve defaultSidewaysFrictionCurve;
    #endregion

    #region Sound
    [Header("Sound")]
    FMOD.Studio.Bus SkidBus;
    FMOD.Studio.EventInstance skidSound;
    public float Revs { get; private set; }
    private int gearNum;
    private float gearFactor;

    //public string revsoundLocation; //<-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    #endregion

    #region Driver Abilities
    [Header("Driver Abilities")]
    public DriverEquipment CurrentEquipment;
    DriverEquipment OldEquipment;
    public DriverUltimate CurrentUltimate;
    DriverUltimate OldUltimate;
    [SerializeField] private KeyCode abilityKeyCode = KeyCode.Q, equipmentKeyCode = KeyCode.E; //Create Keycode Variables for the buttons
    bool notUsingBrakerAbility = false;
    bool usingBrakerAbility = true;
    #endregion

    #region Ability Data
    [Header("Ability Data")]
    public Equipment[] DriverEquipmentData;
    Equipment equipmentData;
    public Ultimate[] DriverUltimateData;
    Ultimate ultimateData;

    #endregion

    #region Driver Abilities UI
    [Header("Driver Abilities UI")]
    [SerializeField] private Transform equipmentChargeBar; //equipment chargebar
    [SerializeField] private Transform abilityChargeBar; //ability chargebar
    private Transform equipmentOverChargeBar;//not in use
    [SerializeField] private Transform abilityOverChargeBar;
    private float equipmentChargeAmount, equipmentOverchargeAmount, abilityChargeAmount, abilityOverChargeAmount; //equipment/ability charge Amount

    #endregion

    #region Other
    public Rigidbody rb { get; private set; }
    public enum DriverEquipment { SmokeScreen, Mine }
    public enum DriverUltimate { Brake, Shred }
    [SerializeField] private Animator anim;
    [SerializeField] private GameObject abilitySpawn;
    #endregion

    #region Default Functions
    private void Awake()
    {
        
        shredUltimate = GetComponent<ShredUltimate>();
        if(anim == null)
            anim = GetComponentInChildren<Animator>();
        pv = GetComponent<PhotonView>();
        //healthScript = GetComponent<Health>();
        healthScript = GetComponent<Target>();
        driverAbilities = GetComponent<DriverAbilities>();
        playerInputs = GetComponent<PlayerInputs>();
        rb = GetComponent<Rigidbody>();

        skidmarksController = FindObjectOfType<Skidmarks>();
        skidSound = FMODUnity.RuntimeManager.CreateInstance("event:/CarFX/All/Skid");
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(skidSound, transform, rb);
        skidSound.start();
        skidSound.setVolume(0);

        if (smokeTrail != null)
            smokeTrail.SetActive(false);
    }

    private void Start()
    {
        SetupCar(currentCarClass);
        if(currentCarClass == CarClass.Braker)
        {
            smokeBall = PhotonNetwork.Instantiate(equipmentData.photonPrefab, Vector3.zero, Quaternion.identity, 0);
            smokeBall.GetComponent<VisualEffect>().Stop();
        }
        cineCamera = gameObject.transform.GetChild(0).gameObject.GetComponent<CinemachineVirtualCamera>();
        cineCamTransposer = cineCamera.GetCinemachineComponent<CinemachineTransposer>();
        cineCamTransposer.m_FollowOffset = carData.stationaryCamOffset;

        skidmarks[0] = skidmarksController;
        skidmarks[1] = skidmarksController;
        skidmarks[2] = skidmarksController;
        skidmarks[3] = skidmarksController;


        rb.centerOfMass = carData.centerOfMass;
        rb.mass = carData.vehicleMass;
        rb.drag = carData.vehicleDrag;
        rb.angularDrag = carData.vehicleAngularDrag;
        currentTorque = carData.fullTorqueOverAllWheels - (carData.tractionControl * carData.fullTorqueOverAllWheels);
        boostTorque = carData.boostFullTorqueOverAllWheels;
        if (pv.IsMine && IsThisMultiplayer.Instance.multiplayer || !IsThisMultiplayer.Instance.multiplayer)
        {
            ResetAllBars(); //set all the bars to 0
            CheckIfCanUseEquipmentAndAbility(); //check if the player can use their equipment
        }
    }

    private void Update()
    {
        //Debug.Log("Muh name is: " + pv.Owner.NickName);
        GetInput();
        /*
        if (Input.GetKeyDown(KeyCode.R))
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + 2f, transform.position.z - 1f);
            transform.rotation = Quaternion.Euler(0, transform.localRotation.y, 0);
            
        }
        */
        if ((Input.GetKeyDown(KeyCode.Mouse1) && canChangeCar))
        {
            if (currentCarClass == CarClass.Braker)
            {
                currentCarClass = CarClass.Shredder;
            }
            else
            {
                currentCarClass = CarClass.Braker;
            }
        }

        if (oldCarClass != currentCarClass)
        {
            SetupCar(currentCarClass);
        }

        if (pv.IsMine && IsThisMultiplayer.Instance.multiplayer && MasterClientRaceStart.Instance.weaponsFree || !IsThisMultiplayer.Instance.multiplayer)
        {
            DriverAbilities();
        }


        //rev sound <-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //if (Input.GetKeyDown(KeyCode.W))
        //{
            //FMODUnity.RuntimeManager.PlayOneShotAttached(revsoundLocation, gameObject);
        //}
    }

    private void FixedUpdate()
    {
        /*
        if (healthScript.isDead)
        {
            GetComponent<Rigidbody>().drag = 5;
            return;
        }
        else
        {
            GetComponent<Rigidbody>().drag = 0;
        }
        */

        if (healthScript.isDead)
        {
            GetComponent<Rigidbody>().drag = 5;
            return;
        }
        else
            GetComponent<Rigidbody>().drag = 0;

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

    private void GetInput()
    {
        horizontalInput = Input.GetAxis("Horizontal") * 0.5f;

        if (Input.GetAxis("RT") > 0 || Input.GetAxis("LT") > 0)
        {
            if (Input.GetAxis("RT") > 0)
            {
                verticalInput = Input.GetAxis("RT");
            }
            else
            {
                verticalInput = -Input.GetAxis("LT");
            }
        }
        else
        {
            verticalInput = Input.GetAxis("Vertical");
        }
        if (!braking)
        {
            brake = Input.GetButton("B");
            //Debug.Log(98989898989898989);
        }
        if (Input.GetButtonUp("B"))
        {
            brake = false;
            ReleaseBrake();
            //Debug.Log(234234324);
        }

        //boost = Input.GetKey(KeyCode.LeftShift);
        //playerInputs.Input.DriveX = verticalInput;
        //playerInputs.Input.DriveZ = horizontalInput;
    }
    #endregion

    #region Setup
    void SetupCar(CarClass car)
    {
        //turn all models off
        foreach (GameObject model in Models)
        {
            model.SetActive(false);
        }

        //switch to right car and car data
        switch (car)
        {
            case CarClass.Braker:
                carData = VehicleData[0];
                model = Models[0];
                break;
            case CarClass.Shredder:
                carData = VehicleData[1];
                model = Models[1];
                break;
            default:
                carData = VehicleData[0];
                model = Models[0];
                break;
        }

        //turn model on
        model.SetActive(true);

        //Get position to spawn equipment from
        //abilitySpawn = model.transform.GetChild(1).gameObject;

        //add models wheel meshes to array
        for (int i = 0; i < 4; i++)
        {
            wheelMeshes[i] = model.transform.GetChild(0).GetChild(i);
        }

        foreach (WheelCollider wheelCollider in wheelColliders)
        {
            SetupWheel(wheelCollider);
        }
        defaultForwardFrictionCurve = carData.forwardFriction;
        defaultSidewaysFrictionCurve = carData.sideFriction;

        if (lockAbilitiesToCar)
        {
            if (currentCarClass == CarClass.Braker)
            {
                CurrentEquipment = DriverEquipment.SmokeScreen;
                CurrentUltimate = DriverUltimate.Brake;
            }

            if (currentCarClass == CarClass.Shredder)
            {
                CurrentEquipment = DriverEquipment.Mine;
                CurrentUltimate = DriverUltimate.Shred;
            }
        }
        SetupEquipment(CurrentEquipment);
        SetupUltimate(CurrentUltimate);
        oldCarClass = car;
    }

    void SetupWheel(WheelCollider wheel)
    {
        //Main
        wheel.mass = carData.wheelMass;
        wheel.radius = carData.wheelRadius;
        wheel.wheelDampingRate = carData.wheelDampingRate;
        wheel.suspensionDistance = carData.wheelSuspensionDistance;
        wheel.forceAppPointDistance = carData.wheelForceAppPontDistance;
        wheel.center = carData.wheelCenter;

        //Suspension
        wheel.suspensionSpring = carData.suspension;

        //Forward Friction
        wheel.forwardFriction = carData.forwardFriction;

        //Side Friction
        wheel.sidewaysFriction = carData.sideFriction;
    }

    void SetupEquipment(DriverEquipment equipment)
    {
        //switch to right equipment
        switch (equipment)
        {
            case DriverEquipment.SmokeScreen:
                equipmentData = DriverEquipmentData[0];
                break;
            case DriverEquipment.Mine:
                equipmentData = DriverEquipmentData[1];
                break;
            default:
                equipmentData = DriverEquipmentData[0];
                break;
        }
        OldEquipment = equipment;
    }

    void SetupUltimate(DriverUltimate ultimate)
    {
        //switch to right ultimate
        switch (ultimate)
        {
            case DriverUltimate.Brake:
                ultimateData = DriverUltimateData[0];
                break;
            case DriverUltimate.Shred:
                ultimateData = DriverUltimateData[1];
                break;
            default:
                ultimateData = DriverUltimateData[0];
                break;
        }
        OldUltimate = ultimate;
    }

    private void StartRace()
    {
        if (!MasterClientRaceStart.Instance.countdownTimerStart)
        {
            ApplyBrake(carData.brakeTorque, notUsingBrakerAbility);
        }
        else
        {
            ReleaseBrake();
        }
    }
    #endregion

    #region Driver Abilities

    [SerializeField] private GameObject smokeTrail;
    [SerializeField] private float smokeTrailActiveTime = 3f;

    void DriverAbilities()
    {
        ChargeBars(); //charge the equipment and ability bars
        CheckIfCanUseEquipmentAndAbility(); //check if the player can use their equipment

        //if you use the equipment
        if (Input.GetButtonDown("A") && canUseEquipment)
        {
            usingEquipment = true;
            StartCoroutine(UseEquipmentUI(equipmentData.equipmentUseTime));
            FMODUnity.RuntimeManager.PlayOneShotAttached(equipmentData.sound, gameObject);
            //spawn the smoke grenade accross the network
            if (IsThisMultiplayer.Instance.multiplayer)
            {
                pv.RPC("SendSmoke", RpcTarget.All);
            }
            //spawn the smoke grenade in single player
            if (!IsThisMultiplayer.Instance.multiplayer)
            {
                Instantiate(equipmentData.prefab, abilitySpawn.transform.position, abilitySpawn.transform.rotation);
            }
            //equipmentChargeAmount = 0; //reset the cooldownbar after the equipment is used

            //<------------------------------------------------------------------------------------------------------------NEW--------------------------------------------------------------
            if (currentCarClass == CarClass.Braker && smokeTrail != null)
            {
                StartCoroutine(SmokeTrailCourotine(smokeTrailActiveTime));
            }
            else
                Debug.Log("Error, Something is wrong with the smoke trail click here");
        }

        

        //if you use the Ability
        if (Input.GetButtonDown("X") && canUseAbility)
        {

            if (CurrentUltimate == DriverUltimate.Brake)
            {
                StartCoroutine(UseBrakerAbility());

            }
            if (CurrentUltimate == DriverUltimate.Shred)
            {
                StartCoroutine(UseShredderAbility());
            }
            abilityChargeAmount = 0; //reset the cooldownbar after the ability is used
        }

    }

    GameObject smokeBall;

    [PunRPC]
    void SendSmoke()
    {
        smokeBall.transform.position = abilitySpawn.transform.position;
        smokeBall.transform.rotation = abilitySpawn.transform.rotation;
        smokeBall.GetComponent<VisualEffect>().Play();
        smokeBall.GetComponent<TurnOff>().TriggerMe();
    }

    private IEnumerator SmokeTrailCourotine(float time)
    {
        smokeTrail.SetActive(true);

        yield return new WaitForSeconds(time);

        smokeTrail.SetActive(false);
    }

    #region Ultimates

    private IEnumerator UseBrakerAbility()
    {
        usingUltimate = true;
        abilityOverChargeAmount = 100;
        StartCoroutine(UseUltimateUIPrep(ultimateData.ultimatePrepTime));

        Debug.Log("BRAKER ANIMATION STUFF HERE");
        //anim.SetTrigger("BreakerTransTrigger");
        anim.SetBool("StartBreaker", true);
        anim.SetBool("LeaveBreaker", false);

        //brake
        ApplyBrake(30000000, usingBrakerAbility);


        yield return new WaitForSeconds(ultimateData.ultimatePrepTime);
        

        ReleaseBrake();

        //anim.SetTrigger("LeaveBreakerTrigger");
        anim.SetBool("StartBreaker", false);
        anim.SetBool("LeaveBreaker", true);

        //speed
        StartCoroutine(UseUltimateUI(ultimateData.ultimateUsetime));
        boost = true;

        yield return new WaitForSeconds(ultimateData.ultimateUsetime);
        boost = false;
        usingUltimate = false;
    }

    private IEnumerator UseShredderAbility()
    {
        usingUltimate = true;
        FMODUnity.RuntimeManager.PlayOneShotAttached(ultimateData.sound, gameObject);
        //FMODUnity.RuntimeManager.PlayOneShotAttached(equipmentData.sound, gameObject);
        shredUltimate.enabled = true;
        StartCoroutine(UseUltimateUI(ultimateData.ultimateUsetime));


        //anim.SetBool("StartShred", true);
        //anim.SetBool("LeaveShred", false);
        anim.SetBool("Shred", true);

        yield return new WaitForSeconds(ultimateData.ultimateUsetime);

        //anim.SetBool("StartShred", false);
        //anim.SetBool("LeaveShred", true);
        anim.SetBool("Shred", false);

        shredUltimate.enabled = false;
        usingUltimate = false;
    }

    #endregion

    float timeElapsed = 0;

    #region Charge Bars
    //sets all bars to 0
    private void ResetAllBars()
    {
        equipmentChargeBar.GetComponent<Image>().fillAmount = 0;
        //equipmentOverChargeBar.GetComponent<Image>().fillAmount = 0;
        abilityChargeBar.GetComponent<Image>().fillAmount = 0;
        //abilityOverChargeBar.GetComponent<Image>().fillAmount = 0;
    }

    //check if the player can use their equipment
    private void CheckIfCanUseEquipmentAndAbility()
    {
        //check if the equipment bar is full
        if (equipmentChargeAmount >= 100)
            canUseEquipment = true;
        else
            canUseEquipment = false;


        //check if the ability bar is full
        if (abilityChargeAmount >= 100)
            canUseAbility = true;
        else
            canUseAbility = false;
    }

    IEnumerator UseEquipmentUI(float timeTomove)
    {
        var t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime / timeTomove;
            equipmentChargeAmount = Mathf.Lerp(100, 0, t);
            equipmentChargeBar.GetComponent<Image>().fillAmount = equipmentChargeAmount / 100;

            if (equipmentChargeAmount == 0)
            {
                usingEquipment = false;
            }
            yield return null;
        }

    }

    IEnumerator UseUltimateUIPrep(float timeTomove)
    {
        var t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime / timeTomove;
            abilityOverChargeAmount = Mathf.Lerp(100, 0, t);
            abilityOverChargeBar.GetComponent<Image>().fillAmount = abilityOverChargeAmount / 100;

            if (abilityOverChargeAmount == 0)
            {
                usingUltimatePrep = false;
            }
            yield return null;
        }

    }

    IEnumerator UseUltimateUI(float timeTomove)
    {
        var t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime / timeTomove;
            abilityChargeAmount = Mathf.Lerp(100, 0, t);
            abilityChargeBar.GetComponent<Image>().fillAmount = abilityChargeAmount / 100;

            if (equipmentChargeAmount == 0)
            {
                usingUltimate = false;
            }
            yield return null;
        }

    }

    //charge the equipment and ability bars
    private void ChargeBars()
    {
        if(!usingEquipment)
        {
            //if the qeuipment bar isnt full add to it
            if (equipmentChargeAmount < 100)
            {
                equipmentChargeAmount += equipmentData.chargeRate * Time.deltaTime;
                equipmentChargeBar.GetComponent<Image>().fillAmount = equipmentChargeAmount / 100;
            }
        }
        if (!usingUltimate)
        {
            //if the ability bar isnt full add to it
            if (abilityChargeAmount < 100)
            {
                abilityChargeAmount += ultimateData.chargeRate * Time.deltaTime;
                abilityChargeBar.GetComponent<Image>().fillAmount = abilityChargeAmount / 100;
            }
        }
        if (useOverCharge)
        {
            //if the ability bar is full add to the overcharge bar
            if (abilityChargeAmount >= 100)
            {
                //abilityOverChargeAmount += abilityChargeSpeed * Time.deltaTime;

            }
        }
    }
    #endregion
    #endregion

    #region Audio

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
    #endregion

    #region Driving

    // simple function to add a curved bias towards 1 for a value in the 0-1 range
    private static float CurveFactor(float factor)
    {
        return 1 - (1 - factor) * (1 - factor);
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

        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        foreach (WheelCollider wheel in wheelColliders)
        {
            Debug.Log(wheel.gameObject.name + " Motor Torque = " + wheel.motorTorque);
        }

        /*
        //holding s
        if (verticalInput < 0)
        {
            //ApplyBrake(1000, usingBrakerAbility);

            foreach (WheelCollider wheel in wheelColliders)
            {
                wheel.motorTorque = -1000;

                //Debug.Log(wheel.gameObject.name + " Motor Torque = " + wheel.motorTorque);

                if (wheel.motorTorque > 1)
                {
                    //defaultForwardFrictionCurve.stiffness = 2;
                    //wheel.forwardFriction = defaultForwardFrictionCurve;
                    //wheel.motorTorque = 2;
                    //rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, rb.velocity.z - rb.velocity.z);
                }
            }
        }

        */






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

        defaultForwardFrictionCurve.stiffness = a;

        if (verticalInput == 0)
        {
            defaultForwardFrictionCurve.stiffness = 0.75f; ;
            rb.drag = 0.2f;
        }

        else if (verticalInput < 0)
        {
            defaultForwardFrictionCurve.stiffness = 1;
            rb.drag = 0.5f;
            if(currentSpeed >= 100)
            {
                defaultForwardFrictionCurve.stiffness = 2;
            }
        }
        else
        {
            rb.drag = 0.05f;
        }
        foreach (WheelCollider wheel in wheelColliders)
        {
            wheel.forwardFriction = defaultForwardFrictionCurve;
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
        if (MasterClientRaceStart.Instance.countdownTimerStart)
        {
            if (brake && !braking)
            {
                ApplyBrake(carData.brakeTorque, notUsingBrakerAbility);
            }
            else if (!brake && !braking)
            {
                ReleaseBrake();
            }
        }
    }

    public void ApplyBrake(float brakeAmount, bool brakerAbility)
    {
        braking = true;

        if (brakerAbility)
        {
            //<-------------------------------------------------------------------------------------------------------------------------------------------------BRAKER ABILITY
            Debug.Log("Using Brake Ability");

            defaultForwardFrictionCurve.stiffness = 200f;
            foreach (WheelCollider wheel in wheelColliders)
            {
                wheel.motorTorque = 0;
                //wheel.brakeTorque = brakeAmount;
                wheel.forwardFriction = defaultForwardFrictionCurve;
            }
        }
        else
        {
            defaultForwardFrictionCurve.stiffness = 1;
            foreach (WheelCollider wheel in wheelColliders)
            {
                wheel.brakeTorque = brakeAmount;
                wheel.forwardFriction = defaultForwardFrictionCurve;
            }
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

            wheelColliders[2].sidewaysFriction = defaultSidewaysFrictionCurve;
            wheelColliders[3].sidewaysFriction = defaultSidewaysFrictionCurve;
            drifting = false;
        }
    }

    private void AddDownForce()
    {
        rb.AddForce(-transform.up * carData.downforce * rb.velocity.magnitude);
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
            if (drifting)
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
    #endregion

    #region Driving Aesthetics

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


    private void ChangeFOV()
    {
        //Debug.Log(currentSpeed);
        if (currentSpeed > cineCamera.m_Lens.FieldOfView)
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
        else if (currentSpeed < cineCamera.m_Lens.FieldOfView - 10)
        {
            cineCamera.m_Lens.FieldOfView = Mathf.Lerp(cineCamera.m_Lens.FieldOfView, 60, Time.deltaTime * 0.1f);
        }

        if (boost)
        {
            cineCamTransposer.m_FollowOffset = Vector3.Lerp(cineCamTransposer.m_FollowOffset, carData.boostCamOffset, Time.deltaTime);
        }
        else if (currentSpeed < 50)
        {
            cineCamTransposer.m_FollowOffset = Vector3.Lerp(cineCamTransposer.m_FollowOffset, carData.stationaryCamOffset, Time.deltaTime);
        }
        else
        {
            cineCamTransposer.m_FollowOffset = Vector3.Lerp(cineCamTransposer.m_FollowOffset, carData.movingCamOffset, currentSpeed * carData.fovChangeSpeed * Time.deltaTime);
        }

    }
    #endregion

    #region Skidding

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

    #endregion

    #region Shooter Connect
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "shooter")
        {
            Shooter = other.gameObject;
        }
    }
    #endregion

    // Debug GUI.
    void OnGUI()
    {

        if (displayGui)
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
}
