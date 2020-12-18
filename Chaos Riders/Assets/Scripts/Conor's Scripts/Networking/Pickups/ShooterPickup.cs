using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ShooterPickup : MonoBehaviour
{
    public bool hasRPG = false;
    bool rpgOn = false;
    private PhotonView pv;
    [SerializeField] private GameObject rpgUI;

    private void Awake()
    {
        pv = GetComponent<PhotonView>();
        rpgUI.SetActive(false);
    }

    private void Update()
    {
        //use this to make the rocket launcher active and inactive
        rpgOn = GetComponent<Shooter>().RPG;
        if (rpgOn)
            rpgUI.SetActive(true);
        else
            rpgUI.SetActive(false);
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
