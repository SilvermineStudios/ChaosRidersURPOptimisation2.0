using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon;
using Photon.Realtime;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    /*
    [SerializeField]
    Camera standbyCamera;
    [SerializeField]
    private Transform[] spawnPoint;
    [SerializeField]
    private GameObject ConnectButtonGO;
    [SerializeField]
    private Text ConnectingText;

    public GameObject Sticks;
    const string VERSION = "v0.0.1";
    public string roomName = "Maze";
    public string playerPrefabName = "Player";
    int positionNumber;
    int newPos;
    bool StartGame = false;
    int numberPlayers;



    void Update()
    {
        if (StartGame)
        {
            CheckPlayers();

            //Determine which spawn point to use based on the number of player
            if (numberPlayers == 1)
            {
                PhotonNetwork.Instantiate("Player", spawnPoint[0].position, spawnPoint[0].rotation, 0);
                numberPlayers = 2;
                ConnectButton.Connect = false;
            }

            else if (numberPlayers == 2)
            {
                PhotonNetwork.Instantiate("Player", spawnPoint[1].position, spawnPoint[1].rotation, 0);
                numberPlayers = 3;
                ConnectButton.Connect = false;
            }
            else if (numberPlayers == 3)
            {
                PhotonNetwork.Instantiate("Player", spawnPoint[2].position, spawnPoint[2].rotation, 0);
                numberPlayers = 4;
                ConnectButton.Connect = false;
            }
            else if (numberPlayers == 4)
            {
                PhotonNetwork.Instantiate("Player", spawnPoint[3].position, spawnPoint[3].rotation, 0);
                numberPlayers = 1;
                ConnectButton.Connect = false;
            }
        }
    }

    void CheckPlayers()
    {
        numberPlayers = PhotonNetwork.countOfPlayers;
        //if the number of player is heigher than the number of spawnpoint in the game (in this case 4),
        //spawn the players in round order
        for (int i = 0; i <= numberPlayers; i++)
        {
            if (numberPlayers > 4)
            {
                numberPlayers -= 4;
            }

        }
    }
    */
}
