using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.VFX;

public class MuzzleFlashTest : MonoBehaviour
{
    public GameObject muzzleFlashGo;
    

    // Start is called before the first frame update
    void Awake()
    {
        muzzleFlashGo.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        Shoot();
    }

    void Shoot()
    {
        if (Input.GetKey(KeyCode.W))
        {
            muzzleFlashGo.SetActive(true);
        }
        else
        {
            muzzleFlashGo.SetActive(false);
        }
    }
}
