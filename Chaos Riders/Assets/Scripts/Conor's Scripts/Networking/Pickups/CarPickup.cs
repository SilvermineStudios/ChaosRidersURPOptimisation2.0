using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class CarPickup : MonoBehaviour
{
    private GameObject go;
    Health healthScript;
    [SerializeField] private bool hasSpeedBoost = false, hasInvincibilityPickup = false; //<-------------use this bool in target script
    private bool hasPickup = false;
    public bool hasRPG = false;
    [SerializeField] private GameObject shooter;
    [SerializeField] private GameObject nitroUiImage, armourUiImage;

    //CoolDown Bars
    [SerializeField] private Transform nitroCooldownBar, invincibleCooldownBar;
    [SerializeField] private float nitroTimerNormalized, invincibleTimerNormalized;
    [SerializeField] private float nitroStartTimer, invincibleStartTimer;
    [SerializeField] private float nitroCurrentTimer, invincibleCurrentTimer;
    [SerializeField] private bool nitroTimerCountDown = false, invincibleTimerCountDown = false;

    private PhotonView pv;
    private Controller carController;

    void Start()
    {
        carController = GetComponent<Controller>();
        healthScript = GetComponent<Health>();
        go = this.GetComponent<GameObject>();

        nitroUiImage.SetActive(false);
        armourUiImage.SetActive(false);

        pv = GetComponent<PhotonView>();

        nitroStartTimer = PickupManager.speedBoostTime;
        invincibleStartTimer = PickupManager.InvincibleTime;
    }

    private void Update()
    {
        //Debug.Log(PickupManager.speedBoostTime);
        if (pv.IsMine && IsThisMultiplayer.Instance.multiplayer || !IsThisMultiplayer.Instance.multiplayer)
        {
            shooter = GetComponent<Controller>().Shooter;
            //if (!shooter.GetComponent<Shooter>().RPG)
            //hasRPG = false;

            if (hasSpeedBoost || hasInvincibilityPickup) //<---------------------------------------------------------Add all types of pickup bools here as more are added
                hasPickup = true;
            else
                hasPickup = false;

            //player can speedboost by pressing the space bar when they have one
            if (hasSpeedBoost && (Input.GetKeyDown(KeyCode.Space)))// || Input.GetButtonDown("A")))
            {
                hasSpeedBoost = false;
                StartCoroutine(SpeedBoostTimer(PickupManager.speedBoostTime));
            }

            //player can activate invincibility by pressing the space bar when they have one
            if (hasInvincibilityPickup && (Input.GetKeyDown(KeyCode.Space)))// || Input.GetButtonDown("A")))
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
        if (pv.IsMine && IsThisMultiplayer.Instance.multiplayer || !IsThisMultiplayer.Instance.multiplayer)
        {
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
        }

        ////////////////////////////////////////////////////////////////////////////////////////////<---------------------check if RPC necessary
        if(pv.IsMine && IsThisMultiplayer.Instance.multiplayer)
        {
            if (other.CompareTag("RPGPickup") && !hasRPG)
            {
                pv.RPC("RPG", RpcTarget.All);
            }
        }
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
        FMODUnity.RuntimeManager.PlayOneShotAttached("event:/Pickups/Use", gameObject);
        //armourUiImage.SetActive(true);
        healthScript.isProtected = true;
        invincibleTimerCountDown = true;
        this.GetComponent<Target>().invincible = true;
        

        yield return new WaitForSeconds(time);

        hasInvincibilityPickup = false;
        healthScript.isProtected = false;
        armourUiImage.SetActive(false);
        invincibleTimerCountDown = false;
        this.GetComponent<Target>().invincible = false;
    }

    
    private IEnumerator SpeedBoostTimer(float time)
    {
        FMODUnity.RuntimeManager.PlayOneShotAttached("event:/Pickups/Use", gameObject);
        //nitroUiImage.SetActive(true);
        carController.boost = true;
        nitroTimerCountDown = true;

        yield return new WaitForSeconds(time);

        carController.boost = false;
        nitroUiImage.SetActive(false);
        nitroTimerCountDown = false;
    }
    #endregion
}
