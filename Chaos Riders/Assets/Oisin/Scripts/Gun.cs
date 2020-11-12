using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Gun : MonoBehaviour
{
    public GameObject spawnpoint;
    public GameObject bullet;

    public float timeSinceLastBullet, fireRate;
    public PhotonView pv;
    private AudioSource speaker;
    public AudioClip gunShot;


    private void Start()
    {
        pv = transform.parent.parent.parent.GetComponent<PhotonView>();
        timeSinceLastBullet = fireRate;
        speaker = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!pv.IsMine && IsThisMultiplayer.Instance.multiplayer) { return; }

        if ((Input.GetAxis("RT") > 0.01f || Input.GetButton("Fire1")) && (pv.IsMine || !IsThisMultiplayer.Instance.multiplayer) )
        {
            if(timeSinceLastBullet > fireRate)
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


    void fireBullet()
    {
        GameObject fired = Instantiate(bullet, spawnpoint.transform.position, transform.rotation);
        fired.GetComponent<Rigidbody>().AddForce(transform.forward * 100, ForceMode.Impulse);

        speaker.PlayOneShot(gunShot);
    }
}
