using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class GameVariables : MonoBehaviour
{
    private PhotonView pv;

    [SerializeField] private GameObject hostScreen, nonHostScreen;
    public TMP_Text nonHostLapText;

    public static int Laps;
    [SerializeField] private int laps = 3;

    public static int AmountOfAICars;
    [SerializeField] private int amountOfAICars = 2;

    public static bool NitroPickup;
    [SerializeField] private bool nitroPickup = true;

    public static bool ShieldPickup;
    [SerializeField] private bool shieldPickup = true;

    public static bool RPGPickup;
    [SerializeField] private bool rpgPickup = true;

    void Awake()
    {
        pv = GetComponent<PhotonView>();
        
        Laps = laps;
        AmountOfAICars = amountOfAICars;
        NitroPickup = nitroPickup;
        ShieldPickup = shieldPickup;
        RPGPickup = rpgPickup;
    }

    
    void Update()
    {
        

        if (PhotonNetwork.IsMasterClient) 
        {
            hostScreen.SetActive(true);
            nonHostScreen.SetActive(false);
        }
        else
        {
            hostScreen.SetActive(false);
            nonHostScreen.SetActive(true);
        }


        //Laps = laps;
        AmountOfAICars = amountOfAICars;
        NitroPickup = nitroPickup;
        ShieldPickup = shieldPickup;
        RPGPickup = rpgPickup;
    }

    //runs everytime the host changes the lap count
    public void HangleLapsInputData(int val)
    {
        pv.RPC("UpdateLapText", RpcTarget.AllBuffered, val);
    }

    [PunRPC]
    void UpdateLapText(int val)
    {
        int lapVal = val + 1;
        laps = lapVal;
        nonHostLapText.text = "Laps: " + laps;
        Laps = laps;
        //Debug.Log("Laps = " + Laps + " laps = " + laps);
    }
}
