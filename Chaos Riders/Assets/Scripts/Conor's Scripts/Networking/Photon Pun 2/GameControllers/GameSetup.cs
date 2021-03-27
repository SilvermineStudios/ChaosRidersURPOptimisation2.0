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
    [SerializeField] private PlayerDataManager playerDataManager;

    //public static GameSetup GS;
    public PhotonView pv;

    [SerializeField] private bool canSpawnPlayers = true;

    [SerializeField] private float spawnDelay = 5f;
    [SerializeField] private int menuSceneIndex = 0;

    public Transform[] spawnPoints;

    private int amountOfShooters = 0, amountOfDrivers = 0;
    public GameObject[] aiCars;
    public GameObject[] aiGuns;

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
    }

    private void Start()
    {
        CalculateDriverAndShooterCount();

        Debug.Log("The Amount of players in the PlayerDataManager is: " + playerDataManager.photonMenuPlayers.Length);

        

        Debug.Log("The Amount of players in the PlayerDataManager is: " + playerDataManager.photonMenuPlayers.Length);

        if (pv.IsMine && PlayerDataManager.PhotonMenuPlayers.Length > 0)
        {
            foreach (Player p in PhotonNetwork.PlayerList)
            {
                if(p == PhotonNetwork.MasterClient)
                {
                    //Debug.Log("You Are the master and trying to spawn players");
                    SpawnPlayers();
                }
            }
        }
    }

    private void Update()
    {
        if (!IsThisMultiplayer.Instance.multiplayer) { return; }
    }

    private void CalculateDriverAndShooterCount()
    {
        foreach(PhotonMenuPlayer p in PlayerDataManager.PhotonMenuPlayers)
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
        //Debug.Log("Amount of Drivers: " + amountOfDrivers);
        //Debug.Log("Amount of Shooters: " + amountOfShooters);
    }

    public void SpawnPlayers() //happens in start
    {

        foreach (PhotonMenuPlayer p in playerDataManager.photonMenuPlayers)
        {
            if (p.driver)
            {
                //Debug.Log(p.Player.NickName + " is a Driver");
                //pv.RPC("RPC_SpawnDriver", p.Player, spawnPoints[p.teamNumber].position, spawnPoints[p.teamNumber].rotation, p);
                pv.RPC("RPC_SpawnDriver", p.Player, spawnPoints[p.teamNumber].position, spawnPoints[p.teamNumber].rotation);
                //PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "DriverPlayer"), spawnPoints[p.teamNumber].position, spawnPoints[p.teamNumber].rotation, 0);

                //if there are no shooters
                if(amountOfShooters == 0)
                {
                    pv.RPC("RPCSpawnAIGun", p.Player, spawnPoints[p.teamNumber].position, spawnPoints[p.teamNumber].rotation);
                }

                //if there are more drivers than shooters
                if (p.teamNumber > amountOfShooters - 1 && amountOfShooters != 0)
                {
                    pv.RPC("RPCSpawnAIGun", p.Player, spawnPoints[p.teamNumber].position, spawnPoints[p.teamNumber].rotation);
                }
            }

            if (p.shooter)
            {
                //Debug.Log(p.Player.NickName + " is a Shooter");
                //pv.RPC("RPC_SpawnShooter", p.Player, spawnPoints[p.teamNumber].position, spawnPoints[p.teamNumber].rotation, p);
                pv.RPC("RPC_SpawnShooter", p.Player, spawnPoints[p.teamNumber].position, spawnPoints[p.teamNumber].rotation);
                

                //if there are no drivers
                if (amountOfDrivers == 0)
                {
                    pv.RPC("RPCSpawnAICar", p.Player, spawnPoints[p.teamNumber].position, spawnPoints[p.teamNumber].rotation);
                }

                //if there are more shooters than drivers
                if (p.teamNumber > amountOfDrivers - 1 && amountOfDrivers != 0)
                {
                    pv.RPC("RPCSpawnAICar", p.Player, spawnPoints[p.teamNumber].position, spawnPoints[p.teamNumber].rotation);
                }
            }
        }
    }

    [PunRPC]
    void RPC_SpawnDriver(Vector3 spawnPos, Quaternion spawnRot)//, PhotonMenuPlayer p)
    {
        //Debug.Log(p.Player.NickName + " is trying to spawn a car at spawn position: " + spawnPos + " and at spawn rotation: " + spawnRot);
        //Debug.Log("Trying to spawn Driver for " + p.Player.NickName);

        //GameObject GO = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Driver"), spawnPos, spawnRot, 0);
        //GO.GetComponent<Controller>().currentCarClass = tempHolder.currentCarClass;

        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "DriverPlayer"), spawnPos, spawnRot, 0);
    }

    [PunRPC]
    void RPC_SpawnShooter(Vector3 spawnPos, Quaternion spawnRot)//, PhotonMenuPlayer p)
    {
        //Debug.Log(p.Player.NickName + " is trying to spawn a gun at spawn position: " + spawnPos + " and at spawn rotation: " + spawnRot);
        //Debug.Log("Trying to spawn Shooter for " + p.Player.NickName);

        //GameObject GO = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Shooter"), spawnPos, spawnRot, 0);
        //GO.GetComponent<Shooter>().minigunClass = p.currentMinigunClass;

        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "ShooterPlayer"), spawnPos, spawnRot, 0);
    }

    [PunRPC]
    void RPCSpawnAICar(Vector3 spawnPos, Quaternion spawnRot)
    {
        Quaternion spawnRotation = Quaternion.Euler(spawnRot.x, spawnRot.y - 90, spawnRot.z);

        //PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "AI", "AI Mustang"), spawnPos, spawnRotation, 0);
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "AI", aiCars[Random.Range(0, aiCars.Length)].name), spawnPos, spawnRotation, 0);
    }

    [PunRPC]
    void RPCSpawnAIGun(Vector3 spawnPos, Quaternion spawnRot)
    {
        //PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "AI", aiGuns[Random.Range(0, aiCars.Length)].name), spawnPos, spawnRot, 0);
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "AI", aiGuns[0].name), spawnPos, spawnRot, 0); //<-----------------------------------------------change back to the above line if there are more than 1 ai gun
    }
}
