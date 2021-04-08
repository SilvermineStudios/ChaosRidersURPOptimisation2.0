using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedPickup : MonoBehaviour
{
    public bool pickedUp = false;
    private MeshRenderer[] meshRenderers;

    [SerializeField] private AudioClip pickUpSound;

    void Start()
    {
        meshRenderers = this.GetComponentsInChildren<MeshRenderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!pickedUp)
        {
            pickedUp = true;
            FMODUnity.RuntimeManager.PlayOneShotAttached("event:/Pickups/PickupItem", other.gameObject);

            StartCoroutine(Timer(PickupManager.pickupRespawnTime));
        }
    }

    private IEnumerator Timer(float time)
    {
        //add speed boost here
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
