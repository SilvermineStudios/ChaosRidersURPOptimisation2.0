using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarSetup : MonoBehaviour
{

    private PhotonView pv;
    public int characterValue;
    public GameObject myCharacter;

    void Start()
    {
        pv = GetComponent<PhotonView>();
        if(pv.IsMine) //make sure it only calls it for the person it belongs to
        {
            pv.RPC("RPC_AddCharacter", RpcTarget.AllBuffered, PlayerInfo.pi.mySelectedCharacter);
        }
    }

    [PunRPC]
    void RPC_AddCharacter(int whichCharacter)
    {
        characterValue = whichCharacter;
        myCharacter = Instantiate(PlayerInfo.pi.allCharacters[whichCharacter], transform.position, transform.rotation, transform);
    }
}
