using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvinciblePickUp : MonoBehaviour
{
    public bool pickedUp = false;
    private MeshRenderer myMesh;

    [SerializeField] private AudioClip pickUpSound;
    private AudioSource audioS;

    void Start()
    {
        myMesh = GetComponent<MeshRenderer>();
        audioS = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!pickedUp)
        {
            pickedUp = true;
            myMesh.enabled = false;
            audioS.PlayOneShot(pickUpSound);

            StartCoroutine(Timer(PickupManager.pickupRespawnTime));
        }
    }

    private IEnumerator Timer(float time)
    {
        yield return new WaitForSeconds(time);

        pickedUp = false;
        myMesh.enabled = true;
    }
}
