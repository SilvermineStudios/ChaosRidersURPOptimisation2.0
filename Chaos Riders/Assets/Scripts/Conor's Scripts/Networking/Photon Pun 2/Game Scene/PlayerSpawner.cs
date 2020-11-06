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

    //Bool to flip if spawn car or shooter
    public bool driver = true;
    public List<Transform> gunSpawnPoints = new List<Transform>();
    private bool canSpawnShooters = true;


    private void Awake()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(TimerToSpawnCars(spawnTimer));
        }
    }

    private IEnumerator TimerToSpawnCars(float time)
    {
        yield return new WaitForSeconds(time);

        SpawnCars();
    }

    public void SpawnCars()
    {
        if(PhotonNetwork.PlayerList.Length == 1) ///////////////////////////////////////////////////////////////////////////////////////////////////////1 player
        {
            photonView.RPC("RPC_SpawnCar", PhotonNetwork.PlayerList[0], GameSetup.gs.spawnPoints[0].position, GameSetup.gs.spawnPoints[0].rotation);
        }

        if (PhotonNetwork.PlayerList.Length == 2) ///////////////////////////////////////////////////////////////////////////////////////////////////////2 players
        {
            photonView.RPC("RPC_SpawnCar", PhotonNetwork.PlayerList[0], GameSetup.gs.spawnPoints[0].position, GameSetup.gs.spawnPoints[0].rotation);
        }

        if (PhotonNetwork.PlayerList.Length == 3) ///////////////////////////////////////////////////////////////////////////////////////////////////////3 players
        {
            photonView.RPC("RPC_SpawnCar", PhotonNetwork.PlayerList[0], GameSetup.gs.spawnPoints[0].position, GameSetup.gs.spawnPoints[0].rotation);
            photonView.RPC("RPC_SpawnCar", PhotonNetwork.PlayerList[2], GameSetup.gs.spawnPoints[2].position, GameSetup.gs.spawnPoints[2].rotation);
        }
    }

    private void Update()
    {
        //if there is at least 1 gun spawn point in the scene
        if (gunSpawnPoints.Count > 0 && canSpawnShooters)
        {
            if (PhotonNetwork.PlayerList.Length == 2) ///////////////////////////////////////////////////////////////////////////////////////////////////////2 players
            {
                photonView.RPC("RPC_SpawnShooter", PhotonNetwork.PlayerList[1], gunSpawnPoints[0].position, gunSpawnPoints[0].rotation, gunSpawnPoints[0]);
            }

            if (PhotonNetwork.PlayerList.Length == 3) ///////////////////////////////////////////////////////////////////////////////////////////////////////3 players
            {
                photonView.RPC("RPC_SpawnShooter", PhotonNetwork.PlayerList[1], gunSpawnPoints[0].position, gunSpawnPoints[0].rotation, gunSpawnPoints[0]);
            }

            canSpawnShooters = false;
        }
    }






    [PunRPC]
    void RPC_SpawnCar(Vector3 spawnPos, Quaternion spawnRot)
    {
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "CarAvatar"), spawnPos, spawnRot, 0);
    }


    [PunRPC]
    void RPC_SpawnShooter(Vector3 spawnPos, Quaternion spawnRot, Transform parent)
    {
        GameObject go = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Shooter"), spawnPos, spawnRot, 0);
        go.transform.SetParent(parent);
    }

    
}
