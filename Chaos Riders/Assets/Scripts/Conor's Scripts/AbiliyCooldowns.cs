using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbiliyCooldowns : MonoBehaviour
{
    public Transform grenadeChargeBar, grenadeOverChargeBar;
    public Transform nitroGuzzlerChargeBar, nitroGuzzlerOverChargeBar;

    [SerializeField] private float standardChargeAmount, overchargeAmount;
    [SerializeField] private float speed;


    private void Start()
    {
        grenadeChargeBar.GetComponent<Image>().fillAmount = 0;
        grenadeOverChargeBar.GetComponent<Image>().fillAmount = 0;
        nitroGuzzlerChargeBar.GetComponent<Image>().fillAmount = 0;
        nitroGuzzlerOverChargeBar.GetComponent<Image>().fillAmount = 0;
    }

    void Update()
    {
        //if the grenade bar isnt full add to it
        if (grenadeChargeBar.GetComponent<Image>().fillAmount < 1)
        {
            standardChargeAmount += speed * Time.deltaTime;
            grenadeChargeBar.GetComponent<Image>().fillAmount = standardChargeAmount / 100;
        }
        //if the grenade bar is full add to the overcharge bar
        if(grenadeChargeBar.GetComponent<Image>().fillAmount == 1)
        {
            overchargeAmount += speed * Time.deltaTime;
            grenadeOverChargeBar.GetComponent<Image>().fillAmount = overchargeAmount / 100;
        }


        //if the nitroguzzler bar isnt full add to it
        if (nitroGuzzlerChargeBar.GetComponent<Image>().fillAmount < 1)
        {
            standardChargeAmount += speed * Time.deltaTime;
            nitroGuzzlerChargeBar.GetComponent<Image>().fillAmount = standardChargeAmount / 100;
        }
        //if the nitroguzzler bar is full add to the overcharge bar
        if (nitroGuzzlerChargeBar.GetComponent<Image>().fillAmount == 1)
        {
            overchargeAmount += speed * Time.deltaTime;
            nitroGuzzlerOverChargeBar.GetComponent<Image>().fillAmount = overchargeAmount / 100;
        }
    }
}
