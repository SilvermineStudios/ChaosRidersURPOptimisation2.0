using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class SmoothServerPosition : MonoBehaviourPun, IPunObservable
{
    protected Controller car;
    private PhotonView pv;
    protected Vector3 RemotePlayerPosition;

    void Awake()
    {
        car = GetComponent<Controller>();
        pv = GetComponent<PhotonView>();
    }

    void Update()
    {
        if (pv.IsMine)
            return;

        var LagDistance = RemotePlayerPosition - transform.position;

        //High Distance => sync is to much off => send to position
        if(LagDistance.magnitude > 5f) //if the player is lagging too much
        {
            transform.position = RemotePlayerPosition; //move the car to the remote position
            LagDistance = Vector3.zero;
        }

        if(LagDistance.magnitude < 0.11f)
        {
            
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            stream.SendNext(transform.position);
        }
        else
        {
            RemotePlayerPosition = (Vector3)stream.ReceiveNext();
        }
    }
}
