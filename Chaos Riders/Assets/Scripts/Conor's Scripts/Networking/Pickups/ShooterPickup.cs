using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ShooterPickup : MonoBehaviour
{
    private PhotonView pv;

    [SerializeField] private KeyCode togglePickupButton = KeyCode.Space;

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
        if (pv.IsMine && IsThisMultiplayer.Instance.multiplayer || !IsThisMultiplayer.Instance.multiplayer)
        {
            gunPartsToMakeInvisible = gunbarrelHolder.GetComponentsInChildren<MeshRenderer>();

            //use this to make the rocket launcher active and inactive
            rpgOn = GetComponent<Shooter>().RPG;

            if(hasRPG && rpgOn && Input.GetKeyDown(togglePickupButton))
            {
                //rpgOn = false;
            }

            if (hasRPG && !rpgOn && Input.GetKeyDown(togglePickupButton))
            {
                //rpgOn = true;
            }
        }

        if (pv.IsMine && IsThisMultiplayer.Instance.multiplayer)
        {
            if (rpgOn)
            {
                rpgUI.SetActive(true);
                pv.RPC("MakeMinigunInvisible", RpcTarget.All);
            }
            else
            {
                rpgUI.SetActive(false);
                pv.RPC("MakeMinigunVisible", RpcTarget.All);
            }
        }

        
        if (!IsThisMultiplayer.Instance.multiplayer)
        {
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
        
    }

    [PunRPC]
    void MakeMinigunInvisible()
    {
        foreach (MeshRenderer mr in gunPartsToMakeInvisible)
        {
            mr.enabled = false;
        }
    }

    [PunRPC]
    void MakeMinigunVisible()
    {
        foreach (MeshRenderer mr in gunPartsToMakeInvisible)
        {
            mr.enabled = true;
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
