using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using FMODUnity;
using Photon.Pun;

public class Ping : MonoBehaviour
{
    [SerializeField] CanPing canPing;
    [SerializeField] float pingRadius = 1;
    [SerializeField] KeyCode pingButton = KeyCode.Q;
    [SerializeField] LayerMask IgnoreWalls;
    RaycastHit[] hits;
    CinemachineVirtualCamera cineCamera;
    GameObject car;
    PhotonView pv;
    PhotonView driverPv;
    bool isPing;

    void Start()
    {
        cineCamera = GetComponent<Shooter>().cineCamera;
        pv = GetComponent<PhotonView>();
     }

    private void Update()
    {
        isPing = Input.GetKey(pingButton);
    }

    void FixedUpdate()
    {

        if (pv.IsMine && car == null)
        {
            if (GetComponent<Shooter>().car != null)
            {
                car = GetComponent<Shooter>().car;
                driverPv = car.GetComponent<PhotonView>();
            }
        }

        if (isPing && pv.IsMine)
        {
            Debug.Log("Attempted Ping");
            hits = Physics.SphereCastAll(cineCamera.transform.position, pingRadius, cineCamera.transform.forward, 9999, IgnoreWalls);
           
            foreach (RaycastHit hit in hits)
            {
                if (canPing.tags.Contains(hit.transform.gameObject.tag) && hit.transform.gameObject != car)
                {
                    Debug.Log(hit.transform.gameObject.tag + " Success");
                    hit.transform.gameObject.SendMessage("RelayPingToOutline", driverPv);
                    break;
                }
                else
                {
                    if(hit.transform.gameObject == car)
                    {
                        Debug.Log("My car Fail");
                    }
                    else
                       Debug.Log(hit.transform.gameObject.tag + " Fail");

                }
                    
            }

            
            
        }
        //Debug.DrawRay(cineCamera.transform.position, cineCamera.transform.forward * 100, Color.red);
    }
}
