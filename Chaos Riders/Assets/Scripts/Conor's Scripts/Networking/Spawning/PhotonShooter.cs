using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.IO;

public class PhotonShooter : MonoBehaviour
{
    public PhotonView pv;
    public GameObject myAvatar;

    private void Start()
    {
        pv = GetComponent<PhotonView>();
        int spawnPicker = Random.Range(0, PlayerSpawner.PS.carSpawnPoints.Length);
        Debug.Log("Car Spawn point = " + spawnPicker);

        if (pv.IsMine)
        {
            myAvatar = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Shooter2"),
                this.transform.position, this.transform.rotation, 0);
        }
    }

    //[PunRPC]
    //void RPC_GetTeam
}
