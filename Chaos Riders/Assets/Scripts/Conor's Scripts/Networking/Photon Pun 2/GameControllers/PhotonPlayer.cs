using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.IO;

public class PhotonPlayer : MonoBehaviour
{
    private PhotonView pv;
    public GameObject myAvatar;

    void Start()
    {
        pv = GetComponent<PhotonView>();
        int spawnPicker = Random.Range(0, GameSetup.gs.spawnPoints.Length); //select a random spawn point out of the list of spawn points on the gamesetup script

        //Debug.Log("Photon View Name" + pv.ViewID);

        if(pv.IsMine)
        {
            //myAvatar = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerAvatar"), GameSetup.gs.spawnPoints[spawnPicker].position, GameSetup.gs.spawnPoints[spawnPicker].rotation, 0);
            //myAvatar = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "CarAvatar"), GameSetup.gs.spawnPoints[spawnPicker].position, GameSetup.gs.spawnPoints[spawnPicker].rotation, 0);
        }
    }

    
    void Update()
    {
        
    }
}
