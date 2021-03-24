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
        RemoveFromShooters();

        //drivers
        AddToDrivers();
        RemoveFromDrivers();
        

        Drivers = drivers;
        Shooters = shooters;
    }

    private void UpdatePlayerList()
    {
        photonMenuPlayers = FindObjectsOfType<PhotonMenuPlayer>();
        PhotonMenuPlayers = photonMenuPlayers;
    }

    #region Shooter Stuff
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

    private void RemoveFromShooters()
    {
        if(shooters.Count > 0)
        {
            foreach (GameObject go in shooters)
            {
                if (go == null || !go.GetComponent<PhotonMenuPlayer>().shooter)
                    shooters.Remove(go);
            }
        }
        else
            return;    
    }
    #endregion

    #region Driver Stuff
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

    private void RemoveFromDrivers()
    {
        if (drivers.Count > 0)
        {
            foreach (GameObject go in shooters)
            {
                if (go == null || !go.GetComponent<PhotonMenuPlayer>().driver)
                    drivers.Remove(go);
            }
        }
        else
            return;
    }
    #endregion

    /*
    private void UpdateDriverAndShooterLists()
    {
        //remove players from list when they leave the game and when they change f
        if (drivers.Count > 0)
        {
            foreach (GameObject go in drivers)
            {
                if (!go.GetComponent<PhotonMenuPlayer>().driver || go == null)
                {
                    drivers.Remove(go);
                }
            }
        }

        //remove players from list when they leave the game
        if (shooters.Count > 0)
        {
            foreach (GameObject go in shooters)
            {
                if (!go.GetComponent<PhotonMenuPlayer>().shooter)
                {
                    shooters.Remove(go);
                }

                //if(go == null)
                //{
                    //shooters.Remove(go);
                //}
            }
        }
    }
    */
}
