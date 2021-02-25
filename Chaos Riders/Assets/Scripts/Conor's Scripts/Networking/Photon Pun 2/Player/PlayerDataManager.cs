using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerDataManager : MonoBehaviour
{
    private PhotonView pv;

    [SerializeField] private PhotonMenuPlayer[] players;
    public static PhotonMenuPlayer[] Players;

    public List <GameObject> drivers = new List<GameObject>();
    public List <GameObject> shooters = new List<GameObject>();

    void Start()
    {
        
    }

    
    void Update()
    {
        UpdatePlayerList();
        AssignPlayerNumbers(); //give each player the index of the spawnpoint they will be spawned at
    }

    private void FixedUpdate()
    {
        UpdateDriverAndShooterLists();
    }

    private void UpdatePlayerList()
    {
        players = FindObjectsOfType<PhotonMenuPlayer>();
        Players = players;
    }

    private void UpdateDriverAndShooterLists()
    {
        //remove players from list when they leave the game
        if (drivers.Count != 0)
        {
            foreach (GameObject go in drivers)
            {
                if (go == null)
                {
                    drivers.Remove(go);
                }
            }
        }
        //remove players from list when they leave the game
        if (shooters.Count != 0)
        {
            foreach (GameObject go in shooters)
            {
                if (go == null)
                {
                    shooters.Remove(go);
                }
            }
        }
    }

    private void AssignPlayerNumbers()
    {
        //drivers
        if(drivers.Count != 0)
        {
            for (int i = 0; i < drivers.Count; i++)
            {
                drivers[i].GetComponent<PhotonMenuPlayer>().teamNumber = i;
            }
        }

        //shooters
        if (shooters.Count != 0)
        {
            for (int i = 0; i < shooters.Count; i++)
            {
                shooters[i].GetComponent<PhotonMenuPlayer>().teamNumber = i;
            }
        }
    }
}
