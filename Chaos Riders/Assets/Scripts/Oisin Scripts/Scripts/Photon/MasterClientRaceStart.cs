using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MasterClientRaceStart : MonoBehaviour
{
    #region SingltonStuff

    private static MasterClientRaceStart _instance;

    public static MasterClientRaceStart Instance { get { return _instance; } }


    public bool countdownTimer3 { get { return setCountdownTimer3; } private set { } }
    public bool countdownTimer2 { get { return setCountdownTimer2; } private set { } }
    public bool countdownTimer1 { get { return setCountdownTimer1; } private set { } }
    public bool countdownTimerStart { get { return setCountdownTimerStart; } private set { } }
    public bool weaponsFree { get { return setWeaponsFree; } private set { } }


    private void Awake()
    {
        if (!IsThisMultiplayer.Instance.multiplayer) { return; }
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        ResetAll();
    }

    private void ResetAll()
    {
        countdownTimer3 = setCountdownTimer3;
        countdownTimer2 = setCountdownTimer2;
        countdownTimer1 = setCountdownTimer1;
        countdownTimerStart = setCountdownTimerStart;
        weaponsFree = setWeaponsFree;
        timer = 18;
    }



    #endregion

    #region Bools
    public bool setCountdownTimer3;
    public bool setCountdownTimer2;
    public bool setCountdownTimer1;
    public bool setCountdownTimerStart;
    public bool setWeaponsFree;



#endregion

#region Floats
    public float timer;




#endregion

    private void Start()
    {
        if (!IsThisMultiplayer.Instance.multiplayer) { return; }
    }

    void FixedUpdate()
    {
        if (!IsThisMultiplayer.Instance.multiplayer) { return; }
        if (PhotonNetwork.IsMasterClient)
        {
            if(timer <= 16 && !setCountdownTimer3)
            {
                setCountdownTimer3 = true;
                FMODUnity.RuntimeManager.PlayOneShot("event:/RaceStart/CountDown");
            }
            else if (timer <= 14 && !setCountdownTimer2)
            {
                setCountdownTimer2 = true;
                FMODUnity.RuntimeManager.PlayOneShot("event:/RaceStart/CountDown");
            }
            else if (timer <= 12 && !setCountdownTimer1)
            {
                setCountdownTimer1 = true;
                FMODUnity.RuntimeManager.PlayOneShot("event:/RaceStart/CountDown");
            }
            else if (timer <= 10 && !setCountdownTimerStart)
            {
                setCountdownTimerStart = true;
                FMODUnity.RuntimeManager.PlayOneShot("event:/RaceStart/Start");
            }

            else if (timer <= 5 && !setWeaponsFree)
            {
                setWeaponsFree = true;
                FMODUnity.RuntimeManager.PlayOneShot("event:/RaceStart/Start");
            }

            timer -= Time.deltaTime;

        }

        //Debug.Log(timer);
        Debug.Log(PhotonNetwork.IsMasterClient);
    }

}
