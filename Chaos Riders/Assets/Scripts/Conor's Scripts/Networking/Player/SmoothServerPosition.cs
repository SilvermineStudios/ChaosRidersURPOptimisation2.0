using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class SmoothServerPosition : MonoBehaviourPun, IPunObservable
{
    protected Player Player;


    void Awake()
    {
        Player = GetComponent<Player>();
    }

    void Update()
    {
        
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        throw new System.NotImplementedException();
    }
}
