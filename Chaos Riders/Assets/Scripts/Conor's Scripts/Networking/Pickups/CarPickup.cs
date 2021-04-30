using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class CarPickup : MonoBehaviour
{
    private PhotonView pv;
    private Controller carController;
    [HideInInspector] public bool hasRPG = false;
    private bool hasPickup = false;
    private Target healthScript;
    
    [Header("Nitro Boost")]
    [SerializeField] private Transform nitroCooldownBar;
    [SerializeField] private GameObject nitroUiImage;
    [SerializeField] private GameObject nitroVFX;
    private bool hasSpeedBoost = false;
    private float nitroTimerNormalized;
    private float nitroStartTimer;
    private float nitroCurrentTimer;
    private bool nitroTimerCountDown = false;

    [Header("Invincible")]
    [SerializeField] private Transform invincibleCooldownBar;
    [SerializeField] private GameObject armourUiImage;
    private bool hasInvincibilityPickup = false;
    private float invincibleTimerNormalized;
    private float invincibleStartTimer;
    private float invincibleCurrentTimer;
    private bool invincibleTimerCountDown = false;


    void Start()
    {
        carController = GetComponent<Controller>();
        healthScript = GetComponent<Target>();

        nitroUiImage.SetActive(false);
        armourUiImage.SetActive(false);

        pv = GetComponent<PhotonView>();

        nitroStartTimer = PickupManager.speedBoostTime;
        invincibleStartTimer = PickupManager.InvincibleTime;

        if (nitroVFX != null)
            nitroVFX.SetActive(false);
    }

    private void Update()
    {
        //Debug.Log(PickupManager.speedBoostTime);
        if (pv.IsMine && IsThisMultiplayer.Instance.multiplayer || !IsThisMultiplayer.Instance.multiplayer)
        {
            //if (!shooter.GetComponent<Shooter>().RPG)
            //hasRPG = false;

            if (hasSpeedBoost || hasInvincibilityPickup) //<---------------------------------------------------------Add all types of pickup bools here as more are added
                hasPickup = true;
            else
                hasPickup = false;

            //player can speedboost by pressing the space bar when they have one
            if (hasSpeedBoost && (Input.GetButtonDown("Y")))// || Input.GetButtonDown("A")))
            {
                hasSpeedBoost = false;
                StartCoroutine(SpeedBoostTimer(PickupManager.speedBoostTime));
            }

            //player can activate invincibility by pressing the space bar when they have one
            if (hasInvincibilityPickup && (Input.GetButtonDown("Y")))// || Input.GetButtonDown("A")))
            {
                
                StartCoroutine(InvincibleTimer(PickupManager.InvincibleTime));
            }

            if (nitroTimerCountDown && nitroCurrentTimer > 0)
            {
                nitroCurrentTimer -= 1 * Time.deltaTime;
                nitroTimerNormalized = nitroCurrentTimer / nitroStartTimer;
                CoolDownBar(nitroTimerNormalized, nitroCooldownBar);
            }

            if (invincibleTimerCountDown && invincibleCurrentTimer > 0)
            {
                invincibleCurrentTimer -= 1 * Time.deltaTime;
                invincibleTimerNormalized = invincibleCurrentTimer / invincibleStartTimer;
                CoolDownBar(invincibleTimerNormalized, invincibleCooldownBar);
            }
                
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //if(pv.IsMine)
        //{
            //if the player picked up a speed pickup
            if (other.CompareTag("SpeedPickUp") && !hasPickup)
            {
                hasSpeedBoost = true;
                nitroCurrentTimer = nitroStartTimer;
                nitroUiImage.SetActive(true);
                nitroCooldownBar.localScale = new Vector3(1f, 1f, 1f);
            }

            //if the player picked up the invincible pickup
            if (other.CompareTag("InvinciblePickUp") && !hasPickup)
            {
                hasInvincibilityPickup = true;
                invincibleCurrentTimer = invincibleStartTimer;
                armourUiImage.SetActive(true);
                invincibleCooldownBar.localScale = new Vector3(1f, 1f, 1f);
            }


            if (other.CompareTag("RPGPickup") && !hasRPG)
                hasRPG = true;
        //}

        ////////////////////////////////////////////////////////////////////////////////////////////<---------------------check if RPC necessary
        /*
        if(pv.IsMine && IsThisMultiplayer.Instance.multiplayer)
        {
            if (other.CompareTag("RPGPickup") && !hasRPG)
            {
                pv.RPC("RPG", RpcTarget.All);
            }
        }
        */
    }

    [PunRPC]
    void RPG()
    {
        hasRPG = true;
    }

    private void CoolDownBar(float sizeNormalized, Transform transform)
    {
        transform.localScale = new Vector3(sizeNormalized, 1f); //scale the ui cooldown bar to match the ammo count
    }

    #region Puwerup Courotines
    private IEnumerator InvincibleTimer(float time)
    {
        FMODUnity.RuntimeManager.PlayOneShotAttached("event:/Pickups/PickupShield", gameObject);
        //armourUiImage.SetActive(true);
        //healthScript.isProtected = true;
        healthScript.invincible = true;
        invincibleTimerCountDown = true;
        this.GetComponent<Target>().invincible = true;
        

        yield return new WaitForSeconds(time);

        hasInvincibilityPickup = false;
        //healthScript.isProtected = false;
        healthScript.invincible = false;
        armourUiImage.SetActive(false);
        invincibleTimerCountDown = false;
        this.GetComponent<Target>().invincible = false;
    }

    
    private IEnumerator SpeedBoostTimer(float time)
    {
        FMODUnity.RuntimeManager.PlayOneShotAttached("event:/Pickups/PickupBoost", gameObject);
        //nitroUiImage.SetActive(true);
        carController.boost = true;
        nitroTimerCountDown = true;
        if (nitroVFX != null)
            nitroVFX.SetActive(true);

        yield return new WaitForSeconds(time);

        carController.boost = false;
        nitroUiImage.SetActive(false);
        nitroTimerCountDown = false;
        if (nitroVFX != null)
            nitroVFX.SetActive(false);
    }
    #endregion
}
