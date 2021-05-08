using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Photon.Pun;

public class FindPlayerCamera : MonoBehaviour
{
    public static CinemachineVirtualCamera cineCam { get; private set; }

    PhotonView[] photonViews;

    bool foundCam;

    PhotonView myPV;


    //TODO Remove reference if Player leaves

    void FixedUpdate()
    {
        if (!foundCam)
        {
            photonViews = FindObjectsOfType<PhotonView>();
            foreach (PhotonView pv in photonViews)
            {
                if(pv.IsMine)
                {
                    myPV = pv;
                    if(pv.gameObject.transform.root.TryGetComponent<LinkToCM>(out var LinkToCM))
                    {
                        cineCam = pv.gameObject.transform.root.GetComponent<LinkToCM>().CMcamera;
                        foundCam = true;
                    }
                    break;
                }
            }
        }
    }
}
