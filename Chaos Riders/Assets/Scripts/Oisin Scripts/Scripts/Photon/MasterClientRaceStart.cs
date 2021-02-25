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

    public bool countdownTimerStart;
    public bool weaponsFree;

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

    [SerializeField] GameObject GBackgroundPanel;
    [SerializeField] GameObject Gcount3;
    [SerializeField] GameObject Gcount2;
    [SerializeField] GameObject Gcount1;
    [SerializeField] GameObject GcountStart;
    [SerializeField] GameObject GcountWeaponsFree;

    private TextMeshProUGUI currentText;

    #endregion

    #region Bools
    public bool setCountdownTimer3;
    public bool setCountdownTimer2;
    public bool setCountdownTimer1;
    public bool setCountdownTimerStart;
    public bool setWeaponsFree;


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
        if (!IsThisMultiplayer.Instance.multiplayer) { return; }
        pv = GetComponent<PhotonView>();
        panelTemp = BackgroundPanel.color;
        if(PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(StartTime());
        }
    }

    IEnumerator StartTime()
    {
        yield return new WaitForSeconds(4);
        pv.RPC("CountDown", RpcTarget.All, 16);
        yield return new WaitForSeconds(2);
        pv.RPC("CountDown", RpcTarget.All, 14);
        yield return new WaitForSeconds(2);
        pv.RPC("CountDown", RpcTarget.All, 12);
        yield return new WaitForSeconds(2);
        pv.RPC("CountDown", RpcTarget.All, 10);
        yield return new WaitForSeconds(5);
        pv.RPC("CountDown", RpcTarget.All, 5);
    }

    void FixedUpdate()
    {
        if (!IsThisMultiplayer.Instance.multiplayer) { return; }

        /*
        if (PhotonNetwork.IsMasterClient)
        {

            


            if (timer <= 10 && !countdownTimerStart)
            {
                pv.RPC("CountDownStart", RpcTarget.All);
            }

            else if (timer <= 5 && !weaponsFree)
            {
                pv.RPC("WeaponsFree", RpcTarget.All);
            }

            timer -= Time.deltaTime;
        }

        */
        if (currentText != null && currentText.alpha > 0 && !weaponsFree)
        {
            currentText.SubtractAlpha(0.008f);
            if (fadePanelOut)
            {
                BackgroundPanel.SubtractAlpha(0.008f);
            }
        }

        else if (currentText != null && currentText.alpha > 0 && weaponsFree)
        {
            currentText.SubtractAlpha(0.004f);
            BackgroundPanel.SubtractAlpha(0.004f);
        }
    }


    [PunRPC]
    public void CountDown(int time)
    {
        MasterClientRaceStart.Instance.countdownTimer = time;

        switch (time)
        {
            case 16:
                BackgroundPanel.ChangeAlpha(0.5f);
                currentText = count3;
                currentText.ChangeAlpha(1);
                FMODUnity.RuntimeManager.PlayOneShot("event:/RaceStart/CountDown");
                break;

            case 14:
                currentText = count2;
                currentText.ChangeAlpha(1);
                FMODUnity.RuntimeManager.PlayOneShot("event:/RaceStart/CountDown");
                break;

            case 12:
                currentText = count1;
                currentText.ChangeAlpha(1);
                FMODUnity.RuntimeManager.PlayOneShot("event:/RaceStart/CountDown");
                break;

            case 10:
                currentText = countStart;
                currentText.ChangeAlpha(1);
                FMODUnity.RuntimeManager.PlayOneShot("event:/RaceStart/Start");
                fadePanelOut = true;
                MasterClientRaceStart.Instance.countdownTimerStart = true;
                break;

            case 5:
                currentText = countWeaponsFree;
                currentText.ChangeAlpha(1);
                BackgroundPanel.ChangeAlpha(0.5f);
                FMODUnity.RuntimeManager.PlayOneShot("event:/RaceStart/Start");
                MasterClientRaceStart.Instance.weaponsFree = true;
                fadePanelOut = true;
                break;
        }

        
    }


    [PunRPC]
    public void CountDownStart()
    {
        MasterClientRaceStart.Instance.countdownTimerStart = true;
        FMODUnity.RuntimeManager.PlayOneShot("event:/RaceStart/Start");
    }

    [PunRPC]
    public void WeaponsFree()
    {
        MasterClientRaceStart.Instance.weaponsFree = true;
        FMODUnity.RuntimeManager.PlayOneShot("event:/RaceStart/Start");
    }

}
