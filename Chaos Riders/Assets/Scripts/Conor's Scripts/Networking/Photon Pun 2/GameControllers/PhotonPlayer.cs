using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.IO;

public class PhotonPlayer : MonoBehaviour
{
    public int arrayIndex;
    private float time;
    private PlayerSpawner playerS;

    public PhotonView pv;
    public GameObject myAvatar;

    private void Awake()
    {
        pv = GetComponent<PhotonView>();
    }

    void Start()
    {
        playerS = FindObjectOfType<PlayerSpawner>();
        time = playerS.spawnTimer;
        
        int spawnPicker = Random.Range(0, GameSetup.gs.spawnPoints.Length); //select a random spawn point out of the list of spawn points on the gamesetup script

        //Debug.Log("Photon View Name" + pv.ViewID);

        if(pv.IsMine)
        {
            //myAvatar = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "CarAvatar"), GameSetup.gs.spawnPoints[spawnPicker].position, GameSetup.gs.spawnPoints[spawnPicker].rotation, 0);
            //Debug.Log("Player: " + arrayIndex + " spawned at spawnpoint: " + arrayIndex);
            //myAvatar = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "CarAvatar"), GameSetup.gs.spawnPoints[arrayIndex].position, GameSetup.gs.spawnPoints[arrayIndex].rotation, 0);
            
        }
        //StartCoroutine(Timer(time));

        if(pv.IsMine)
        {
            CallSpawnPlayers();
        }
    }

    private IEnumerator Timer(float time)
    {
        Debug.Log("Timer started");

        yield return new WaitForSeconds(time);

        Debug.Log("Timer done");

        if (pv.IsMine)
        {
            //myAvatar = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "CarAvatar"), GameSetup.gs.spawnPoints[arrayIndex].position, GameSetup.gs.spawnPoints[arrayIndex].rotation, 0);
        }
    }

    [PunRPC]
    void SpawnPlayers()
    {
        //PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "CarAvatar"), GameSetup.gs.spawnPoints[arrayIndex].position, GameSetup.gs.spawnPoints[arrayIndex].rotation, 0);
        Debug.Log("Spawened player at: " + arrayIndex);
    }
    public void CallSpawnPlayers()
    {
        //pv.RPC("SpawnPlayers", RpcTarget.All);
    }
}
