using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

// This class is repsonsible for controlling inputs to the car.
[RequireComponent(typeof(DriveTrainMultiplayer))]
[RequireComponent(typeof(PhotonView))]
public class CarControllerMultiplayer : MonoBehaviour
{
    #region Variables
    private PhotonView pv;
    [SerializeField] private GameObject playerCamera;
    [SerializeField] private bool multiplayer = true;

    // Add all wheels of the car here, so brake and steering forces can be applied to them.
    public WheelMultiplayer[] wheels;

    // A transform object which marks the car's center of gravity.
    // Cars with a higher CoG tend to tilt more in corners.
    // The further the CoG is towards the rear of the car, the more the car tends to oversteer. 
    // If this is not set, the center of mass is calculated from the colliders.
    public Transform centerOfMass;

    // A factor applied to the car's inertia tensor. 
    // Unity calculates the inertia tensor based on the car's collider shape.
    // This factor lets you scale the tensor, in order to make the car more or less dynamic.
    // A higher inertia makes the car change direction slower, which can make it easier to respond to.
    public float inertiaFactor = 1.5f;

    // current input state
    [HideInInspector]
    public float brake;
    [HideInInspector]
    float throttle;
    float throttleInput;
    float clutch;
    [HideInInspector]
    public float steering;
    float lastShiftTime = -1;
    [HideInInspector]
    public float handbrake;

    // cached Drivetrain reference
    DriveTrainMultiplayer drivetrain;

    // How long the car takes to shift gears
    public float shiftSpeed = 0.8f;


    // These values determine how fast throttle value is changed when the accelerate keys are pressed or released.
    // Getting these right is important to make the car controllable, as keyboard input does not allow analogue input.
    // There are different values for when the wheels have full traction and when there are spinning, to implement 
    // traction control schemes.

    // How long it takes to fully engage the throttle
    public float throttleTime = 1.0f;
    // How long it takes to fully engage the throttle 
    // when the wheels are spinning (and traction control is disabled)
    public float throttleTimeTraction = 10.0f;
    // How long it takes to fully release the throttle
    public float throttleReleaseTime = 0.5f;
    // How long it takes to fully release the throttle 
    // when the wheels are spinning.
    public float throttleReleaseTimeTraction = 0.1f;

    // Turn traction control on or off
    public bool tractionControl = false;


    // These values determine how fast steering value is changed when the steering keys are pressed or released.
    // Getting these right is important to make the car controllable, as keyboard input does not allow analogue input.

    // How long it takes to fully turn the steering wheel from center to full lock
    public float steerTime = 1.2f;
    // This is added to steerTime per m/s of velocity, so steering is slower when the car is moving faster.
    public float veloSteerTime = 0.1f;

    // How long it takes to fully turn the steering wheel from full lock to center
    public float steerReleaseTime = 0.6f;
    // This is added to steerReleaseTime per m/s of velocity, so steering is slower when the car is moving faster.
    public float veloSteerReleaseTime = 0f;
    // When detecting a situation where the player tries to counter steer to correct an oversteer situation,
    // steering speed will be multiplied by the difference between optimal and current steering times this 
    // factor, to make the correction easier.
    public float steerCorrectionFactor = 4.0f;


    private bool gearShifted = false;
    private bool gearShiftedFlag = false;

    // Used by SoundController to get average slip velo of all wheels for skid sounds.
    public float slipVelo
    {
        get
        {
            float val = 0.0f;
            foreach (WheelMultiplayer w in wheels)
                val += w.slipVelo / wheels.Length;
            return val;
        }

    }
    #endregion
    // Initialize
    private void Awake()
    {
        pv = GetComponent<PhotonView>();
    }
    void Start()
    {
        if(pv.IsMine && IsThisMultiplayer.Instance.multiplayer)
        {
            playerCamera.SetActive(true);

            if (centerOfMass != null)
                GetComponent<Rigidbody>().centerOfMass = centerOfMass.localPosition;

            GetComponent<Rigidbody>().inertiaTensor *= inertiaFactor;
            drivetrain = GetComponent(typeof(DriveTrainMultiplayer)) as DriveTrainMultiplayer;
        }
        else if(!IsThisMultiplayer.Instance.multiplayer)
        {
            playerCamera.SetActive(true);

            if (centerOfMass != null)
                GetComponent<Rigidbody>().centerOfMass = centerOfMass.localPosition;

            GetComponent<Rigidbody>().inertiaTensor *= inertiaFactor;
            drivetrain = GetComponent(typeof(DriveTrainMultiplayer)) as DriveTrainMultiplayer;
        }
    }

    void Update()
    {
        if(pv.IsMine && IsThisMultiplayer.Instance.multiplayer)
        {
            PlayerUpdate();
        }
        else if(!IsThisMultiplayer.Instance.multiplayer)
        {
            PlayerUpdate();
        }
    }

    private bool GasPedal()
    {
        if (Input.GetAxis("RT") > 0.1f || Input.GetKey(KeyCode.W))
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    private bool BreakPedal()
    {
        if (Input.GetAxis("LT") > 0.1f || Input.GetKey(KeyCode.S))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //all of the stuff that was in update is now here
    private void PlayerUpdate()
    {
        if (drivetrain.gear <= 3 || drivetrain.nitro)
        {
            GetComponent<Rigidbody>().angularDrag = 0.1f;
        }
        else
        {
            GetComponent<Rigidbody>().angularDrag = 0.01f;
        }


        // Steering
        Vector3 carDir = transform.forward;
        float fVelo = GetComponent<Rigidbody>().velocity.magnitude;
        Vector3 veloDir = GetComponent<Rigidbody>().velocity * (1 / fVelo);
        float angle = -Mathf.Asin(Mathf.Clamp(Vector3.Cross(veloDir, carDir).y, -1, 1));
        float optimalSteering = angle / (wheels[0].maxSteeringAngle * Mathf.Deg2Rad);
        if (fVelo < 1)
        {
            optimalSteering = 0;
        }

        float steerInput = 0;

        steerInput = Input.GetAxis("Horizontal");

        if (steerInput < steering)
        {
            float steerSpeed = (steering > 0) ? (1 / (steerReleaseTime + veloSteerReleaseTime * fVelo)) : (1 / (steerTime + veloSteerTime * fVelo));

            if (steering > optimalSteering)
            {
                steerSpeed *= 1 + (steering - optimalSteering) * steerCorrectionFactor;
            }
            steering -= steerSpeed * Time.deltaTime;

            if (steerInput > steering)
            {
                steering = steerInput;
            }
        }
        else if (steerInput > steering)
        {
            float steerSpeed = (steering < 0) ? (1 / (steerReleaseTime + veloSteerReleaseTime * fVelo)) : (1 / (steerTime + veloSteerTime * fVelo));
            if (steering < optimalSteering)
            {
                steerSpeed *= 1 + (optimalSteering - steering) * steerCorrectionFactor;
            }
            steering += steerSpeed * Time.deltaTime;
            if (steerInput < steering)
            {
                steering = steerInput;
            }
        }


        bool accelKey = GasPedal();
        bool brakeKey = BreakPedal();

        if (drivetrain.automatic && drivetrain.gear == 0)
        {
            accelKey = BreakPedal();
            brakeKey = GasPedal();
        }

        if (accelKey)
        {

            if (drivetrain.slipRatio < 0.10f)
            {
                throttle += Time.deltaTime / throttleTime;
            }
            else if (!tractionControl)
            {
                throttle += Time.deltaTime / throttleTimeTraction;
            }
            else
            {
                throttle -= Time.deltaTime / throttleReleaseTime;
            }

            if (throttleInput < 0)
            {
                throttleInput = 0;
            }

            throttleInput += Time.deltaTime / throttleTime;
        }
        else
        {
            if (drivetrain.slipRatio < 0.2f)
            {
                throttle -= Time.deltaTime / throttleReleaseTime;
            }
            else
            {
                throttle -= Time.deltaTime / throttleReleaseTimeTraction;
            }
        }

        throttle = Mathf.Clamp01(throttle);

        if (brakeKey)
        {
            if (drivetrain.slipRatio < 0.2f)
            {
                brake += Time.deltaTime / throttleTime;
            }
            else
            {
                brake += Time.deltaTime / throttleTimeTraction;
            }
            throttle = 0;
            throttleInput -= Time.deltaTime / throttleTime;
        }
        else
        {
            if (drivetrain.slipRatio < 0.2f)
            {
                brake -= Time.deltaTime / throttleReleaseTime;
            }
            else
            {
                brake -= Time.deltaTime / throttleReleaseTimeTraction;
            }
        }

        brake = Mathf.Clamp01(brake);
        throttleInput = Mathf.Clamp(throttleInput, -1, 1);

        // Handbrake
        handbrake = (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.JoystickButton2)) ? 1f : 0f;

        // Gear shifting
        float shiftThrottleFactor = Mathf.Clamp01((Time.time - lastShiftTime) / shiftSpeed);

        if (drivetrain.gear == 0 && GasPedal())
        {
            throttle = 0.4f;// Anti reverse lock thingy??
        }

        if (drivetrain.gear == 0)
        {
            drivetrain.throttle = GasPedal() ? throttle : 0f;
        }
        else
        {
            drivetrain.throttle = GasPedal() ? (tractionControl ? throttle : 1) * shiftThrottleFactor : 0f;
        }

        drivetrain.throttleInput = throttleInput;


        //play gear shift sound
        if (gearShifted && gearShiftedFlag && drivetrain.gear != 1)
        {
            GetComponent<SoundController>().playShiftUp();
            gearShifted = false;
            gearShiftedFlag = false;
        }


        // Apply inputs
        foreach (WheelMultiplayer w in wheels)
        {
            w.brake = BreakPedal() ? brake : 0;
            w.handbrake = handbrake;

        }

        for (int i = 0; i < 2; i++)
        {
            wheels[i].steering = steering;
        }

        // Reset Car position and rotation in case it rolls over
        if (Input.GetKeyDown(KeyCode.R))
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + 2f, transform.position.z);
            transform.rotation = Quaternion.Euler(0, transform.localRotation.y, 0);
        }


        // Traction Control Toggle
        if (Input.GetKeyDown(KeyCode.T))
        {

            if (tractionControl)
            {
                tractionControl = false;
            }
            else
            {
                tractionControl = true;
            }
        }
    }
}
