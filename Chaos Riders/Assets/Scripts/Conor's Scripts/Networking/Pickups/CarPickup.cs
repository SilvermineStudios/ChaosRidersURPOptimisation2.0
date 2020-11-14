using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class CarPickup : MonoBehaviour
{
    private GameObject go;

    [SerializeField] private bool hasSpeedBoost = false;
    private PhotonView pv;

    private DriveTrainMultiplayer dt;

    void Start()
    {
        go = this.GetComponent<GameObject>();
        pv = GetComponent<PhotonView>();
        dt = GetComponent<DriveTrainMultiplayer>();
    }

    private void Update()
    {
        //player can speedboost by pressing the w key when they have one
        if (hasSpeedBoost && (Input.GetKeyDown(KeyCode.Space)))// || Input.GetButtonDown("A")))
        {
            hasSpeedBoost = false;
            StartCoroutine(SpeedBoostTimer(PickupManager.speedBoostTime));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //if the player picked up a speed pickup
        if (other.CompareTag("SpeedPickUp") && pv.IsMine || other.CompareTag("SpeedPickUp") && !IsThisMultiplayer.Instance.multiplayer) //the or is for testing when not in multiplayer
            hasSpeedBoost = true;

        //if the player picked up the invincible pickup
        if (other.CompareTag("InvinciblePickUp") && pv.IsMine || other.CompareTag("InvinciblePickUp") && !IsThisMultiplayer.Instance.multiplayer) //the or is for testing when not in multiplayer
            StartCoroutine(InvincibleTimer(PickupManager.InvincibleTime));
    }

    private IEnumerator InvincibleTimer(float time)
    {
        //Debug.Log("Invincible");
        PickupManager.invincibleUI.SetActive(true);

        yield return new WaitForSeconds(time);

        PickupManager.invincibleUI.SetActive(false);
        //Debug.Log("Not Invincible");
    }

    ///////////////////////////////////////////////////
    private IEnumerator SpeedBoostTimer(float time)
    {
        dt.nitro = true;


        //Debug.Log("Speed boost");
        PickupManager.speedUI.SetActive(true);

        yield return new WaitForSeconds(time);

        PickupManager.speedUI.SetActive(false);
        //Debug.Log("Normal speed");

        dt.nitro = false;
    }
}
