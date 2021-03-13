using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerDataManager : MonoBehaviour
{
    [SerializeField] private PhotonMenuPlayer[] photonMenuPlayers;
    public static PhotonMenuPlayer[] PhotonMenuPlayers;

    public List <GameObject> drivers = new List<GameObject>();
    public static List <GameObject> Drivers;
    public List <GameObject> shooters = new List<GameObject>();
    public static List<GameObject> Shooters;

    void Start()
    {

    }

    
    void Update()
    {
        UpdatePlayerList();

        Drivers = drivers;
        Shooters = shooters;
    }

    private void FixedUpdate()
    {
        UpdateDriverAndShooterLists();
    }

    private void UpdatePlayerList()
    {
        photonMenuPlayers = FindObjectsOfType<PhotonMenuPlayer>();
        PhotonMenuPlayers = photonMenuPlayers;
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

}
