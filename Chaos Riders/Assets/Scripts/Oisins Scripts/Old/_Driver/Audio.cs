using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio : MonoBehaviour
{
    //Default CarAudio Script from Unity Standard Assets
    //---------------------------------------------------------------

    // This script reads some of the car's current properties and plays sounds accordingly.
    // The engine sound can be a simple single clip which is looped and pitched, or it
    // can be a crossfaded blend of four clips which represent the timbre of the engine
    // at different RPM and Throttle state.

    // the engine clips should all be a steady pitch, not rising or falling.

    // when using four channel engine crossfading, the four clips should be:
    // lowAccelClip : The engine at low revs, with throttle open (i.e. begining acceleration at very low speed)
    // highAccelClip : Thenengine at high revs, with throttle open (i.e. accelerating, but almost at max speed)
    // lowDecelClip : The engine at low revs, with throttle at minimum (i.e. idling or engine-braking at very low speed)
    // highDecelClip : Thenengine at high revs, with throttle at minimum (i.e. engine-braking at very high speed)

    // For proper crossfading, the clips pitches should all match, with an octave offset between low and high.

    public bool onlineCar = true;

    public enum EngineAudioOptions // Options for the engine audio
    {
        Simple, // Simple style audio
    }

    public EngineAudioOptions engineSoundStyle = EngineAudioOptions.Simple;// Set the default audio options to be four channel
    public AudioClip highAccelClip;                                             // Audio clip for high acceleration
    public float pitchMultiplier = 1f;                                          // Used for altering the pitch of audio clips
    public float lowPitchMin = 1f;                                              // The lowest possible pitch for the low sounds
    public float lowPitchMax = 6f;                                              // The highest possible pitch for the low sounds
    public float highPitchMultiplier = 0.25f;                                   // Used for altering the pitch of high sounds
    public float maxRolloffDistance = 500;                                      // The maximum distance where rollof starts to take place
    public float dopplerLevel = 1;                                              // The mount of doppler effect used in the audio
    public bool useDoppler = true;                                              // Toggle for using doppler
    Rigidbody rb;

    private Controller m_CarController; // Reference to car we are controlling
    private OfflineController m_OfflineCarController; // Reference to car we are controlling

    FMOD.Studio.EventInstance brakerSound;
    FMOD.Studio.EventInstance brakerSound2;


    public Vehicle vehicalData;
    private float topSpeed, currentSpeed, currentGear;
    private float fifth, gear1Speed;

    //[FMODUnity.EventRef]
    public string revsoundLocation;
    public KeyCode driveButtonKeyboard;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        brakerSound = FMODUnity.RuntimeManager.CreateInstance("event:/CarFX/Braker/BrakerAudio");
        brakerSound2 = FMODUnity.RuntimeManager.CreateInstance("event:/CarFX/Braker/Lion Roar Loop");
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(brakerSound, transform, rb);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(brakerSound2, transform, rb);
        brakerSound.start();

        if (onlineCar)
            m_CarController = GetComponent<Controller>();
        else
            m_OfflineCarController = GetComponent<OfflineController>();

        topSpeed = vehicalData.topSpeed;
        fifth = topSpeed / 5;
        gear1Speed = fifth - (fifth / 4);
    }

    bool played;
    private void Update()
    {
        if (onlineCar)
            currentSpeed = m_CarController.currentSpeed;
        else
            currentSpeed = m_OfflineCarController.currentSpeed;

        //rev sound <-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        if ((Input.GetKeyDown(driveButtonKeyboard) || Input.GetAxis("RT") > 0) && !played)
        {
            FMODUnity.RuntimeManager.PlayOneShotAttached(revsoundLocation, gameObject);
            played = true;
        }
        if ((!Input.GetKey(driveButtonKeyboard) && Input.GetAxis("RT") == 0))
        {
            played = false;
        }

        ChangingGear();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {

        // The pitch is interpolated between the min and max values, according to the car's revs.
        float pitch;

        if(onlineCar)
            pitch = ULerp(lowPitchMin, lowPitchMax, m_CarController.Revs);
        else
            pitch = ULerp(lowPitchMin, lowPitchMax, m_OfflineCarController.Revs);

        // clamp to minimum pitch (note, not clamped to max for high revs while burning out)
        pitch = Mathf.Min(lowPitchMax, pitch);

        if (engineSoundStyle == EngineAudioOptions.Simple)
        {
            // for 1 channel engine sound, it's oh so simple:
            if (onlineCar)
            {
                if (m_CarController.boost)
                {
                    brakerSound.setPitch(pitch * pitchMultiplier * highPitchMultiplier * 1.2f);
                }
                else
                {
                    brakerSound.setPitch(pitch * pitchMultiplier * highPitchMultiplier);
                }
            }
            else
            {
                if (m_OfflineCarController.boost)
                {
                    brakerSound.setPitch(pitch * pitchMultiplier * highPitchMultiplier * 1.2f);
                }
                else
                {
                    brakerSound.setPitch(pitch * pitchMultiplier * highPitchMultiplier);
                }
            }
            
                
            //m_HighAccel.dopplerLevel = useDoppler ? dopplerLevel : 0;
            //m_HighAccel.volume = 0.45f;
        } 
    }


    private void ChangingGear()
    {
        //Debug.Log("One Fifth = " + fifth + "    gear 1 speed = " + gear1Speed);

        //gear 1
        if (currentSpeed <= gear1Speed)
        {
            currentGear = 1f;
            brakerSound.setParameterByName("Gear Change", 0.1f);
        }

        //gear 2
        if (currentSpeed > gear1Speed && currentSpeed <= gear1Speed * 2)
        {
            currentGear = 2f;
            brakerSound.setParameterByName("Gear Change", 0.2f);
        }

        //gear 3
        if (currentSpeed > gear1Speed * 2 && currentSpeed <= gear1Speed * 3)
        {
            currentGear = 3f;
            brakerSound.setParameterByName("Gear Change", 0.3f);
        }

        //gear 4
        if (currentSpeed > gear1Speed * 3 && currentSpeed <= gear1Speed * 4)
        {
            currentGear = 4f;
            brakerSound.setParameterByName("Gear Change", 0.4f);
        }

        //gear 5
        if (currentSpeed > gear1Speed * 4 && currentSpeed < topSpeed)
        {
            currentGear = 5f;
            brakerSound.setParameterByName("Gear Change", 0.5f);
        }
    }


    // unclamped versions of Lerp and Inverse Lerp, to allow value to exceed the from-to range
    private static float ULerp(float from, float to, float value)
    {
        return (1.0f - value) * from + value * to;
    }
}
