using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class dumb : MonoBehaviour
{

    //This script moves a cube around for the gun and camera to face towards, to be replaced later when working with the actual model



    public GameObject target;
    public float Hspeed,Vspeed;
    float angle;
    private PhotonView pv;
    public GameObject playerCamera;

    private void Start()
    {
        //grabs pv from base shooter model (hopefully)
        pv = GetComponent<PhotonView>();


        if (!pv.IsMine && IsThisMultiplayer.Instance.multiplayer) { return; }

        if (pv.IsMine || !IsThisMultiplayer.Instance.multiplayer)
        {
            playerCamera.SetActive(true);
        }
        
    }

    void FixedUpdate()
    {
        if (!pv.IsMine && IsThisMultiplayer.Instance.multiplayer) { return; }

        
        if(pv.IsMine || !IsThisMultiplayer.Instance.multiplayer)
        {
            target.transform.RotateAround(transform.position, Vector3.up, Input.GetAxis("Horizontal") * Hspeed * Time.deltaTime);
            target.transform.Translate(new Vector3(0, Input.GetAxis("Vertical") * Vspeed, 0));
            if (target.transform.position.y > 3.5f)
            {
                target.transform.position = new Vector3(target.transform.position.x, 3.5f, target.transform.position.z);
            }
            else if (target.transform.position.y < -1)
            {
                target.transform.position = new Vector3(target.transform.position.x, -1, target.transform.position.z);
            }
        }
    }
}
