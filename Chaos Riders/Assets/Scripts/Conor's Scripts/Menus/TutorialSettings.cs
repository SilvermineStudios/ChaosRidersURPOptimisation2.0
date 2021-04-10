using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    public Color standardColour;
    public GameObject brakerIcon;
    public GameObject shredderIcon;
    public GameObject standarGunIcon;
    public GameObject goldenGunIcon;
    public GameObject brakerButton;
    public GameObject shredderButton;
    public GameObject standarGunButton;
    public GameObject goldenGunButton;
    public GameObject backButton;

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
        backButton.SetActive(false);
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
        backButton.SetActive(true);
    }

    public void Button_Shooter()
    {
        playerTypeButtons.SetActive(false);
        driverButtons.SetActive(false);

        shooterButtons.SetActive(true);
        backButton.SetActive(true);
    }

    public void Button_CarPicker(int val)
    {
        //braker
        if(val == 0)
        {
            brakerIcon.SetActive(true);
            shredderIcon.SetActive(false);
            standarGunIcon.SetActive(false);
            goldenGunIcon.SetActive(false);

            brakerButton.GetComponent<Image>().color = selectedColour;
            shredderButton.GetComponent<Image>().color = standardColour;
            standarGunButton.GetComponent<Image>().color = standardColour;
            goldenGunButton.GetComponent<Image>().color = standardColour;
        }

        //shredder
        if(val == 1)
        {
            brakerIcon.SetActive(false);
            shredderIcon.SetActive(true);
            standarGunIcon.SetActive(false);
            goldenGunIcon.SetActive(false);

            shredderButton.GetComponent<Image>().color = selectedColour;
            brakerButton.GetComponent<Image>().color = standardColour;
            standarGunButton.GetComponent<Image>().color = standardColour;
            goldenGunButton.GetComponent<Image>().color = standardColour;
        }
    }

    public void Button_ShooterPicker(int val)
    {
        //standard gun
        if(val == 0)
        {
            brakerIcon.SetActive(false);
            shredderIcon.SetActive(false);
            standarGunIcon.SetActive(true);
            goldenGunIcon.SetActive(false);

            standarGunButton.GetComponent<Image>().color = selectedColour;
            goldenGunButton.GetComponent<Image>().color = standardColour;
            brakerButton.GetComponent<Image>().color = standardColour;
            shredderButton.GetComponent<Image>().color = standardColour;
        }

        //golden gun
        if (val == 1)
        {
            brakerIcon.SetActive(false);
            shredderIcon.SetActive(false);
            standarGunIcon.SetActive(false);
            goldenGunIcon.SetActive(true);

            goldenGunButton.GetComponent<Image>().color = selectedColour;
            standarGunButton.GetComponent<Image>().color = standardColour;
            brakerButton.GetComponent<Image>().color = standardColour;
            shredderButton.GetComponent<Image>().color = standardColour;
        }
    }

    public void Button_Back()
    {
        driverButtons.SetActive(false);
        shooterButtons.SetActive(false);
        backButton.SetActive(false);

        playerTypeButtons.SetActive(true);
    }
    #endregion
}
