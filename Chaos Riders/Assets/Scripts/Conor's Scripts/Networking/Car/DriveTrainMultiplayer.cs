using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

// This class simulates a car's engine and drivetrain, generating
// torque, and applying the torque to the wheels.
public class DriveTrainMultiplayer : MonoBehaviour
{
    #region Variables

    public bool reverse = false;

    private PhotonView pv;

    // All the wheels the drivetrain should power
    public WheelMultiplayer[] poweredWheels;

    // The gear ratios, including neutral (0) and reverse (negative) gears
    public float[] gearRatios;

    // The final drive ratio, which is multiplied to each gear ratio
    public float finalDriveRatio = 3.23f;

    // The engine's torque curve characteristics. Since actual curves are often hard to come by,
    // we approximate the torque curve from these values instead.

    // powerband RPM range
    public float minRPM = 800;
    public float maxRPM = 6400;

    // engine's maximal torque (in Nm) and RPM.
    public float maxTorque = 664;
    public float torqueRPM = 4000;

    // engine's maximal power (in Watts) and RPM.
    public float maxPower = 317000;
    public float powerRPM = 5000;

    // engine inertia (how fast the engine spins up), in kg * m^2
    public float engineInertia = 0.3f;

    // engine's friction coefficients - these cause the engine to slow down, and cause engine braking.

    // constant friction coefficient
    public float engineBaseFriction = 25f;
    // linear friction coefficient (higher friction when engine spins higher)
    public float engineRPMFriction = 0.02f;

    // Engine orientation (typically either Vector3.forward or Vector3.right). 
    // This determines how the car body moves as the engine revs up.	
    public Vector3 engineOrientation = Vector3.right;

    // Coefficient determining how muchg torque is transfered between the wheels when they move at 
    // different speeds, to simulate differential locking.
    public float differentialLockCoefficient = 0;

    // inputs
    public bool nitro;
    [SerializeField] private float nitroBoost = 2.5f;


    // engine throttle
    public float throttle = 0;
    // engine throttle without traction control (used for automatic gear shifting)
    public float throttleInput = 0;

    //clutch
    public float clutch;
    private float clutchTorque;

    // shift gears automatically?
    public bool automatic = true;

    // state
    public int gear = 2;
    public float rpm;
    public float slipRatio = 0.0f;
    float engineAngularVelo;


    float Sqr(float x) { return x * x; }


    // Calculate engine torque for current rpm and throttle values.
    float CalcEngineTorque()
    {
        float result;
        if (rpm < torqueRPM)
        {
            result = maxTorque * (-Sqr(rpm / torqueRPM - 1) + 1);
        }

        else
        {
            float maxPowerTorque = maxPower / (powerRPM * 2 * Mathf.PI / 60);
            float aproxFactor = (maxTorque - maxPowerTorque) / (2 * torqueRPM * powerRPM - Sqr(powerRPM) - Sqr(torqueRPM));
            float torque = aproxFactor * Sqr(rpm - torqueRPM) + maxTorque;
            result = torque > 0 ? torque : 0;
        }

        if (rpm > maxRPM)
        {
            result *= 1 - ((rpm - maxRPM) * 0.006f);
            if (result < 0)
            {
                result = 0;
            }
        }
        if (rpm < 0)
        {
            result = 0;
        }

        if (nitro)
        {
            result *= nitroBoost;
        }


        return result;
    }


    private float counter, offset = 1f;
    #endregion

    private void Awake()
    {
        pv = GetComponent<PhotonView>();
    }

    private void Update()
    {
        if(pv.IsMine && IsThisMultiplayer.Instance.multiplayer)
        {
            if (shiftedRecently)
            {
                counter -= Time.deltaTime;
                if (counter <= 0)
                {
                    shiftedRecently = false;
                }
            }
        }
        else if(!IsThisMultiplayer.Instance.multiplayer)
        {
            if (shiftedRecently)
            {
                counter -= Time.deltaTime;
                if (counter <= 0)
                {
                    shiftedRecently = false;
                }
            }
        }
    }

    
    void FixedUpdate()
    {
        if(pv.IsMine && IsThisMultiplayer.Instance.multiplayer)
        {
            float ratio = gearRatios[gear] * finalDriveRatio;
            float inertia = engineInertia * Sqr(ratio);
            float engineFrictionTorque = engineBaseFriction + rpm * engineRPMFriction;
            float engineTorque = (CalcEngineTorque() + Mathf.Abs(engineFrictionTorque)) * throttle;

            slipRatio = 0.0f;


            if (ratio == 0 || (clutch == 1 && (int)(GetComponent<Rigidbody>().velocity.magnitude * 3.6f) > 5))
            {

                // Neutral gear - just rev up engine
                float engineAngularAcceleration = (engineTorque - engineFrictionTorque) / engineInertia;
                engineAngularVelo += engineAngularAcceleration * Time.deltaTime;

                //if ((int)GetComponent<Rigidbody>().velocity.magnitude * 3.6f == 0 && engineAngularVelo < 0f)
                    //engineAngularVelo = 0f;

                // Apply torque to car body
                //GetComponent<Rigidbody>().AddTorque(-engineOrientation * engineTorque * 2.5f);

            }

            else
            {
                float drivetrainFraction = 1.0f / poweredWheels.Length;
                float averageAngularVelo = 0;
                foreach (WheelMultiplayer w in poweredWheels)
                    averageAngularVelo += w.angularVelocity * drivetrainFraction;

                float engineAngularAcceleration = (engineTorque - engineFrictionTorque) / engineInertia;


                // Apply torque to wheels
                foreach (WheelMultiplayer w in poweredWheels)
                {
                    float lockingTorque = (averageAngularVelo - w.angularVelocity) * differentialLockCoefficient;
                    w.drivetrainInertia = inertia * drivetrainFraction;
                    w.driveFrictionTorque = engineFrictionTorque * Mathf.Abs(ratio) * drivetrainFraction;
                    w.driveTorque = engineTorque * ratio * drivetrainFraction + lockingTorque;
                    slipRatio += w.slipRatio * drivetrainFraction;
                }

                engineAngularVelo = averageAngularVelo * ratio;

            }

            // update state
            slipRatio *= Mathf.Sign(ratio);
            rpm = engineAngularVelo * (60.0f / (2 * Mathf.PI));
            rpm = Mathf.Clamp(rpm, 0f, maxRPM + minRPM); //limit excess rpm

            // very simple simulation of clutch - just pretend we are at a higher rpm.
            float minClutchRPM = minRPM;
            if (gear != 1)
            {
                //steering wheel with pedals
                minClutchRPM += throttle * (maxRPM - minRPM) * clutch;
            }
            else if (gear == 2)
            {
                //keyboard
                minClutchRPM += throttle * 4200f;
            }

            if (rpm < minClutchRPM)
            {
                rpm = minClutchRPM;
            }

            /*
            // shake car on low speeds
            if (gear > 1 && (int)(GetComponent<Rigidbody>().velocity.magnitude * 3.6f) <= 20)
            {
                GetComponent<Rigidbody>().AddTorque(-engineOrientation * engineTorque * 2.5f);
            }
            */

            if (rpm >= powerRPM * (0.5f + 0.5f * throttleInput) && !shiftedRecently)
            {
                ShiftUp();
            }
            else if (rpm <= maxRPM * (0.25f + 0.4f * throttleInput) && gear > 2 && !shiftedRecently)
            {
                ShiftDown();
            }
            if (throttleInput < 0 && rpm <= minRPM)
            {
                gear = (gear == 0 ? 2 : 0);
            }
        }
        else if(!IsThisMultiplayer.Instance.multiplayer)
        {
            float ratio = gearRatios[gear] * finalDriveRatio;
            float inertia = engineInertia * Sqr(ratio);
            float engineFrictionTorque = engineBaseFriction + rpm * engineRPMFriction;
            float engineTorque = (CalcEngineTorque() + Mathf.Abs(engineFrictionTorque)) * throttle;

            slipRatio = 0.0f;


            if (ratio == 0 || (clutch == 1 && (int)(GetComponent<Rigidbody>().velocity.magnitude * 3.6f) > 5))
            {

                // Neutral gear - just rev up engine
                float engineAngularAcceleration = (engineTorque - engineFrictionTorque) / engineInertia;
                engineAngularVelo += engineAngularAcceleration * Time.deltaTime;

                //if ((int)GetComponent<Rigidbody>().velocity.magnitude * 3.6f == 0 && engineAngularVelo < 0f)
                    //engineAngularVelo = 0f;

                // Apply torque to car body
                //GetComponent<Rigidbody>().AddTorque(-engineOrientation * engineTorque * 2.5f);

            }

            else
            {
                float drivetrainFraction = 1.0f / poweredWheels.Length;
                float averageAngularVelo = 0;
                foreach (WheelMultiplayer w in poweredWheels)
                    averageAngularVelo += w.angularVelocity * drivetrainFraction;

                float engineAngularAcceleration = (engineTorque - engineFrictionTorque) / engineInertia;


                // Apply torque to wheels
                foreach (WheelMultiplayer w in poweredWheels) //<--------------------------------------------------------------------------------------
                {
                    float lockingTorque = (averageAngularVelo - w.angularVelocity) * differentialLockCoefficient;
                    w.drivetrainInertia = inertia * drivetrainFraction;
                    w.driveFrictionTorque = engineFrictionTorque * Mathf.Abs(ratio) * drivetrainFraction;
                    w.driveTorque = engineTorque * ratio * drivetrainFraction + lockingTorque; 
                    slipRatio += w.slipRatio * drivetrainFraction;
                }

                engineAngularVelo = averageAngularVelo * ratio;

            }

            // update state
            slipRatio *= Mathf.Sign(ratio);
            rpm = engineAngularVelo * (60.0f / (2 * Mathf.PI));
            rpm = Mathf.Clamp(rpm, 0f, maxRPM + minRPM); //limit excess rpm

            // very simple simulation of clutch - just pretend we are at a higher rpm.
            float minClutchRPM = minRPM;
            if (gear != 1)
            {
                //steering wheel with pedals
                minClutchRPM += throttle * (maxRPM - minRPM) * clutch;
            }
            else if (gear == 2)
            {
                //keyboard
                minClutchRPM += throttle * 4200f;
            }

            if (rpm < minClutchRPM)
            {
                rpm = minClutchRPM;
            }

            /*
            // shake car on low speeds
            if (gear > 1 && (int)(GetComponent<Rigidbody>().velocity.magnitude * 3.6f) <= 20)
            {
                GetComponent<Rigidbody>().AddTorque(-engineOrientation * engineTorque * 2.5f);
            }
            */

            if (rpm >= powerRPM * (0.5f + 0.5f * throttleInput) && !shiftedRecently)
            {
                ShiftUp();
            }
            else if (rpm <= maxRPM * (0.25f + 0.4f * throttleInput) && gear > 2 && !shiftedRecently)
            {
                ShiftDown();
            }
            if (throttleInput < 0 && rpm <= minRPM)
            {
                gear = (gear == 0 ? 2 : 0);
            }
        }
    }

    public bool shiftedRecently;

    public void ShiftUp()
    {

        shiftedRecently = true;
        counter = offset;
        if (gear < gearRatios.Length - 1)
        {
            gear++;
            GetComponent<SoundControllerMultiplayer>().playBOV();
            GetComponent<SoundControllerMultiplayer>().playShiftUp();
        }
    }

    public void ShiftDown()
    {
        shiftedRecently = true;
        counter = offset;
        if (gear > 0)
        {
            gear--;
            GetComponent<SoundControllerMultiplayer>().playShiftDown();
        }
    }



    // Debug GUI.
    void OnGUI()
    {
        //GUILayout.Label("RPM: " + rpm);
        GUILayout.Label("Gear: " + (gear - 1));
        //automatic = GUILayout.Toggle(automatic, "Automatic Transmission");
    }


}
