using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbiliyCooldowns : MonoBehaviour
{
    [SerializeField] private bool useOverCharge = true;

    [SerializeField] private Transform equipmentChargeBar, equipmentOverChargeBar;
    public static Image equipmentCharge;
    [SerializeField] private Transform abilityChargeBar, abilityOverChargeBar;

    [SerializeField] private float equipmentChargeAmount, equipmentOverchargeAmount, abilityChargeAmount, abilityOverChargeAmount;
    [SerializeField] private float speed;

    public static bool canUseSmoke = false, resetEquipment = false;
    public bool resetE = false;


    private void Start()
    {
        equipmentCharge = equipmentChargeBar.GetComponent<Image>();

        //set the bars = 0 at the begining
        equipmentCharge.fillAmount = 0;
        equipmentOverChargeBar.GetComponent<Image>().fillAmount = 0;
        abilityChargeBar.GetComponent<Image>().fillAmount = 0;
        abilityOverChargeBar.GetComponent<Image>().fillAmount = 0;
    }

    void Update()
    {
        resetE = resetEquipment;

        ChargeBars();
        CheckIfCanUseSmoke();
        //Debug.Log(canUseSmoke);

        //Debug.Log(resetEquipment);

        if(resetE)
        {
            //resetEquipment = false;
            //equipmentChargeAmount = 0;
            //ResetEquipment();
        }

        if (Input.GetKeyDown(KeyCode.Space) && resetEquipment)
        {
            //resetEquipment = false;
            //Debug.Log("Test");
            //equipmentCharge.fillAmount = 0;
            //equipmentChargeAmount = 0;
        }
    }

    private void ResetEquipment()
    {
        equipmentChargeAmount = 0;
    }

    private void CheckIfCanUseSmoke()
    {
        if (equipmentCharge.fillAmount == 1)
        {
            canUseSmoke = true;
        }
        else
            canUseSmoke = false;
    }

    private void ChargeBars()
    {
        //if the grenade bar isnt full add to it
        if (equipmentCharge.fillAmount < 1)
        {
            equipmentChargeAmount += speed * Time.deltaTime;
            equipmentCharge.fillAmount = equipmentChargeAmount / 100;
        }

        if(useOverCharge)
        {
            //if the grenade bar is full add to the overcharge bar
            if (equipmentCharge.fillAmount == 1)
            {
                equipmentOverchargeAmount += speed * Time.deltaTime;
                equipmentOverChargeBar.GetComponent<Image>().fillAmount = equipmentOverchargeAmount / 100;
            }
        }
        


        //if the nitroguzzler bar isnt full add to it
        if (abilityChargeBar.GetComponent<Image>().fillAmount < 1)
        {
            abilityChargeAmount += speed * Time.deltaTime;
            abilityChargeBar.GetComponent<Image>().fillAmount = abilityChargeAmount / 100;
        }

        if(useOverCharge)
        {
            //if the nitroguzzler bar is full add to the overcharge bar
            if (abilityChargeBar.GetComponent<Image>().fillAmount == 1)
            {
                abilityOverChargeAmount += speed * Time.deltaTime;
                abilityOverChargeBar.GetComponent<Image>().fillAmount = abilityOverChargeAmount / 100;
            }
        }
    }
}
