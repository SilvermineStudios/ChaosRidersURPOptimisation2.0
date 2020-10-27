using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject spawnpoint;
    public GameObject bullet;

    public float timeSinceLastBullet, fireRate;

    private void Start()
    {
        timeSinceLastBullet = fireRate;
    }

    void Update()
    {
        if(Input.GetAxis("RT") > 0.01f)
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
    }
}
