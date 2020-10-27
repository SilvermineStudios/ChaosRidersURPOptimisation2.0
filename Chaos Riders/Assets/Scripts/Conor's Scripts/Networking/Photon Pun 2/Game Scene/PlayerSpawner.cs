using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.IO;
using UnityEngine.UI;

public class PlayerSpawner : MonoBehaviourPunCallbacks
{
    [SerializeField]private float spawnTimer = 8.5f; //have this variable match the length of time the camera animation is

    private void Awake()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(Timer(spawnTimer));
        }
    }

    private IEnumerator Timer(float time)
    {
        Debug.Log("Timer started");

        yield return new WaitForSeconds(time);

        StartGame();
    }

    public void StartGame()
    {
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            photonView.RPC("RPC_StartGame", PhotonNetwork.PlayerList[i], GameSetup.gs.spawnPoints[i].position, GameSetup.gs.spawnPoints[i].rotation);
        }
    }

    [PunRPC]
    void RPC_StartGame(Vector3 spawnPos, Quaternion spawnRot)
    {
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "CarAvatar"), spawnPos, spawnRot, 0);
    }
}
