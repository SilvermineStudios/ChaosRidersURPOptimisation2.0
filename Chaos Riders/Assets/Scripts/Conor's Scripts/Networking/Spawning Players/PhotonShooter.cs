using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.IO;

public class PhotonShooter : MonoBehaviour
{
    public PhotonView pv;
    public int characterValue;
    public GameObject myCharacter;

    [SerializeField] private float spawnHeightOffset;

    private void Start()
    {
        pv = GetComponent<PhotonView>();
        if (pv.IsMine) //make sure it only calls it for the person it belongs to
        {
            //myAvatar = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Golden Shooter"),this.transform.position, this.transform.rotation, 0);
            pv.RPC("RPC_AddCharacter", RpcTarget.AllBuffered, ShooterPlayerInfo.pi.mySelectedCharacter);
        }
    }

    [PunRPC]
    void RPC_AddCharacter(int whichCharacter)
    {
        Vector3 spawnPos = new Vector3(transform.position.x, transform.position.y + spawnHeightOffset, transform.position.z);

        characterValue = whichCharacter;
        myCharacter = Instantiate(ShooterPlayerInfo.pi.allCharacters[whichCharacter], spawnPos, transform.rotation, transform);
    }
}
