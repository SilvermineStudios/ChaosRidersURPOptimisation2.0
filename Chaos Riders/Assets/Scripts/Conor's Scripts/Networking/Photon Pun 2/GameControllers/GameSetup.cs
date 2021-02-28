using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

//Photon Room https://www.youtube.com/watch?v=IsiWRD1Xh5g
public class GameSetup : MonoBehaviour
{
    //public static GameSetup GS;
    public PhotonView pv;

    [SerializeField] private bool canSpawnPlayers = true;

    [SerializeField] private float spawnDelay = 5f;
    [SerializeField] private int menuSceneIndex = 0;

    public Transform[] spawnPoints;

    private int amountOfShooters = 0, amountOfDrivers = 0;
    public GameObject[] aiCars;


    private void OnDrawGizmos()
    {
        spawnPoints = new Transform[this.transform.childCount]; //make the array the same length as the amount of children waypoints
        for (int i = 0; i < this.transform.childCount; i++) //put every waypoint(Child) in the array
        {
            spawnPoints[i] = transform.GetChild(i);
        }
    }

    private void Awake()
    {
        pv = GetComponent<PhotonView>();
        CalculateDriverAndShooterCount();
    }

    private void Start()
    {
        if (pv.IsMine)
        {
            RPC_SpawnPlayers();

            foreach (Player p in PhotonNetwork.PlayerList)
            {
                if(p == PhotonNetwork.MasterClient)
                {
                    //RPC_SpawnPlayers();
                }
            }
        }
        //Debug.Log("AI CAR LENGTH = " + aiCars.Length);
    }

    private void Update()
    {
        if (!IsThisMultiplayer.Instance.multiplayer) { return; }
    }

    private void CalculateDriverAndShooterCount()
    {
        foreach(PhotonMenuPlayer p in PlayerDataManager.Players)
        {
            if(p.driver)
            {
                amountOfDrivers++;
            }
            if(p.shooter)
            {
                amountOfShooters++;
            }
        }
    }

    public void RPC_SpawnPlayers() //happens in start
    {
        foreach (PhotonMenuPlayer p in PlayerDataManager.Players)
        {
            if (p.driver)
            {
                pv.RPC("RPC_SpawnDriver", p.Player, spawnPoints[p.teamNumber].position, spawnPoints[p.teamNumber].rotation);
            }
            if (p.shooter)
            {
                pv.RPC("RPC_SpawnShooter", p.Player, spawnPoints[p.teamNumber].position, spawnPoints[p.teamNumber].rotation);
            }
        }

        //spawn AI cars for shooters to attach to if there are not enough driver players
        foreach (PhotonMenuPlayer p in PlayerDataManager.Players)
        {
            if (p.shooter)
            {
                if (amountOfDrivers == 0) //needs to be instansiated here too in the case of there being only 1 shooter and no drivers
                {
                    //GameObject aiCar = Instantiate(aiCars[Random.Range(0, aiCars.Length)], spawnPoints[p.teamNumber].position, spawnPoints[p.teamNumber].rotation);
                    //pv.RPC("RPCSpawnAI", p.Player, spawnPoints[p.teamNumber].position, spawnPoints[p.teamNumber].rotation);
                    pv.RPC("RPCSpawnAI", PhotonNetwork.MasterClient, spawnPoints[p.teamNumber].position, spawnPoints[p.teamNumber].rotation);
                    //pv.RPC("RPCSpawnAI", RpcTarget.AllBuffered, spawnPoints[p.teamNumber].position, spawnPoints[p.teamNumber].rotation);
                }

                if (p.teamNumber > amountOfDrivers)
                {
                    //GameObject aiCar = Instantiate(aiCars[Random.Range(0, aiCars.Length)], spawnPoints[p.teamNumber].position, spawnPoints[p.teamNumber].rotation);
                    //pv.RPC("RPCSpawnAI", p.Player, spawnPoints[p.teamNumber].position, spawnPoints[p.teamNumber].rotation);
                    pv.RPC("RPCSpawnAI", PhotonNetwork.MasterClient, spawnPoints[p.teamNumber].position, spawnPoints[p.teamNumber].rotation);
                    //pv.RPC("RPCSpawnAI", RpcTarget.AllBuffered, spawnPoints[p.teamNumber].position, spawnPoints[p.teamNumber].rotation);
                }
                
            }
        }

    }

    [PunRPC]
    void RPC_SpawnDriver(Vector3 spawnPos, Quaternion spawnRot)
    {
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "DriverPlayer"), spawnPos, spawnRot, 0);
    }

    [PunRPC]
    void RPC_SpawnShooter(Vector3 spawnPos, Quaternion spawnRot)
    {
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "ShooterPlayer"), spawnPos, spawnRot, 0);
    }

    [PunRPC]
    void RPCSpawnAI(Vector3 spawnPos, Quaternion spawnRot)
    {
        Quaternion spawnRotation = Quaternion.Euler(spawnRot.x, spawnRot.y - 90, spawnRot.z);

        //PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "AI", "AI Mustang"), spawnPos, spawnRotation, 0);
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "AI", aiCars[Random.Range(0, aiCars.Length-1)].name), spawnPos, spawnRotation, 0);
    }
}
