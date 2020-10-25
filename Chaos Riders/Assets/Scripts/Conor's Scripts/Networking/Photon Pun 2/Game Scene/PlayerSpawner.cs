using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.IO;

public class PlayerSpawner : MonoBehaviour
{
    int numberPlayers;
    [SerializeField] private int maxPlayers;

    public int spawnTimer = 5;

    public GameObject playerCar;

    private PhotonPlayer[] photonPlayers;
    public GameObject[] photonPlayersGO;

    private void Awake()
    {
        photonPlayers = new PhotonPlayer[maxPlayers];
        photonPlayersGO = new GameObject[maxPlayers];
        CheckPlayers();
    }

    private void Start()
    {
        SpawnPlayers();
    }

    void Update()
    {
        photonPlayersGO = (GameObject[])GameObject.FindGameObjectsWithTag("PhotonPlayer");
        photonPlayers = GetComponents<PhotonPlayer>();

        UpdatePlayerIndex();
    }

    void CheckPlayers()
    {
        numberPlayers = PhotonNetwork.CountOfPlayers;

        /*
        for (int i = 0; i <= numberPlayers; i++)
        {
            if (numberPlayers > maxPlayers)
            {
                numberPlayers -= maxPlayers;
            }
        }
        */
    }

    
    void SpawnPlayers()
    {
        //playerCar = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "CarAvatar"), GameSetup.gs.spawnPoints[1].position, GameSetup.gs.spawnPoints[1].rotation, 0);

        for (int i = 0; i <= PhotonNetwork.CountOfPlayers - 1; i++)
        {
            playerCar = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "CarAvatar"), GameSetup.gs.spawnPoints[i].position, GameSetup.gs.spawnPoints[i].rotation, 0);
            //playerCar.transform.SetParent(photonPlayersGO[i].transform, false);
        }
    }
    

    void UpdatePlayerIndex()
    {
        for (int i = 0; i <= numberPlayers - 1; i++)
        {
            photonPlayersGO[i].GetComponent<PhotonPlayer>().arrayIndex = i;
        }
    }

    private IEnumerator Timer(float time, int i)
    {
        Debug.Log("Timer started");

        yield return new WaitForSeconds(time);

        Debug.Log("Timer done");

        //if (pv.IsMine)
        //{
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "CarAvatar"), GameSetup.gs.spawnPoints[i].position, GameSetup.gs.spawnPoints[i].rotation, 0);
        //}
    }
}
