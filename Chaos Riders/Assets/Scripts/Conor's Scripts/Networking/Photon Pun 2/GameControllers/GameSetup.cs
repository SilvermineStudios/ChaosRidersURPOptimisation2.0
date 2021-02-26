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
    }

    private void Start()
    {
        if (pv.IsMine)
        {
            RPC_SpawnPlayers();
        } 
    }

    private void Update()
    {
        if (!IsThisMultiplayer.Instance.multiplayer) { return; }
    }

    private void calculateDriverAndShooterCount()
    {

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
}
