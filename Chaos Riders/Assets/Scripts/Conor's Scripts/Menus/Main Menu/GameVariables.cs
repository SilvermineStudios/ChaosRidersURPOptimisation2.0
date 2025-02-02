﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class GameVariables : MonoBehaviour
{
    private PhotonView pv;

    [SerializeField] private GameObject hostScreen, nonHostScreen;
    public TMP_Text nonHostLapText, nonHostToggleAIText, nonHostAmountOfAIText, nonHostPickupsText, nonHostTimeOfDayText;

    public static int Laps;
    [SerializeField] private int laps = 3;

    public static bool ToggleAI;
    [SerializeField] private bool toggleAI = true;
    [SerializeField] private TMP_Dropdown toggleAIDropdown;

    public static int AmountOfAICars;
    [SerializeField] private int amountOfAICars = 2;

    public static bool Daytime;
    [SerializeField] private bool daytime = true;

    public static bool Pickups;
    [SerializeField] private bool pickups = true;

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
        Daytime = daytime;
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
        Daytime = daytime;
    }



    //runs everytime the host changes the lap count
    public void HangleLapsInputData(int val)
    {
        pv.RPC("UpdateLap", RpcTarget.AllBuffered, val); //val = the array index of the chose box on the dropdown menu
    }

    //runs everytime the host changes the time of day
    public void HangleUpdateTimeOfDay(int val)
    {
        pv.RPC("UpdateTimeOfDay", RpcTarget.AllBuffered, val); //val = the array index of the chose box on the dropdown menu
    }

    //runs everytime the host toggles the Ai on and off
    public void HangleToggleAIInputData(int val)
    {
        pv.RPC("UpdateToggleAI", RpcTarget.AllBuffered, val); //val = the array index of the chose box on the dropdown menu
    }

    //runs everytime the host changes the ai count
    public void HangleAmountOfAIInputData(int val)
    {
        pv.RPC("UpdateAmountOfAI", RpcTarget.AllBuffered, val); //val = the array index of the chose box on the dropdown menu
    }

    public void HangleTogglePickupsInputData(int val)
    {
        pv.RPC("UpdateTogglePickups", RpcTarget.AllBuffered, val); //val = the array index of the chose box on the dropdown menu
    }



    [PunRPC]
    void UpdateLap(int val)
    {
        int lapVal = val + 1; //adding 1 to val, val is the array index value from the dropdown which starts at 0, the index 0 on the drop down is 1 lap
        laps = lapVal;
        //nonHostLapText.text = "Laps: " + laps; //display what the host chooses for the other players
        nonHostLapText.text = laps.ToString(); //display what the host chooses for the other players
        Laps = laps;
    }

    [PunRPC]
    void UpdateTimeOfDay(int val)
    {
        if(val == 0)
        {
            nonHostTimeOfDayText.text = "Day";
            daytime = true;
        }
        if(val == 1)
        {
            nonHostTimeOfDayText.text = "Night";
            daytime = false;
        }
    }

    [PunRPC]
    void UpdateToggleAI(int val)
    {
        if (val == 0)
        {
            toggleAI = true;
            //nonHostToggleAIText.text = "AI Drivers: on"; //display what the host chooses for the other players
            nonHostToggleAIText.text = "on"; //display what the host chooses for the other players
            ToggleAI = toggleAI;

            //amountOfAIDropdown.value = 2;
            //pv.RPC("UpdateAmountOfAI", RpcTarget.AllBuffered, 2);
        } 
        else
        {
            toggleAI = false;
            //nonHostToggleAIText.text = "AI Drivers: off"; //display what the host chooses for the other players
            nonHostToggleAIText.text = "off"; //display what the host chooses for the other players
            ToggleAI = toggleAI;

            //amountOfAIDropdown.value = 0;
            //pv.RPC("UpdateAmountOfAI", RpcTarget.AllBuffered, 0);
        }  
    }

    [PunRPC]
    void UpdateAmountOfAI(int val)
    {
        int amountAI = val;
        amountOfAICars = amountAI;

        //host has chosen 0 ai cars
        if (val == 0)
        {
            //turn off the toggleAI if there is 0 ai drivers selected
            toggleAIDropdown.value = 1;
            pv.RPC("UpdateToggleAI", RpcTarget.AllBuffered, 1);
        }
        else
        {
            //turn on the toggleAI if there are more than one ai driver selected
            toggleAIDropdown.value = 0;
            pv.RPC("UpdateToggleAI", RpcTarget.AllBuffered, 0);
        }

        //nonHostAmountOfAIText.text = "Amount of AI: " + amountAI; //display what the host chooses for the other players
        nonHostAmountOfAIText.text = amountAI.ToString(); //display what the host chooses for the other players
        AmountOfAICars = amountOfAICars;
    }

    [PunRPC]
    void UpdateTogglePickups(int val)
    {
        if (val == 0)
        {
            pickups = true;
            //nonHostPickupsText.text = "Pickups: on"; //display what the host chooses for the other players
            nonHostPickupsText.text = "on"; //display what the host chooses for the other players
            Pickups = pickups;
        }
        else
        {
            pickups = false;
            //nonHostPickupsText.text = "Pickups: off"; //display what the host chooses for the other players
            nonHostPickupsText.text = "off"; //display what the host chooses for the other players
            Pickups = pickups;
        }
    }
}
