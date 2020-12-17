using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ShooterPickup : MonoBehaviour
{
    public bool hasRPG = false;
    private PhotonView pv;

    private void Awake()
    {
        pv = GetComponent<PhotonView>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (pv.IsMine && IsThisMultiplayer.Instance.multiplayer || !IsThisMultiplayer.Instance.multiplayer)
        {
            //if (other.CompareTag("RPGPickup") && !hasRPG)
                //hasRPG = true;

            if(other.CompareTag("RPGPickup") && !GetComponent<Shooter>().RPG)
            {
                GetComponent<Shooter>().RPG = true;
                hasRPG = true;
            }
        }
    }
}
