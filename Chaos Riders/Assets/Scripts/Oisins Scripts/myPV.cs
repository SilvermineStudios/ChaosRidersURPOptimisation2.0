using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class myPV : MonoBehaviour
{
    public PhotonView pv { get; private set; }

    void Start()
    {
        pv = GetComponent<PhotonView>();
    }
}
