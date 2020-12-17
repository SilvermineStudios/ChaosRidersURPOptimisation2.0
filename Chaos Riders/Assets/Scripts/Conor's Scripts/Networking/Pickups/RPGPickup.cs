using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPGPickup : MonoBehaviour
{
    public bool pickedUp = false;
    [SerializeField] private MeshRenderer[] meshRenderers;

    [SerializeField] private AudioClip pickUpSound;
    private AudioSource audioS;

    void Start()
    {
        audioS = GetComponent<AudioSource>();
        meshRenderers = this.GetComponentsInChildren<MeshRenderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag != "car")
        if (!pickedUp)
        {
            pickedUp = true;
            
            audioS.PlayOneShot(pickUpSound);

            StartCoroutine(Timer(PickupManager.pickupRespawnTime));
        }
    }

    private IEnumerator Timer(float time)
    {
        foreach (MeshRenderer meshR in meshRenderers)
        {
            meshR.enabled = false;
        }

        yield return new WaitForSeconds(time);

        pickedUp = false;
        foreach(MeshRenderer meshR in meshRenderers)
        {
            meshR.enabled = true;
        }
    }
}
