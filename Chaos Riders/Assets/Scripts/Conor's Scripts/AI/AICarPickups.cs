using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICarPickups : MonoBehaviour
{
    public bool hasRPG = false;

    private PhotonView pv;

    void Awake()
    {
        pv = GetComponent<PhotonView>();
    }


    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (pv.IsMine && IsThisMultiplayer.Instance.multiplayer || !IsThisMultiplayer.Instance.multiplayer)
        {
            if (other.CompareTag("RPGPickup") && !hasRPG)
                hasRPG = true;
        }

        if (pv.IsMine && IsThisMultiplayer.Instance.multiplayer)
        {
            if (other.CompareTag("RPGPickup") && !hasRPG)
            {
                //pv.RPC("RPG", RpcTarget.All);
            }
        }
    }

    [PunRPC]
    void RPG()
    {
        hasRPG = true;
    }
}
