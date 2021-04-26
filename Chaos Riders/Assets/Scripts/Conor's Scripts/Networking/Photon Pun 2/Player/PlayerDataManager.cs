using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerDataManager : MonoBehaviour
{
    public PhotonMenuPlayer[] photonMenuPlayers;
    public static PhotonMenuPlayer[] PhotonMenuPlayers;

    public List <GameObject> drivers = new List<GameObject>();
    public static List <GameObject> Drivers;
    public List <GameObject> shooters = new List<GameObject>();
    public static List<GameObject> Shooters;

    void Awake()
    {
        DontDestroyOnLoad(this);
    }

    
    void Update()
    {
        //all players
        UpdatePlayerList();

        //shooters
        AddToShooters();

        //drivers
        AddToDrivers();
        
        Drivers = drivers;
        Shooters = shooters;

        drivers.RemoveAll(x => x == null); //remove any targets that are null (Disconnected drivers)
        Shooters.RemoveAll(x => x == null); //remove any targets that are null (Disconnected drivers)

        RemovePlayersFromList();
    }

    private void UpdatePlayerList()
    {
        photonMenuPlayers = FindObjectsOfType<PhotonMenuPlayer>();
        PhotonMenuPlayers = photonMenuPlayers;
    }

    //removes players from dirver / shooter list when they change team
    private void RemovePlayersFromList()
    {
        foreach (PhotonMenuPlayer p in photonMenuPlayers)
        {
            if (!p.shooter && shooters.Contains(p.gameObject))
            {
                shooters.Remove(p.gameObject);
            }
            if (!p.driver && drivers.Contains(p.gameObject))
            {
                drivers.Remove(p.gameObject);
            }
        }
    }

    private void AddToShooters()
    {
        if (photonMenuPlayers.Length > 0)
        {
            foreach(PhotonMenuPlayer p in photonMenuPlayers)
            {
                if(p.shooter && !shooters.Contains(p.gameObject))
                {
                    shooters.Add(p.gameObject);
                }
            }
        }
        else
            return;
    }
    private void AddToDrivers()
    {
        if (photonMenuPlayers.Length > 0)
        {
            foreach (PhotonMenuPlayer p in photonMenuPlayers)
            {
                if (p.driver && !drivers.Contains(p.gameObject))
                {
                    drivers.Add(p.gameObject);
                }
            }
        }
        else
            return;
    }
}
