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
    public TMP_Text nonHostLapText, nonHostToggleAIText, nonHostAmountOfAIText, nonHostPickupsText;

    public static int Laps;
    [SerializeField] private int laps = 3;

    public static bool ToggleAI;
    [SerializeField] private bool toggleAI = true;

    public static int AmountOfAICars;
    [SerializeField] private int amountOfAICars = 2;

    public static bool Pickups;
    [SerializeField] private bool pickup = true;

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
        ToggleAI = toggleAI;
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

    //runs everytime the host toggles the Ai on and off
    public void HangleToggleAIInputData(int val)
    {
        pv.RPC("UpdateToggleAIText", RpcTarget.AllBuffered, val);
    }

    //runs everytime the host changes the ai count
    public void HangleAmountOfAIInputData(int val)
    {
        pv.RPC("UpdateAmountOfAIText", RpcTarget.AllBuffered, val);
    }

    public void HangleTogglePickupsInputData(int val)
    {
        pv.RPC("UpdateTogglePickupsText", RpcTarget.AllBuffered, val);
    }



    [PunRPC]
    void UpdateLapText(int val)
    {
        int lapVal = val + 1;
        laps = lapVal;
        nonHostLapText.text = "Laps: " + laps;
        Laps = laps;
    }

    [PunRPC]
    void UpdateToggleAIText(int val)
    {
        if (val == 0)
        {
            ToggleAI = true;
            nonHostToggleAIText.text = "AI Drivers: on";
        } 
        else
        {
            ToggleAI = false;
            nonHostToggleAIText.text = "AI Drivers: off";
        }  
    }

    [PunRPC]
    void UpdateAmountOfAIText(int val)
    {
        int amountAI = val + 1;
        AmountOfAICars = amountAI;
        nonHostAmountOfAIText.text = "Amount of AI: " + amountAI;
    }

    [PunRPC]
    void UpdateTogglePickupsText(int val)
    {
        if (val == 0)
        {
            Pickups = true;
            nonHostPickupsText.text = "Pickups: on";
        }
        else
        {
            Pickups = false;
            nonHostPickupsText.text = "Pickups: off";
        }
    }
}
