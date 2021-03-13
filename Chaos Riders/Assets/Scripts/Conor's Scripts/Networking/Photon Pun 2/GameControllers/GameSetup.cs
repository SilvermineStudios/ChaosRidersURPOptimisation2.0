﻿using Photon.Pun;
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

    [SerializeField] private PlayerDataManager playerDataManager;
    [SerializeField] private PhotonMenuPlayer[] photonMenuPlayers;

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
        playerDataManager = FindObjectOfType<PlayerDataManager>();
        photonMenuPlayers = FindObjectsOfType<PhotonMenuPlayer>();
    }

    private void Start()
    {
        CalculateDriverAndShooterCount();

        //if(photonMenuPlayers.Length > 0)
            //SpawnPlayers();

        if (pv.IsMine && photonMenuPlayers.Length > 0)
        {
            //Debug.Log("You Are the master and trying to spawn players");
            //SpawnPlayers();

            foreach (Player p in PhotonNetwork.PlayerList)
            {
                if(p == PhotonNetwork.MasterClient)
                {
                    Debug.Log("You Are the master and trying to spawn players");
                    SpawnPlayers();
                }
            }
        }

        //Debug.Log("PlayerDatatManager player 1 = " + PlayerDataManager.Players[0]);

        foreach (PhotonMenuPlayer p in PlayerDataManager.PhotonMenuPlayers)
        {
            //Debug.Log("PhotonMenuPlayer p in PlayerDataManager.Players = " + p);
        }
        //Debug.Log("AI CAR LENGTH = " + aiCars.Length);
    }

    private void Update()
    {
        if (!IsThisMultiplayer.Instance.multiplayer) { return; }
    }

    private void CalculateDriverAndShooterCount()
    {
        foreach(PhotonMenuPlayer p in photonMenuPlayers)
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
        Debug.Log("Amount of Drivers: " + amountOfDrivers);
        Debug.Log("Amount of Shooters: " + amountOfShooters);
    }

    public void SpawnPlayers() //happens in start
    {
        //Debug.Log("Player Data Manager Lists Length = " + PlayerDataManager.Players.Length);
        //Debug.Log("Player Data Manager Player 1 name = " + PlayerDataManager.Players[0].name);

        foreach (PhotonMenuPlayer p in photonMenuPlayers)
        {
            //Debug.Log("Spawn");
            if (p.driver)
            {
                Debug.Log(p.Player.NickName + " = Driver");
                pv.RPC("RPC_SpawnDriver", p.Player, spawnPoints[p.teamNumber].position, spawnPoints[p.teamNumber].rotation, p);
            }

            if (p.shooter)
            {
                Debug.Log(p.Player.NickName + " = Shooter");
                pv.RPC("RPC_SpawnShooter", p.Player, spawnPoints[p.teamNumber].position, spawnPoints[p.teamNumber].rotation, p);
                

                //if there are no drivers
                if (amountOfDrivers == 0)
                {
                    pv.RPC("RPCSpawnAI", p.Player, spawnPoints[p.teamNumber].position, spawnPoints[p.teamNumber].rotation);
                }

                //if there are more shooters than drivers
                if (p.teamNumber > amountOfDrivers - 1 && amountOfDrivers != 0)
                {
                    pv.RPC("RPCSpawnAI", p.Player, spawnPoints[p.teamNumber].position, spawnPoints[p.teamNumber].rotation);
                }
            }
        }
    }

    [PunRPC]
    void RPC_SpawnDriver(Vector3 spawnPos, Quaternion spawnRot, PhotonMenuPlayer p)
    {
        Debug.Log("Trying to spawn Driver for " + p.Player.NickName);
        GameObject GO = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Driver"), spawnPos, spawnRot, 0);
        GO.GetComponent<Controller>().currentCarClass = p.currentCarClass;
    }

    [PunRPC]
    void RPC_SpawnShooter(Vector3 spawnPos, Quaternion spawnRot, PhotonMenuPlayer p)
    {
        Debug.Log("Trying to spawn Shooter for " + p.Player.NickName);
        GameObject GO = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Shooter"), spawnPos, spawnRot, 0);
        GO.GetComponent<Shooter>().minigunClass = p.currentMinigunClass;
    }

    [PunRPC]
    void RPCSpawnAI(Vector3 spawnPos, Quaternion spawnRot)
    {
        Quaternion spawnRotation = Quaternion.Euler(spawnRot.x, spawnRot.y - 90, spawnRot.z);

        //PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "AI", "AI Mustang"), spawnPos, spawnRotation, 0);
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "AI", aiCars[Random.Range(0, aiCars.Length)].name), spawnPos, spawnRotation, 0);
    }
}
