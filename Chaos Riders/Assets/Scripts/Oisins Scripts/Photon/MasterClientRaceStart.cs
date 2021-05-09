using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;
public class MasterClientRaceStart : MonoBehaviour
{
    #region SingltonStuff

    private static MasterClientRaceStart _instance;

    public static MasterClientRaceStart Instance { get { return _instance; } }

    public bool raceStart;
    public bool countdownTimerStart;
    public bool weaponsFree;
    bool n3, n2, n1, n0, nWF;
    private void Awake()
    {
        //if (!IsThisMultiplayer.Instance.multiplayer) { return; }
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
        countdownTimerStart = setCountdownTimerStart;
        weaponsFree = setWeaponsFree;
        timer = 18;
    }

    #endregion

    #region UI
    [SerializeField] Image BackgroundPanel;
    [SerializeField] TextMeshProUGUI count3;
    [SerializeField] TextMeshProUGUI count2;
    [SerializeField] TextMeshProUGUI count1;
    [SerializeField] TextMeshProUGUI countStart;
    [SerializeField] TextMeshProUGUI countWeaponsFree;


    private TextMeshProUGUI currentText;

    #endregion

    #region Bools
    public bool setCountdownTimer3;
    public bool setCountdownTimer2;
    public bool setCountdownTimer1;
    public bool setCountdownTimerStart;
    public bool setWeaponsFree;
    [SerializeField] bool SkipAllThis;

    bool fadePanelOut;

#endregion

    #region Floats
    public float timer;

    public int countdownTimer;


    #endregion

    PhotonView pv;
    Color panelTemp;

    private void Start()
    {
        if (!IsThisMultiplayer.Instance.multiplayer)
        {
            MasterClientRaceStart.Instance.countdownTimerStart = true;
            MasterClientRaceStart.Instance.weaponsFree = true;
            return;
        }
        pv = GetComponent<PhotonView>();
        panelTemp = BackgroundPanel.color;

        if (SkipAllThis)
        {
            MasterClientRaceStart.Instance.countdownTimerStart = true;
            MasterClientRaceStart.Instance.raceStart = true;
            MasterClientRaceStart.Instance.weaponsFree = true;
            return;
        }

        if (PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(InitWait());
        }
    }

    IEnumerator InitWait()
    {
        yield return new WaitForSeconds(2);
        pv.RPC("StartMusic", RpcTarget.All);
    }

    [PunRPC]
    public void StartMusic()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Music/Opening");
        StartCoroutine(StartTime());
    }

    IEnumerator StartTime()
    {
        yield return new WaitForSeconds(2.1f);
        pv.RPC("CountDown3", RpcTarget.All);
        yield return new WaitForSeconds(2.05f);
        pv.RPC("CountDown2", RpcTarget.All);
        yield return new WaitForSeconds(2.05f);
        pv.RPC("CountDown1", RpcTarget.All);
        yield return new WaitForSeconds(2.05f);
        pv.RPC("CountDownStart", RpcTarget.All);
        yield return new WaitForSeconds(5);
        pv.RPC("WeaponsFree", RpcTarget.All);
    }

    [PunRPC]
    public void StartCountdown()
    {
        StartCoroutine(StartTime());
    }

    [PunRPC]
    public void CountDown3()
    {
        CountDown(3);
    }

    [PunRPC]
    public void CountDown2()
    {
        CountDown(2);
    }

    [PunRPC]
    public void CountDown1()
    {
        CountDown(1);
    }

    [PunRPC]
    public void CountDownStart()
    {
        CountDown(0);
    }

    [PunRPC]
    public void WeaponsFree()
    {
        CountDown(-1);
    }


    void FixedUpdate()
    {
        if (!IsThisMultiplayer.Instance.multiplayer) { return; }



        if(count3.alpha > 0 && n3)
        {
            count3.SubtractAlpha(0.004f);
        }
        if (count2.alpha > 0 && n2)
        {
            count2.SubtractAlpha(0.004f);
        }
        if (count1.alpha > 0 && n1)
        {
            count1.SubtractAlpha(0.004f);
        }
        if (countStart.alpha > 0 && n0)
        {
            countStart.SubtractAlpha(0.004f);
            BackgroundPanel.SubtractAlpha(0.004f);
        }
        if (countWeaponsFree.alpha > 0 && nWF)
        {
            countWeaponsFree.SubtractAlpha(0.004f);
            BackgroundPanel.SubtractAlpha(0.004f);
        }
    }


    
    public void CountDown(int time)
    {

        switch (time)
        {
            case 3:
                n3 = true;
                BackgroundPanel.ChangeAlpha(0.5f);
                count3.ChangeAlpha(1);
                FMODUnity.RuntimeManager.PlayOneShot("event:/RaceStart/CountDown");
                break;

            case 2:
                n2 = true;
                count2.ChangeAlpha(1);
                count3.ChangeAlpha(0);
                FMODUnity.RuntimeManager.PlayOneShot("event:/RaceStart/CountDown");
                break;

            case 1:
                n1 = true;
                count1.ChangeAlpha(1);
                count2.ChangeAlpha(0);
                FMODUnity.RuntimeManager.PlayOneShot("event:/RaceStart/CountDown");
                break;

            case 0:
                n0 = true;
                countStart.ChangeAlpha(1);
                count1.ChangeAlpha(0);
                FMODUnity.RuntimeManager.PlayOneShot("event:/RaceStart/Start");
                MasterClientRaceStart.Instance.countdownTimerStart = true;
                MasterClientRaceStart.Instance.raceStart = true;
                break;

            case -1:
                nWF = true;
                countWeaponsFree.ChangeAlpha(1);
                BackgroundPanel.ChangeAlpha(0.5f);
                FMODUnity.RuntimeManager.PlayOneShot("event:/RaceStart/WeaponsFree");
                MasterClientRaceStart.Instance.weaponsFree = true;
                break;
        }

        
    }






}
