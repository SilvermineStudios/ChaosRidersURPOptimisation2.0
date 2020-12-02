using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbiliyCooldowns : MonoBehaviour
{
    [SerializeField] private bool useOverCharge = true;

    public Transform grenadeChargeBar, grenadeOverChargeBar;
    public Transform nitroGuzzlerChargeBar, nitroGuzzlerOverChargeBar;

    [SerializeField] private float standardChargeAmount, overchargeAmount;
    [SerializeField] private float speed;

    public static bool canUseSmoke = false;


    private void Start()
    {
        //set the bars = 0 at the begining
        grenadeChargeBar.GetComponent<Image>().fillAmount = 0;
        grenadeOverChargeBar.GetComponent<Image>().fillAmount = 0;
        nitroGuzzlerChargeBar.GetComponent<Image>().fillAmount = 0;
        nitroGuzzlerOverChargeBar.GetComponent<Image>().fillAmount = 0;
    }

    void Update()
    {
        ChargeBars();
        CheckIfCanUseSmoke();
        //Debug.Log(canUseSmoke);
    }

    private void UseGrenade()
    {
        canUseSmoke = false;
        grenadeChargeBar.GetComponent<Image>().fillAmount = 0;
        grenadeOverChargeBar.GetComponent<Image>().fillAmount = 0;
    }

    private void CheckIfCanUseSmoke()
    {
        if (grenadeChargeBar.GetComponent<Image>().fillAmount == 1)
        {
            canUseSmoke = true;
        }
        else
            canUseSmoke = false;
    }

    private void ChargeBars()
    {
        //if the grenade bar isnt full add to it
        if (grenadeChargeBar.GetComponent<Image>().fillAmount < 1)
        {
            standardChargeAmount += speed * Time.deltaTime;
            grenadeChargeBar.GetComponent<Image>().fillAmount = standardChargeAmount / 100;
        }

        if(useOverCharge)
        {
            //if the grenade bar is full add to the overcharge bar
            if (grenadeChargeBar.GetComponent<Image>().fillAmount == 1)
            {
                overchargeAmount += speed * Time.deltaTime;
                grenadeOverChargeBar.GetComponent<Image>().fillAmount = overchargeAmount / 100;
            }
        }
        


        //if the nitroguzzler bar isnt full add to it
        if (nitroGuzzlerChargeBar.GetComponent<Image>().fillAmount < 1)
        {
            standardChargeAmount += speed * Time.deltaTime;
            nitroGuzzlerChargeBar.GetComponent<Image>().fillAmount = standardChargeAmount / 100;
        }

        if(useOverCharge)
        {
            //if the nitroguzzler bar is full add to the overcharge bar
            if (nitroGuzzlerChargeBar.GetComponent<Image>().fillAmount == 1)
            {
                overchargeAmount += speed * Time.deltaTime;
                nitroGuzzlerOverChargeBar.GetComponent<Image>().fillAmount = overchargeAmount / 100;
            }
        }
    }
}
