using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class DriverAbilities : MonoBehaviour
{
    public GameObject driverCanvas; //Driver UI canvas
    private Transform uiTransform; //Transform of the UI Canvas

    public GameObject smokeGameObjectPrefab, smokeSpawn;

    [SerializeField] private Transform equipmentChargeBar, equipmentOverChargeBar, abilityChargeBar, abilityOverChargeBar; //equipment/ability chargebars
    [SerializeField] private float equipmentChargeAmount, equipmentOverchargeAmount, abilityChargeAmount, abilityOverChargeAmount; //equipment/ability charge Amount
    [SerializeField] private float speed = 8f;
    [SerializeField] private bool useOverCharge = true;
    [SerializeField] private bool canUseEquipment = false;

    private PhotonView pv; //my Photon View
 
    void Start()
    {
        uiTransform = driverCanvas.transform;
        pv = GetComponent<PhotonView>();


        if (pv.IsMine && IsThisMultiplayer.Instance.multiplayer || !IsThisMultiplayer.Instance.multiplayer)
        {
            driverCanvas.SetActive(true); //activate the drivers UI canvas
            ResetAllBars(); //set all the bars to 0
            CheckIfCanUseEquipment(); //check if the player can use their equipment
        } 
    }

    // Update is called once per frame
    void Update()
    {
        if (pv.IsMine && IsThisMultiplayer.Instance.multiplayer || !IsThisMultiplayer.Instance.multiplayer)
        {
            ChargeBars(); //charge the equipment and ability bars
            CheckIfCanUseEquipment(); //check if the player can use their equipment

            //if you use the equipment
            if (Input.GetKeyDown(KeyCode.Space) && canUseEquipment)
            {
                Instantiate(smokeGameObjectPrefab, smokeSpawn.transform.position, smokeSpawn.transform.rotation);
                equipmentChargeAmount = 0;
            }
        }
    }

    //sets all bars to 0
    private void ResetAllBars()
    {
        equipmentChargeBar.GetComponent<Image>().fillAmount = 0;
        equipmentOverChargeBar.GetComponent<Image>().fillAmount = 0;
        abilityChargeBar.GetComponent<Image>().fillAmount = 0;
        abilityOverChargeBar.GetComponent<Image>().fillAmount = 0;
    }

    //check if the player can use their equipment
    private void CheckIfCanUseEquipment()
    {
        if (equipmentChargeAmount >= 100)
        {
            canUseEquipment = true;
        }
        else
            canUseEquipment = false;
    }


    //charge the equipment and ability bars
    private void ChargeBars()
    {
        //if the grenade bar isnt full add to it
        if (equipmentChargeAmount < 100)
        {
            equipmentChargeAmount += speed * Time.deltaTime;
            equipmentChargeBar.GetComponent<Image>().fillAmount = equipmentChargeAmount / 100;
        }

        if (useOverCharge)
        {
            //if the grenade bar is full add to the overcharge bar
            if (equipmentChargeAmount >= 100)
            {
                equipmentOverchargeAmount += speed * Time.deltaTime;
                equipmentOverChargeBar.GetComponent<Image>().fillAmount = equipmentOverchargeAmount / 100;
            }
        }

        

        //if the nitroguzzler bar isnt full add to it
        if (abilityChargeAmount < 100)
        {
            abilityChargeAmount += speed * Time.deltaTime;
            abilityChargeBar.GetComponent<Image>().fillAmount = abilityChargeAmount / 100;
        }

        if (useOverCharge)
        {
            //if the nitroguzzler bar is full add to the overcharge bar
            if (abilityChargeAmount >= 100)
            {
                abilityOverChargeAmount += speed * Time.deltaTime;
                abilityOverChargeBar.GetComponent<Image>().fillAmount = abilityOverChargeAmount / 100;
            }
        }
    }
}
