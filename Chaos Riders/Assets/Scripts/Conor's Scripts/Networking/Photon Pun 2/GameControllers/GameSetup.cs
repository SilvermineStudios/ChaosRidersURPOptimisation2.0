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
    public static GameSetup GS;
    public PhotonView pv;

    [SerializeField] private bool canSpawnPlayers = true;

    [SerializeField] private float spawnDelay = 5f;
    [SerializeField] private int menuSceneIndex = 0;

    //public Player[] players = PhotonNetwork.PlayerList;
    //public PhotonPlayer[] photonPlayers;

    public int nextPlayersTeam = 1; //team 1 = drivers, team 2 = shooters
    public Transform[] spawnPoints;
    public static Transform[] SpawnPoints;

    public GameObject BrakerPrefab, ShreddedPrefab, ColtPrefab;
    public GameObject StandardGunPrefab, GoldenGunPrefab;


    private void OnDrawGizmos()
    {
        spawnPoints = new Transform[this.transform.childCount]; //make the array the same length as the amount of children waypoints
        for (int i = 0; i < this.transform.childCount; i++) //put every waypoint(Child) in the array
        {
            spawnPoints[i] = transform.GetChild(i);
        }
    }

    /*
    private void OnEnable()
    {
        if(GameSetup.GS == null)
        {
            GameSetup.GS = this;
        }
    }
    */

    private void Awake()
    {
        SpawnPoints = spawnPoints;
    }

    private void Start()
    {
        Debug.Log("Test");

        pv = GetComponent<PhotonView>();

        if (pv.IsMine)
        {
            //pv.RPC("RPC_SpawnPlayers", RpcTarget.MasterClient); 
            RPC_SpawnPlayers();
        } 
    }

    private void Update()
    {
        if (!IsThisMultiplayer.Instance.multiplayer) { return; }
    }

    //[PunRPC]
    public void RPC_SpawnPlayers()
    {
        if(!canSpawnPlayers)//used for testing
        {
            foreach(PhotonMenuPlayer p in PlayerDataManager.Players)
            {
                if(p.driver)
                {
                    pv.RPC("RPC_SpawnDriver", p.Player, spawnPoints[0].position, spawnPoints[0].rotation);
                    /*
                    //Debug.Log("Player is a driver and has chosen the " + p.carModel + " as their car");
                    
                    //Braker
                    //if (p.carModel == PhotonMenuPlayer.carType.Braker)
                        pv.RPC("RPC_SpawnPlayer", p.Player, BrakerPrefab, spawnPoints[p.teamNumber].position, spawnPoints[p.teamNumber].rotation);

                    //Shredder
                    if (p.carModel == PhotonMenuPlayer.carType.Shredder)
                        pv.RPC("RPC_SpawnPlayer", p.Player, ShreddedPrefab, spawnPoints[p.teamNumber].position, spawnPoints[p.teamNumber].rotation);

                    //Colt
                    if (p.carModel == PhotonMenuPlayer.carType.Colt)
                        pv.RPC("RPC_SpawnPlayer", p.Player, ColtPrefab, spawnPoints[p.teamNumber].position, spawnPoints[p.teamNumber].rotation);
                    */
                }
                if(p.shooter)
                {
                    pv.RPC("RPC_SpawnShooter", p.Player, spawnPoints[0].position, spawnPoints[0].rotation);
                    /*
                    //Debug.Log("Player is a shooter and has chosen the " + p.shooterModel + " as their gun");

                    //standard Gun
                    if (p.shooterModel == PhotonMenuPlayer.shooterType.standardGun)
                        pv.RPC("RPC_SpawnPlayer", p.Player, StandardGunPrefab, spawnPoints[p.teamNumber].position, spawnPoints[p.teamNumber].rotation);

                    //Golden Gun
                    if (p.shooterModel == PhotonMenuPlayer.shooterType.goldenGun)
                        pv.RPC("RPC_SpawnPlayer", p.Player, GoldenGunPrefab, spawnPoints[p.teamNumber].position, spawnPoints[p.teamNumber].rotation);
                    */
                }
            }
        }
        


        if (canSpawnPlayers)
        {
            if (PhotonNetwork.PlayerList.Length == 1)
            {
                //car 1
                pv.RPC("RPC_SpawnDriver", PhotonNetwork.PlayerList[0], spawnPoints[0].position, spawnPoints[0].rotation);
            }

            if (PhotonNetwork.PlayerList.Length == 2)
            {
                //car 1
                pv.RPC("RPC_SpawnDriver", PhotonNetwork.PlayerList[0], spawnPoints[0].position, spawnPoints[0].rotation);
                pv.RPC("RPC_SpawnShooter", PhotonNetwork.PlayerList[1], spawnPoints[0].position, spawnPoints[0].rotation);
            }

            if (PhotonNetwork.PlayerList.Length == 3)
            {
                //car 1
                pv.RPC("RPC_SpawnDriver", PhotonNetwork.PlayerList[0], spawnPoints[0].position, spawnPoints[0].rotation);
                pv.RPC("RPC_SpawnShooter", PhotonNetwork.PlayerList[1], spawnPoints[0].position, spawnPoints[0].rotation);
                //car 2
                pv.RPC("RPC_SpawnDriver", PhotonNetwork.PlayerList[2], spawnPoints[1].position, spawnPoints[1].rotation);
            }

            if (PhotonNetwork.PlayerList.Length == 4)
            {
                //car 1
                pv.RPC("RPC_SpawnDriver", PhotonNetwork.PlayerList[0], spawnPoints[0].position, spawnPoints[0].rotation);
                pv.RPC("RPC_SpawnShooter", PhotonNetwork.PlayerList[1], spawnPoints[0].position, spawnPoints[0].rotation);
                //car 2
                pv.RPC("RPC_SpawnDriver", PhotonNetwork.PlayerList[2], spawnPoints[1].position, spawnPoints[1].rotation);
                pv.RPC("RPC_SpawnShooter", PhotonNetwork.PlayerList[3], spawnPoints[1].position, spawnPoints[1].rotation);
            }

            if (PhotonNetwork.PlayerList.Length == 5)
            {
                //car 1
                pv.RPC("RPC_SpawnDriver", PhotonNetwork.PlayerList[0], spawnPoints[0].position, spawnPoints[0].rotation);
                pv.RPC("RPC_SpawnShooter", PhotonNetwork.PlayerList[1], spawnPoints[0].position, spawnPoints[0].rotation);
                //car 2
                pv.RPC("RPC_SpawnDriver", PhotonNetwork.PlayerList[2], spawnPoints[1].position, spawnPoints[1].rotation);
                pv.RPC("RPC_SpawnShooter", PhotonNetwork.PlayerList[3], spawnPoints[1].position, spawnPoints[1].rotation);
                //car 3
                pv.RPC("RPC_SpawnDriver", PhotonNetwork.PlayerList[4], spawnPoints[2].position, spawnPoints[2].rotation);
            }

            if (PhotonNetwork.PlayerList.Length == 6)
            {
                //car 1
                pv.RPC("RPC_SpawnDriver", PhotonNetwork.PlayerList[0], spawnPoints[0].position, spawnPoints[0].rotation);
                pv.RPC("RPC_SpawnShooter", PhotonNetwork.PlayerList[1], spawnPoints[0].position, spawnPoints[0].rotation);
                //car 2
                pv.RPC("RPC_SpawnDriver", PhotonNetwork.PlayerList[2], spawnPoints[1].position, spawnPoints[1].rotation);
                pv.RPC("RPC_SpawnShooter", PhotonNetwork.PlayerList[3], spawnPoints[1].position, spawnPoints[1].rotation);
                //car 3
                pv.RPC("RPC_SpawnDriver", PhotonNetwork.PlayerList[4], spawnPoints[2].position, spawnPoints[2].rotation);
                pv.RPC("RPC_SpawnShooter", PhotonNetwork.PlayerList[5], spawnPoints[2].position, spawnPoints[2].rotation);
            }

            if (PhotonNetwork.PlayerList.Length == 7)
            {
                //car 1
                pv.RPC("RPC_SpawnDriver", PhotonNetwork.PlayerList[0], spawnPoints[0].position, spawnPoints[0].rotation);
                pv.RPC("RPC_SpawnShooter", PhotonNetwork.PlayerList[1], spawnPoints[0].position, spawnPoints[0].rotation);
                //car 2
                pv.RPC("RPC_SpawnDriver", PhotonNetwork.PlayerList[2], spawnPoints[1].position, spawnPoints[1].rotation);
                pv.RPC("RPC_SpawnShooter", PhotonNetwork.PlayerList[3], spawnPoints[1].position, spawnPoints[1].rotation);
                //car 3
                pv.RPC("RPC_SpawnDriver", PhotonNetwork.PlayerList[4], spawnPoints[2].position, spawnPoints[2].rotation);
                pv.RPC("RPC_SpawnShooter", PhotonNetwork.PlayerList[5], spawnPoints[2].position, spawnPoints[2].rotation);
                //car 4
                pv.RPC("RPC_SpawnDriver", PhotonNetwork.PlayerList[6], spawnPoints[3].position, spawnPoints[3].rotation);
            }

            if (PhotonNetwork.PlayerList.Length == 8)
            {
                //car 1
                pv.RPC("RPC_SpawnDriver", PhotonNetwork.PlayerList[0], spawnPoints[0].position, spawnPoints[0].rotation);
                pv.RPC("RPC_SpawnShooter", PhotonNetwork.PlayerList[1], spawnPoints[0].position, spawnPoints[0].rotation);
                //car 2
                pv.RPC("RPC_SpawnDriver", PhotonNetwork.PlayerList[2], spawnPoints[1].position, spawnPoints[1].rotation);
                pv.RPC("RPC_SpawnShooter", PhotonNetwork.PlayerList[3], spawnPoints[1].position, spawnPoints[1].rotation);
                //car 3
                pv.RPC("RPC_SpawnDriver", PhotonNetwork.PlayerList[4], spawnPoints[2].position, spawnPoints[2].rotation);
                pv.RPC("RPC_SpawnShooter", PhotonNetwork.PlayerList[5], spawnPoints[2].position, spawnPoints[2].rotation);
                //car 4
                pv.RPC("RPC_SpawnDriver", PhotonNetwork.PlayerList[6], spawnPoints[3].position, spawnPoints[3].rotation);
                pv.RPC("RPC_SpawnShooter", PhotonNetwork.PlayerList[7], spawnPoints[3].position, spawnPoints[3].rotation);
            }

            if (PhotonNetwork.PlayerList.Length == 9)
            {
                //car 1
                pv.RPC("RPC_SpawnDriver", PhotonNetwork.PlayerList[0], spawnPoints[0].position, spawnPoints[0].rotation);
                pv.RPC("RPC_SpawnShooter", PhotonNetwork.PlayerList[1], spawnPoints[0].position, spawnPoints[0].rotation);
                //car 2
                pv.RPC("RPC_SpawnDriver", PhotonNetwork.PlayerList[2], spawnPoints[1].position, spawnPoints[1].rotation);
                pv.RPC("RPC_SpawnShooter", PhotonNetwork.PlayerList[3], spawnPoints[1].position, spawnPoints[1].rotation);
                //car 3
                pv.RPC("RPC_SpawnDriver", PhotonNetwork.PlayerList[4], spawnPoints[2].position, spawnPoints[2].rotation);
                pv.RPC("RPC_SpawnShooter", PhotonNetwork.PlayerList[5], spawnPoints[2].position, spawnPoints[2].rotation);
                //car 4
                pv.RPC("RPC_SpawnDriver", PhotonNetwork.PlayerList[6], spawnPoints[3].position, spawnPoints[3].rotation);
                pv.RPC("RPC_SpawnShooter", PhotonNetwork.PlayerList[7], spawnPoints[3].position, spawnPoints[3].rotation);
                //car 5
                pv.RPC("RPC_SpawnDriver", PhotonNetwork.PlayerList[8], spawnPoints[4].position, spawnPoints[4].rotation);
            }

            if (PhotonNetwork.PlayerList.Length == 10)
            {
                //car 1
                pv.RPC("RPC_SpawnDriver", PhotonNetwork.PlayerList[0], spawnPoints[0].position, spawnPoints[0].rotation);
                pv.RPC("RPC_SpawnShooter", PhotonNetwork.PlayerList[1], spawnPoints[0].position, spawnPoints[0].rotation);
                //car 2
                pv.RPC("RPC_SpawnDriver", PhotonNetwork.PlayerList[2], spawnPoints[1].position, spawnPoints[1].rotation);
                pv.RPC("RPC_SpawnShooter", PhotonNetwork.PlayerList[3], spawnPoints[1].position, spawnPoints[1].rotation);
                //car 3
                pv.RPC("RPC_SpawnDriver", PhotonNetwork.PlayerList[4], spawnPoints[2].position, spawnPoints[2].rotation);
                pv.RPC("RPC_SpawnShooter", PhotonNetwork.PlayerList[5], spawnPoints[2].position, spawnPoints[2].rotation);
                //car 4
                pv.RPC("RPC_SpawnDriver", PhotonNetwork.PlayerList[6], spawnPoints[3].position, spawnPoints[3].rotation);
                pv.RPC("RPC_SpawnShooter", PhotonNetwork.PlayerList[7], spawnPoints[3].position, spawnPoints[3].rotation);
                //car 5
                pv.RPC("RPC_SpawnDriver", PhotonNetwork.PlayerList[8], spawnPoints[4].position, spawnPoints[4].rotation);
                pv.RPC("RPC_SpawnShooter", PhotonNetwork.PlayerList[9], spawnPoints[4].position, spawnPoints[4].rotation);
            }
        }
    }

    public void UpdateTeam()
    {
        //alternate the teams
        if(nextPlayersTeam == 1)
            nextPlayersTeam = 2;
        else
            nextPlayersTeam = 1;
    }

    //[PunRPC]
    //void RPC_SpawnPlayer(Vector3 spawnPos, Quaternion spawnRot)
    //{
        //PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PhotonPlayer"), spawnPos, spawnRot, 0);
    //}

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
    void RPC_SpawnPlayer(GameObject playerAvatar, Vector3 spawnPos, Quaternion spawnRot)
    {
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", playerAvatar.name), spawnPos, spawnRot, 0);
    }
}
