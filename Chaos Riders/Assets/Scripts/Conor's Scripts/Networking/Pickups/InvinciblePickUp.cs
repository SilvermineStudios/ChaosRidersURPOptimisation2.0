﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvinciblePickUp : MonoBehaviour
{
    public bool pickedUp = false;
    private MeshRenderer[] meshRenderers;

    [SerializeField] private AudioClip pickUpSound;
    private AudioSource audioS;

    void Start()
    {
        meshRenderers = this.GetComponentsInChildren<MeshRenderer>();
        audioS = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!pickedUp)
        {
            pickedUp = true;
            audioS.PlayOneShot(pickUpSound);

            StartCoroutine(Timer(PickupManager.pickupRespawnTime));
        }
    }

    private IEnumerator Timer(float time)
    {
        //invincible
        foreach (MeshRenderer meshR in meshRenderers)
        {
            meshR.enabled = false;
        }

        yield return new WaitForSeconds(time);

        pickedUp = false;
        foreach (MeshRenderer meshR in meshRenderers)
        {
            meshR.enabled = true;
        }
    }
}
