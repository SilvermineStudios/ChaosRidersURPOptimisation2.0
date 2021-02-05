using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.IO;

public class PhotonDriver : MonoBehaviour
{
    public PhotonView pv;
    public int characterValue;
    public GameObject myCharacter;

    private void Start()
    {
        pv = GetComponent<PhotonView>();
        if (pv.IsMine) //make sure it only calls it for the person it belongs to
        {
            //myAvatar = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Golden Shooter"),this.transform.position, this.transform.rotation, 0);
            pv.RPC("RPC_AddCharacter", RpcTarget.AllBuffered, DriverPlayerInfo.pi.mySelectedCharacter);
        }
    }

    [PunRPC]
    void RPC_AddCharacter(int whichCharacter)
    {
        characterValue = whichCharacter;
        myCharacter = Instantiate(DriverPlayerInfo.pi.allCharacters[whichCharacter], transform.position, transform.rotation, transform);
    }
}
