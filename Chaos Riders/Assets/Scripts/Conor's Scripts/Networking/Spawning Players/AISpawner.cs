using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AISpawner : MonoBehaviour
{
    //get the amount of ai cars to spawn from Game Variables script *********
    private PhotonView pv;

    public Transform[] spawnPoints;
    public GameObject[] aiCars;

    //from the main menu settings
    [SerializeField] private bool spawnAI;
    [SerializeField] private int amountOfAI;

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

        ////////////turn these off to test in offline///////////
        spawnAI = GameVariables.ToggleAI;
        amountOfAI = GameVariables.AmountOfAICars;
        //Debug.Log("Spawn AI = " + spawnAI + " Amount of AI = " + amountOfAI);
    }

    void Start()
    {
        foreach(Player p in PhotonNetwork.PlayerList)
        {
            if (p.IsMasterClient)
            {
                for (int i = 0; i < amountOfAI; i++)
                {
                    pv.RPC("RPCSpawnAI", p, spawnPoints[i].position, spawnPoints[i].rotation);
                }
            }
        }

        /*
        if (spawnAI)
        {
            for (int i = 0; i < amountOfAI; i++)
            {
                GameObject aiCar = Instantiate(aiCars[Random.Range(0, aiCars.Length)], spawnPoints[i].position, spawnPoints[i].rotation);
            }
        }
        else
            return;
        */
    }

    [PunRPC]
    void RPCSpawnAI(Vector3 spawnPos, Quaternion spawnRot)
    {
        Quaternion spawnRotation = Quaternion.Euler(spawnRot.x, spawnRot.y - 90, spawnRot.z);

        //PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "AI", "AI Mustang"), spawnPos, spawnRotation, 0);
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "AI", aiCars[Random.Range(0, aiCars.Length)].name), spawnPos, spawnRotation, 0);
    }
}
