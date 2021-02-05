using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class DriverAbilities : MonoBehaviour
{

    enum Abilities { SmokeScreen, Mine }
    [SerializeField] Abilities CurrentAbility;
    public GameObject abilitySpawn, smokeGameObject, mineGameObject;

    [SerializeField] private KeyCode abilityKeyCode = KeyCode.Q, equipmentKeyCode = KeyCode.E; //Create Keycode Variables for the buttons

    [SerializeField] private Transform equipmentChargeBar, equipmentOverChargeBar, abilityChargeBar, abilityOverChargeBar; //equipment/ability chargebars
    [SerializeField] private float equipmentChargeAmount, equipmentOverchargeAmount, abilityChargeAmount, abilityOverChargeAmount; //equipment/ability charge Amount
    [SerializeField] private float equipmentChargeSpeed = 8f, abilityChargeSpeed = 2f;
    [SerializeField] private bool useOverCharge = true;
    [SerializeField] private bool canUseEquipment = false, canUseAbility = false;

    private PhotonView pv; //my Photon View
    private Animator anim;
    private Controller carController; //my Car Controller
    [SerializeField] private AudioSource speaker;
    [SerializeField] AudioClip AbilitySound;
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        pv = GetComponent<PhotonView>();
        carController = GetComponent<Controller>();
        
        if (pv.IsMine && IsThisMultiplayer.Instance.multiplayer || !IsThisMultiplayer.Instance.multiplayer)
        {
            ResetAllBars(); //set all the bars to 0
            CheckIfCanUseEquipmentAndAbility(); //check if the player can use their equipment
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (pv.IsMine && IsThisMultiplayer.Instance.multiplayer || !IsThisMultiplayer.Instance.multiplayer)
        {
            ChargeBars(); //charge the equipment and ability bars
            CheckIfCanUseEquipmentAndAbility(); //check if the player can use their equipment

            //if you use the equipment
            if (Input.GetKeyDown(equipmentKeyCode) && canUseEquipment)
            {
                speaker.PlayOneShot(AbilitySound);

                if (CurrentAbility == Abilities.SmokeScreen)
                {
                    //spawn the smoke grenade accross the network
                    if (IsThisMultiplayer.Instance.multiplayer)
                        PhotonNetwork.Instantiate("Smoke Particle", abilitySpawn.transform.position, abilitySpawn.transform.rotation, 0);

                    //spawn the smoke grenade in single player
                    if (!IsThisMultiplayer.Instance.multiplayer)
                        Instantiate(smokeGameObject, abilitySpawn.transform.position, abilitySpawn.transform.rotation);
                }
                if(CurrentAbility == Abilities.Mine)
                {
                    //spawn the mine accross the network
                    if (IsThisMultiplayer.Instance.multiplayer)
                        PhotonNetwork.Instantiate("Mine", abilitySpawn.transform.position, abilitySpawn.transform.rotation, 0);

                    //spawn the mine in single player
                    if (!IsThisMultiplayer.Instance.multiplayer)
                        Instantiate(mineGameObject, abilitySpawn.transform.position, abilitySpawn.transform.rotation);
                }
                equipmentChargeAmount = 0; //reset the cooldownbar after the equipment is used
            }

            //if you use the Ability
            if (Input.GetKeyDown(abilityKeyCode) && canUseAbility)
            {
                //<----------------------------------------------------------------------------------------------------------------------------PUT THE ABILITY STUFF HERE
                
                StartCoroutine(UseBrakerAbility());

                abilityChargeAmount = 0; //reset the cooldownbar after the ability is used
            }
        }
    }

    private IEnumerator UseBrakerAbility()
    {
        anim.SetTrigger("BreakerTransTrigger");
        //brake
        carController.ApplyBrake(30000000);
        
        yield return new WaitForSeconds(1.5f);
        carController.ReleaseBrake();
        anim.SetTrigger("LeaveBreakerTrigger");
        //speed

        carController.boost = true;

        yield return new WaitForSeconds(5.5f);
        carController.boost = false;
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
    private void CheckIfCanUseEquipmentAndAbility()
    {
        //check if the equipment bar is full
        if (equipmentChargeAmount >= 100)
            canUseEquipment = true;
        else
            canUseEquipment = false;


        //check if the ability bar is full
        if (abilityChargeAmount >= 100)
            canUseAbility = true;
        else
            canUseAbility = false;
    }


    //charge the equipment and ability bars
    private void ChargeBars()
    {
        //if the qeuipment bar isnt full add to it
        if (equipmentChargeAmount < 100)
        {
            equipmentChargeAmount += equipmentChargeSpeed * Time.deltaTime;
            equipmentChargeBar.GetComponent<Image>().fillAmount = equipmentChargeAmount / 100;
        }

        if (useOverCharge)
        {
            //if the equipment bar is full add to the overcharge bar
            if (equipmentChargeAmount >= 100)
            {
                equipmentOverchargeAmount += equipmentChargeSpeed * Time.deltaTime;
                equipmentOverChargeBar.GetComponent<Image>().fillAmount = equipmentOverchargeAmount / 100;
            }
        }

        

        //if the ability bar isnt full add to it
        if (abilityChargeAmount < 100)
        {
            abilityChargeAmount += abilityChargeSpeed * Time.deltaTime;
            abilityChargeBar.GetComponent<Image>().fillAmount = abilityChargeAmount / 100;
        }

        if (useOverCharge)
        {
            //if the ability bar is full add to the overcharge bar
            if (abilityChargeAmount >= 100)
            {
                abilityOverChargeAmount += abilityChargeSpeed * Time.deltaTime;
                abilityOverChargeBar.GetComponent<Image>().fillAmount = abilityOverChargeAmount / 100;
            }
        }
    }
}
