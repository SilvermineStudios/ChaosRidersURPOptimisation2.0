using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class CarPickup : MonoBehaviour
{
    private GameObject go;
    Health healthScript;
    [SerializeField] private bool hasSpeedBoost = false;
    public bool hasRPG = false;
    [SerializeField] private GameObject shooter;
    [SerializeField] private GameObject nitroUiImage, armourUiImage;

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
    }

    private void Update()
    {
        Debug.Log(PickupManager.speedBoostTime);
        if (pv.IsMine && IsThisMultiplayer.Instance.multiplayer || !IsThisMultiplayer.Instance.multiplayer)
        {
            shooter = GetComponent<Controller>().Shooter;
            //if (!shooter.GetComponent<Shooter>().RPG)
                //hasRPG = false;
        }

        //player can speedboost by pressing the space bar when they have one
        if (hasSpeedBoost && (Input.GetKeyDown(KeyCode.Space)))// || Input.GetButtonDown("A")))
        {
            hasSpeedBoost = false;
            StartCoroutine(SpeedBoostTimer(PickupManager.speedBoostTime));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (pv.IsMine && IsThisMultiplayer.Instance.multiplayer || !IsThisMultiplayer.Instance.multiplayer)
        {
            //if the player picked up a speed pickup
            if (other.CompareTag("SpeedPickUp")) //the or is for testing when not in multiplayer
                hasSpeedBoost = true;

            //if the player picked up the invincible pickup
            if (other.CompareTag("InvinciblePickUp")) //the or is for testing when not in multiplayer
                StartCoroutine(InvincibleTimer(PickupManager.InvincibleTime));

            if (other.CompareTag("RPGPickup") && !hasRPG)
                hasRPG = true;
        }

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

    #region Puwerup Courotines
    private IEnumerator InvincibleTimer(float time)
    {
        armourUiImage.SetActive(true);
        healthScript.isProtected = true;

        yield return new WaitForSeconds(time);

        healthScript.isProtected = false;
        armourUiImage.SetActive(false);
    }

    
    private IEnumerator SpeedBoostTimer(float time)
    {
        nitroUiImage.SetActive(true);
        carController.boost = true;
        yield return new WaitForSeconds(time);
        carController.boost = false;
        nitroUiImage.SetActive(false);
    }
    #endregion
}
