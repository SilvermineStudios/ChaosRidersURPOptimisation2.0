using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialSettings : MonoBehaviour
{
    [Header("Values")]
    public int amountOfLaps = 3;
    public bool aiOn = true;
    public int amountOfAI = 1;

    [Header("Setings Objects")]
    public TMP_Dropdown lapsDropDown;
    public TMP_Dropdown aiOnDropDown;
    public TMP_Dropdown amountOfAiDropDown;

    [Header("Buttons")]
    public GameObject shooterButton;
    public GameObject driverButton;
    public GameObject brakerButton;
    public GameObject shredderButton;
    public GameObject standardGunButton;
    public GameObject goldenGunButton;

    void Start()
    {
        lapsDropDown.value = amountOfLaps - 1;
        amountOfAiDropDown.value = amountOfAI;

        if (aiOn)
            aiOnDropDown.value = 0;
        else
            aiOnDropDown.value = 1;
    }

    public void HangleLapsInputData(int val)
    {
        amountOfLaps = val + 1;
    }

    public void HangleAiOnInputData(int val)
    {
        if (val == 0)
        {
            aiOn = true;

            if(amountOfAI == 0)
            {
                amountOfAI = 1;
                amountOfAiDropDown.value = 1;
            }
        }
        else
        {
            aiOn = false;
            amountOfAI = 0;
            amountOfAiDropDown.value = 0;
        }
    }

    public void HandleAmountOfAiInputData(int val)
    {
        amountOfAI = val;

        if(val == 0)
        {
            aiOn = false;
            aiOnDropDown.value = 1;
        }
        else
        {
            aiOn = true;
            aiOnDropDown.value = 0;
        }
    }
}
