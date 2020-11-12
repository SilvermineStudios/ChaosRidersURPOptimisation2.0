using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class RaycastMinigun : MonoBehaviour
{
    public GameObject spawnpoint,barrel;
    public GameObject pointer;
    public float timeSinceLastBullet, fireRate;
    public PhotonView pv;
    private AudioSource speaker;
    public AudioClip gunShot;

    public LayerMask layerMask;

    private void Start()
    {
        pv = transform.parent.parent.parent.GetComponent<PhotonView>();
        timeSinceLastBullet = fireRate;
        speaker = GetComponent<AudioSource>();
    }

    void Update()
    {
        
        if (!pv.IsMine && IsThisMultiplayer.multiplayer) { return; }

        if ((Input.GetAxis("RT") > 0.01f || Input.GetButton("Fire1")) && (pv.IsMine || !IsThisMultiplayer.multiplayer))
        {
            if (timeSinceLastBullet > fireRate)
            {
                fireBullet();
                timeSinceLastBullet = 0;
            }
            else
            {
                timeSinceLastBullet += Time.deltaTime;
            }
        }
        else
        {
            timeSinceLastBullet = fireRate;
        }
    }

    Vector3 raycastDir;
    void fireBullet()
    {
        RaycastHit hit;
        raycastDir = pointer.transform.position - transform.position;


        if (Physics.Raycast(spawnpoint.transform.position, raycastDir, out hit, Mathf.Infinity, layerMask))
        {
            Debug.DrawRay(transform.position, raycastDir * hit.distance, Color.yellow);
            Debug.Log("Did Hit");
        }
        else
        {
            Debug.DrawRay(transform.position, raycastDir * 1000, Color.white);
            Debug.Log("Did not Hit");
        }

        speaker.PlayOneShot(gunShot);
    }


}