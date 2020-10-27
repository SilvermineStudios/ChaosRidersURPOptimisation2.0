using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarPickup : MonoBehaviour
{
    private GameObject go;

    private bool hasSpeedBoost = false;

    void Start()
    {
        go = this.GetComponent<GameObject>();
    }

    private void Update()
    {
        //player can speedboost by pressing the w key when they have one
        if (hasSpeedBoost && Input.GetKeyDown(KeyCode.W))
        {
            hasSpeedBoost = false;
            StartCoroutine(SpeedBoostTimer(PickupManager.speedBoostTime));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //if the player picked up a speed pickup
        if (other.CompareTag("SpeedPickUp"))
        {
            hasSpeedBoost = true;
        }

        if (other.CompareTag("InvinciblePickUp"))
        {
            StartCoroutine(InvincibleTimer(PickupManager.InvincibleTime));
        }
    }

    private IEnumerator InvincibleTimer(float time)
    {
        Debug.Log("Invincible");
        PickupManager.invincibleUI.SetActive(true);

        yield return new WaitForSeconds(time);

        PickupManager.invincibleUI.SetActive(false);
        Debug.Log("Not Invincible");
    }

    ///////////////////////////////////////////////////
    private IEnumerator SpeedBoostTimer(float time)
    {
        Debug.Log("Speed boost");

        yield return new WaitForSeconds(time);

        Debug.Log("Normal speed");
        
    }
}
