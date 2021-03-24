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
    RaycastHit hit;
    CinemachineVirtualCamera cineCamera;
    GameObject car;
    PhotonView pv;
    PhotonView driverPv;

    void Start()
    {
        cineCamera = GetComponent<Shooter>().cineCamera;
        pv = GetComponent<PhotonView>();
     }

    void Update()
    {

        if (pv.IsMine && car == null)
        {
            if (GetComponent<Shooter>().car != null)
            {
                car = GetComponent<Shooter>().car;
                driverPv = car.GetComponent<PhotonView>();
            }
        }

        if (Input.GetKeyDown(pingButton) && pv.IsMine)
        {
            if (Physics.SphereCast(cineCamera.transform.position, pingRadius, cineCamera.transform.forward, out hit, 9999, IgnoreWalls))
            {
                if (canPing.tags.Contains(hit.transform.gameObject.tag) && hit.transform.gameObject != car)
                {
                    Debug.Log(hit.transform.gameObject.tag);
                    hit.transform.gameObject.SendMessage("RelayPingToOutline", driverPv);
                }
            }
        }
        //Debug.DrawRay(cineCamera.transform.position, cineCamera.transform.forward * 100, Color.red);
    }
}
