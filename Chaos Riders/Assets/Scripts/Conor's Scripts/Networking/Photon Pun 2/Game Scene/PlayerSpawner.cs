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

    private void Awake()
    {
        CheckPlayers();
        SpawnPlayers();
    }

    void CheckPlayers()
    {
        numberPlayers = PhotonNetwork.CountOfPlayers;

        for (int i = 0; i <= numberPlayers; i++)
        {
            if (numberPlayers > maxPlayers)
            {
                numberPlayers -= maxPlayers;
            }
        }
    }

    void SpawnPlayers()
    {
        for(int i = 0; i <= numberPlayers; i++)
        {
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "CarAvatar"), GameSetup.gs.spawnPoints[i].position, GameSetup.gs.spawnPoints[i].rotation, 0);
        }
    }
}
