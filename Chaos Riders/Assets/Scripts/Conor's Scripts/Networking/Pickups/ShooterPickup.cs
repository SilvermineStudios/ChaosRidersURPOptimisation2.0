using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ShooterPickup : MonoBehaviour
{
    private PhotonView pv;

    public bool hasRPG = false;
    bool rpgOn = false;
    [SerializeField] private GameObject rpgUI;
    [SerializeField] private GameObject gunbarrelHolder;
    [SerializeField] private MeshRenderer[] gunPartsToMakeInvisible;


    private void Awake()
    {
        pv = GetComponent<PhotonView>();
        rpgUI.SetActive(false);
    }

    private void Update()
    {
        gunPartsToMakeInvisible = gunbarrelHolder.GetComponentsInChildren<MeshRenderer>();

        //use this to make the rocket launcher active and inactive
        rpgOn = GetComponent<Shooter>().RPG;
        if (rpgOn)
        {
            rpgUI.SetActive(true);
            foreach (MeshRenderer mr in gunPartsToMakeInvisible)
            {
                mr.enabled = false;
            }
        }
        else
        {
            rpgUI.SetActive(false);
            foreach (MeshRenderer mr in gunPartsToMakeInvisible)
            {
                mr.enabled = true;
            }
        }
            
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
