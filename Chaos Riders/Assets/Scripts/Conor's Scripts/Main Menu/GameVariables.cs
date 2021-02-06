using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameVariables : MonoBehaviour
{
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
        
    }

    
    void Update()
    {
        Laps = laps;
        AmountOfAICars = amountOfAICars;
        NitroPickup = nitroPickup;
        ShieldPickup = shieldPickup;
        RPGPickup = rpgPickup;
    }

    public void HangleLapsInputData(int val)
    {
        int lapVal = val + 1;

        laps = lapVal;
        nonHostLapText.text = "Laps: " + lapVal.ToString();
    }
}
