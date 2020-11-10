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

    public List<Transform> gunSpawnPoints = new List<Transform>();
    public List<GameObject> gunners = new List<GameObject>();
    public bool canSpawnShooters = true;

    private bool startSpawningGunners = false;

    public Transform[] carSpawnPoints;

    private void OnDrawGizmos()
    {
        AssignCarSpawnPointsToArray();
    }

    private void Awake()
    {
        AssignCarSpawnPointsToArray();
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
            photonView.RPC("RPC_SpawnCar", PhotonNetwork.PlayerList[0], carSpawnPoints[0].position, carSpawnPoints[0].rotation);
        }

        if (PhotonNetwork.PlayerList.Length == 3 || PhotonNetwork.PlayerList.Length == 4) ///////////////////////////////////////////////////////////3 or 4 players
        {
            photonView.RPC("RPC_SpawnCar", PhotonNetwork.PlayerList[0], carSpawnPoints[0].position, carSpawnPoints[0].rotation);
            photonView.RPC("RPC_SpawnCar", PhotonNetwork.PlayerList[2], carSpawnPoints[1].position, carSpawnPoints[1].rotation);
        }

        if (PhotonNetwork.PlayerList.Length == 5 || PhotonNetwork.PlayerList.Length == 6) ///////////////////////////////////////////////////////////5 or 6 players
        {
            photonView.RPC("RPC_SpawnCar", PhotonNetwork.PlayerList[0], carSpawnPoints[0].position, carSpawnPoints[0].rotation);
            photonView.RPC("RPC_SpawnCar", PhotonNetwork.PlayerList[2], carSpawnPoints[1].position, carSpawnPoints[1].rotation);
            photonView.RPC("RPC_SpawnCar", PhotonNetwork.PlayerList[4], carSpawnPoints[2].position, carSpawnPoints[2].rotation);
        }

        if (PhotonNetwork.PlayerList.Length == 7 || PhotonNetwork.PlayerList.Length == 8) ///////////////////////////////////////////////////////////7 or 8 players
        {
            photonView.RPC("RPC_SpawnCar", PhotonNetwork.PlayerList[0], carSpawnPoints[0].position, carSpawnPoints[0].rotation);
            photonView.RPC("RPC_SpawnCar", PhotonNetwork.PlayerList[2], carSpawnPoints[1].position, carSpawnPoints[1].rotation);
            photonView.RPC("RPC_SpawnCar", PhotonNetwork.PlayerList[4], carSpawnPoints[2].position, carSpawnPoints[2].rotation);
            photonView.RPC("RPC_SpawnCar", PhotonNetwork.PlayerList[6], carSpawnPoints[3].position, carSpawnPoints[3].rotation);
        }

        if (PhotonNetwork.PlayerList.Length == 9 || PhotonNetwork.PlayerList.Length == 10) ///////////////////////////////////////////////////////////9 or 10 players
        {
            photonView.RPC("RPC_SpawnCar", PhotonNetwork.PlayerList[0], carSpawnPoints[0].position, carSpawnPoints[0].rotation);
            photonView.RPC("RPC_SpawnCar", PhotonNetwork.PlayerList[2], carSpawnPoints[1].position, carSpawnPoints[1].rotation);
            photonView.RPC("RPC_SpawnCar", PhotonNetwork.PlayerList[4], carSpawnPoints[2].position, carSpawnPoints[2].rotation);
            photonView.RPC("RPC_SpawnCar", PhotonNetwork.PlayerList[6], carSpawnPoints[3].position, carSpawnPoints[3].rotation);
            photonView.RPC("RPC_SpawnCar", PhotonNetwork.PlayerList[8], carSpawnPoints[4].position, carSpawnPoints[4].rotation);
        }
    }
    
    
    private void Update()
    {
        if (gunners.Count > 0)
            canSpawnShooters = false;

        if (gunSpawnPoints.Count > 0)
            startSpawningGunners = true;

        
        //spawn gunners
        if (gunSpawnPoints.Count > 0 && canSpawnShooters)
        {
            canSpawnShooters = false;
            if (PhotonNetwork.PlayerList.Length == 2 || PhotonNetwork.PlayerList.Length == 3) //////////////////////////////////////////////////////2 or 3 players
            {
                photonView.RPC("RPC_SpawnShooter", PhotonNetwork.PlayerList[1], gunSpawnPoints[0].position, gunSpawnPoints[0].rotation);
            }

            if (PhotonNetwork.PlayerList.Length == 4 || PhotonNetwork.PlayerList.Length == 5) //////////////////////////////////////////////////////4 or 5 players
            {
                photonView.RPC("RPC_SpawnShooter", PhotonNetwork.PlayerList[1], gunSpawnPoints[0].position, gunSpawnPoints[0].rotation);
                photonView.RPC("RPC_SpawnShooter", PhotonNetwork.PlayerList[3], gunSpawnPoints[1].position, gunSpawnPoints[1].rotation);
            }

            if (PhotonNetwork.PlayerList.Length == 6 || PhotonNetwork.PlayerList.Length == 7) //////////////////////////////////////////////////////6 or 7 players
            {
                photonView.RPC("RPC_SpawnShooter", PhotonNetwork.PlayerList[1], gunSpawnPoints[0].position, gunSpawnPoints[0].rotation);
                photonView.RPC("RPC_SpawnShooter", PhotonNetwork.PlayerList[3], gunSpawnPoints[1].position, gunSpawnPoints[1].rotation);
                photonView.RPC("RPC_SpawnShooter", PhotonNetwork.PlayerList[5], gunSpawnPoints[2].position, gunSpawnPoints[2].rotation);
            }

            if (PhotonNetwork.PlayerList.Length == 8 || PhotonNetwork.PlayerList.Length == 9) //////////////////////////////////////////////////////6 or 7 players
            {
                photonView.RPC("RPC_SpawnShooter", PhotonNetwork.PlayerList[1], gunSpawnPoints[0].position, gunSpawnPoints[0].rotation);
                photonView.RPC("RPC_SpawnShooter", PhotonNetwork.PlayerList[3], gunSpawnPoints[1].position, gunSpawnPoints[1].rotation);
                photonView.RPC("RPC_SpawnShooter", PhotonNetwork.PlayerList[5], gunSpawnPoints[2].position, gunSpawnPoints[2].rotation);
                photonView.RPC("RPC_SpawnShooter", PhotonNetwork.PlayerList[7], gunSpawnPoints[3].position, gunSpawnPoints[3].rotation);
            }

            if (PhotonNetwork.PlayerList.Length == 10) /////////////////////////////////////////////////////////////////////////////////////////////10 players
            {
                photonView.RPC("RPC_SpawnShooter", PhotonNetwork.PlayerList[1], gunSpawnPoints[0].position, gunSpawnPoints[0].rotation);
                photonView.RPC("RPC_SpawnShooter", PhotonNetwork.PlayerList[3], gunSpawnPoints[1].position, gunSpawnPoints[1].rotation);
                photonView.RPC("RPC_SpawnShooter", PhotonNetwork.PlayerList[5], gunSpawnPoints[2].position, gunSpawnPoints[2].rotation);
                photonView.RPC("RPC_SpawnShooter", PhotonNetwork.PlayerList[7], gunSpawnPoints[3].position, gunSpawnPoints[3].rotation);
                photonView.RPC("RPC_SpawnShooter", PhotonNetwork.PlayerList[9], gunSpawnPoints[4].position, gunSpawnPoints[4].rotation);
            }
        }
    }

    void AssignCarSpawnPointsToArray()
    {
        carSpawnPoints = new Transform[this.transform.childCount]; //make the array the same length as the amount of children waypoints
        for (int i = 0; i < this.transform.childCount; i++) //put every waypoint(Child) in the array
        {
            carSpawnPoints[i] = transform.GetChild(i);
        }
    }


    [PunRPC]
    void RPC_SpawnCar(Vector3 spawnPos, Quaternion spawnRot)
    {
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "CarAvatar"), spawnPos, spawnRot, 0);
    }


    [PunRPC]
    void RPC_SpawnShooter(Vector3 spawnPos, Quaternion spawnRot)
    {
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Shooter"), spawnPos, spawnRot, 0);
    }
}
