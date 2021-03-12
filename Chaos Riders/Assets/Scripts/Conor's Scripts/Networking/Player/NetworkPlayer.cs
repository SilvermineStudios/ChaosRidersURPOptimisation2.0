using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NetworkPlayer : MonoBehaviourPun, IPunObservable
{
    protected Controller Player;
    protected Vector3 RemotePlayerPosition;
    
    void Awake()
    {
        Player = GetComponent<Controller>();
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
            return;

        var LagDistance = RemotePlayerPosition - transform.position;

        if(LagDistance.magnitude > 5f)
        {
            transform.position = RemotePlayerPosition;
            LagDistance = Vector3.zero;
        }

        if(LagDistance.magnitude < 0.11f)
        {
            //player is nearly at the point

            //Player.Input.RunX = 0;
            //Player.Input.Runz = 0;
        }
        else
        {
            //player has to go to the point

            //Player.Input.RunX = LagDistance.normalized.x;
            //Player.Input.Runz = LagDistance.normalized.z;
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
