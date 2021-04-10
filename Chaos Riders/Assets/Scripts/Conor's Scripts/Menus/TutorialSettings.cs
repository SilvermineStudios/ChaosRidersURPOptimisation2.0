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

    [Header("Character Selection")]
    public GameObject playerTypeButtons;
    public GameObject driverButtons;
    public GameObject shooterButtons;
    public Color selectedColour;

    void Start()
    {
        //Values / Dropdowns
        lapsDropDown.value = amountOfLaps - 1;
        amountOfAiDropDown.value = amountOfAI;

        if (aiOn)
            aiOnDropDown.value = 0;
        else
            aiOnDropDown.value = 1;

        //character Selection
        playerTypeButtons.SetActive(true);
        driverButtons.SetActive(false);
        shooterButtons.SetActive(false);
    }

    #region Settings
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
    #endregion

    #region Buttons
    public void Button_Start()
    {

    }

    public void Button_Driver()
    {
        playerTypeButtons.SetActive(false);
        shooterButtons.SetActive(false);

        driverButtons.SetActive(true);
    }

    public void Button_Shooter()
    {
        playerTypeButtons.SetActive(false);
        driverButtons.SetActive(false);

        shooterButtons.SetActive(true);
    }

    public void Button_Back()
    {
        driverButtons.SetActive(false);
        shooterButtons.SetActive(false);

        playerTypeButtons.SetActive(true);
    }

    public void Button_Braker()
    {

    }

    public void Button_Shredder()
    {

    }

    public void Button_StandardGun()
    {

    }

    public void Button_GoldenGun()
    {

    }
    #endregion
}
