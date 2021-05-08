using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ScoreTracker : MonoBehaviourPun
{
    public static ScoreTracker singleton;
    public int i;
    PhotonView pv;
    private void Awake()
    {
        if (singleton != null && singleton != this)
            Destroy(this);
        singleton = this;
        pv = GetComponent<PhotonView>();
    }


    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            CallSetScore(2);
        }
    }

    //Called by someone who wants to set the score
    public void CallSetScore(sbyte score)
    {
        this.pv.RPC("SetScore", RpcTarget.AllViaServer, score);
    }

    [PunRPC]
    public void SetScore(sbyte score)
    {
        i += score; ;
    }
}
