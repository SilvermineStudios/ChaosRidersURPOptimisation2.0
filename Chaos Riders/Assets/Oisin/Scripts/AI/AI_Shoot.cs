using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Shoot : MonoBehaviour
{

    public GameObject spawnpoint;
    public GameObject bullet;

    public float timeSinceLastBullet, fireRate;
    private AudioSource speaker;
    public AudioClip gunShot;

    AI_Aiming aiScript;

    void Start()
    {
        aiScript = GetComponentInParent<AI_Aiming>();
        timeSinceLastBullet = fireRate;
        speaker = GetComponent<AudioSource>();
    }



    void Update()
    {
        if (aiScript.targetTransform != null)
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


    void fireBullet()
    {
        GameObject fired = Instantiate(bullet, spawnpoint.transform.position, transform.rotation);
        fired.GetComponent<Rigidbody>().AddForce(transform.forward * 100, ForceMode.Impulse);

        speaker.PlayOneShot(gunShot);
    }
}
