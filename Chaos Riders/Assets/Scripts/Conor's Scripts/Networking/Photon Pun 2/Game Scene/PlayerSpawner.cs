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
    public List<GameObject> gunners = new List<GameObject>();
    public bool canSpawnShooters = true;

    private bool startSpawningGunners = false;


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
        if(PhotonNetwork.PlayerList.Length == 1 || PhotonNetwork.PlayerList.Length == 2) ////////////////////////////////////////////////////////////1 or 2 players
        {
            photonView.RPC("RPC_SpawnCar", PhotonNetwork.PlayerList[0], GameSetup.gs.spawnPoints[0].position, GameSetup.gs.spawnPoints[0].rotation);
        }

        if (PhotonNetwork.PlayerList.Length == 3 || PhotonNetwork.PlayerList.Length == 4) ///////////////////////////////////////////////////////////3 or 4 players
        {
            photonView.RPC("RPC_SpawnCar", PhotonNetwork.PlayerList[0], GameSetup.gs.spawnPoints[0].position, GameSetup.gs.spawnPoints[0].rotation);
            photonView.RPC("RPC_SpawnCar", PhotonNetwork.PlayerList[2], GameSetup.gs.spawnPoints[1].position, GameSetup.gs.spawnPoints[1].rotation);
        }

        if (PhotonNetwork.PlayerList.Length == 5 || PhotonNetwork.PlayerList.Length == 6) ///////////////////////////////////////////////////////////5 or 6 players
        {
            photonView.RPC("RPC_SpawnCar", PhotonNetwork.PlayerList[0], GameSetup.gs.spawnPoints[0].position, GameSetup.gs.spawnPoints[0].rotation);
            photonView.RPC("RPC_SpawnCar", PhotonNetwork.PlayerList[2], GameSetup.gs.spawnPoints[1].position, GameSetup.gs.spawnPoints[1].rotation);
            photonView.RPC("RPC_SpawnCar", PhotonNetwork.PlayerList[4], GameSetup.gs.spawnPoints[2].position, GameSetup.gs.spawnPoints[2].rotation);
        }

        if (PhotonNetwork.PlayerList.Length == 7 || PhotonNetwork.PlayerList.Length == 8) ///////////////////////////////////////////////////////////7 or 8 players
        {
            photonView.RPC("RPC_SpawnCar", PhotonNetwork.PlayerList[0], GameSetup.gs.spawnPoints[0].position, GameSetup.gs.spawnPoints[0].rotation);
            photonView.RPC("RPC_SpawnCar", PhotonNetwork.PlayerList[2], GameSetup.gs.spawnPoints[1].position, GameSetup.gs.spawnPoints[1].rotation);
            photonView.RPC("RPC_SpawnCar", PhotonNetwork.PlayerList[4], GameSetup.gs.spawnPoints[2].position, GameSetup.gs.spawnPoints[2].rotation);
            photonView.RPC("RPC_SpawnCar", PhotonNetwork.PlayerList[6], GameSetup.gs.spawnPoints[3].position, GameSetup.gs.spawnPoints[3].rotation);
        }

        if (PhotonNetwork.PlayerList.Length == 9 || PhotonNetwork.PlayerList.Length == 10) ///////////////////////////////////////////////////////////9 or 10 players
        {
            photonView.RPC("RPC_SpawnCar", PhotonNetwork.PlayerList[0], GameSetup.gs.spawnPoints[0].position, GameSetup.gs.spawnPoints[0].rotation);
            photonView.RPC("RPC_SpawnCar", PhotonNetwork.PlayerList[2], GameSetup.gs.spawnPoints[1].position, GameSetup.gs.spawnPoints[1].rotation);
            photonView.RPC("RPC_SpawnCar", PhotonNetwork.PlayerList[4], GameSetup.gs.spawnPoints[2].position, GameSetup.gs.spawnPoints[2].rotation);
            photonView.RPC("RPC_SpawnCar", PhotonNetwork.PlayerList[6], GameSetup.gs.spawnPoints[3].position, GameSetup.gs.spawnPoints[3].rotation);
            photonView.RPC("RPC_SpawnCar", PhotonNetwork.PlayerList[8], GameSetup.gs.spawnPoints[4].position, GameSetup.gs.spawnPoints[4].rotation);
        }
    }

    private void Update()
    {
        if (gunners.Count > 0)
            canSpawnShooters = false;

        if (gunSpawnPoints.Count > 0)
            startSpawningGunners = true;

        //if there is at least 1 gun spawn point in the scene
        if (gunSpawnPoints.Count > 0 && canSpawnShooters)
        {
            canSpawnShooters = false;
            if (PhotonNetwork.PlayerList.Length == 2 || PhotonNetwork.PlayerList.Length == 3) //////////////////////////////////////////////////////2 or 3 players
            {
                photonView.RPC("RPC_SpawnShooter", PhotonNetwork.PlayerList[1], gunSpawnPoints[0].position, gunSpawnPoints[0].rotation, gunSpawnPoints[0]);
            }

            if (PhotonNetwork.PlayerList.Length == 4 || PhotonNetwork.PlayerList.Length == 5) //////////////////////////////////////////////////////4 or 5 players
            {
                photonView.RPC("RPC_SpawnShooter", PhotonNetwork.PlayerList[1], gunSpawnPoints[0].position, gunSpawnPoints[0].rotation, gunSpawnPoints[0]);
                photonView.RPC("RPC_SpawnShooter", PhotonNetwork.PlayerList[3], gunSpawnPoints[1].position, gunSpawnPoints[1].rotation, gunSpawnPoints[1]);
            }

            if (PhotonNetwork.PlayerList.Length == 6 || PhotonNetwork.PlayerList.Length == 7) //////////////////////////////////////////////////////6 or 7 players
            {
                photonView.RPC("RPC_SpawnShooter", PhotonNetwork.PlayerList[1], gunSpawnPoints[0].position, gunSpawnPoints[0].rotation, gunSpawnPoints[0]);
                photonView.RPC("RPC_SpawnShooter", PhotonNetwork.PlayerList[3], gunSpawnPoints[1].position, gunSpawnPoints[1].rotation, gunSpawnPoints[1]);
                photonView.RPC("RPC_SpawnShooter", PhotonNetwork.PlayerList[5], gunSpawnPoints[2].position, gunSpawnPoints[2].rotation, gunSpawnPoints[2]);
            }

            if (PhotonNetwork.PlayerList.Length == 8 || PhotonNetwork.PlayerList.Length == 9) //////////////////////////////////////////////////////6 or 7 players
            {
                photonView.RPC("RPC_SpawnShooter", PhotonNetwork.PlayerList[1], gunSpawnPoints[0].position, gunSpawnPoints[0].rotation, gunSpawnPoints[0]);
                photonView.RPC("RPC_SpawnShooter", PhotonNetwork.PlayerList[3], gunSpawnPoints[1].position, gunSpawnPoints[1].rotation, gunSpawnPoints[1]);
                photonView.RPC("RPC_SpawnShooter", PhotonNetwork.PlayerList[5], gunSpawnPoints[2].position, gunSpawnPoints[2].rotation, gunSpawnPoints[2]);
                photonView.RPC("RPC_SpawnShooter", PhotonNetwork.PlayerList[7], gunSpawnPoints[3].position, gunSpawnPoints[3].rotation, gunSpawnPoints[3]);
            }

            if (PhotonNetwork.PlayerList.Length == 10) /////////////////////////////////////////////////////////////////////////////////////////////10 players
            {
                photonView.RPC("RPC_SpawnShooter", PhotonNetwork.PlayerList[1], gunSpawnPoints[0].position, gunSpawnPoints[0].rotation, gunSpawnPoints[0]);
                photonView.RPC("RPC_SpawnShooter", PhotonNetwork.PlayerList[3], gunSpawnPoints[1].position, gunSpawnPoints[1].rotation, gunSpawnPoints[1]);
                photonView.RPC("RPC_SpawnShooter", PhotonNetwork.PlayerList[5], gunSpawnPoints[2].position, gunSpawnPoints[2].rotation, gunSpawnPoints[2]);
                photonView.RPC("RPC_SpawnShooter", PhotonNetwork.PlayerList[7], gunSpawnPoints[3].position, gunSpawnPoints[3].rotation, gunSpawnPoints[3]);
                photonView.RPC("RPC_SpawnShooter", PhotonNetwork.PlayerList[9], gunSpawnPoints[4].position, gunSpawnPoints[4].rotation, gunSpawnPoints[4]);
            }
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
        //go.transform.SetParent(parent);
    }

    
}
