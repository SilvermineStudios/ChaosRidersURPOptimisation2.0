using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.IO;
using UnityEngine.UI;

public class PlayerSpawner : MonoBehaviourPunCallbacks
{
    public Button startButton;
    int numberPlayers;
    [SerializeField] private int maxPlayers;

    public int spawnTimer = 5;

    public GameObject playerCar;

    private PhotonPlayer[] photonPlayers;
    public GameObject[] photonPlayersGO;
    private PhotonView pv;

    private void Awake()
    {
        photonPlayers = new PhotonPlayer[maxPlayers];
        photonPlayersGO = new GameObject[maxPlayers];
        pv = GetComponent<PhotonView>();
        CheckPlayers();
        Button();
    }

    private void Start()
    {
        //SpawnPlayers();

        startButton.onClick.AddListener(StartGame);
        //startButton.interactable = false;
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
        for (int i = 0; i <= PhotonNetwork.CountOfPlayers - 1; i++)
        {
            //playerCar = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "CarAvatar"), GameSetup.gs.spawnPoints[i].position, GameSetup.gs.spawnPoints[i].rotation, 0);
        }
    }
    

    void UpdatePlayerIndex()
    {
        for (int i = 0; i <= numberPlayers - 1; i++)
        {
            //photonPlayersGO[i].GetComponent<PhotonPlayer>().arrayIndex = i;
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

    void StartGame()
    {
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            photonView.RPC("RPC_StartGame", PhotonNetwork.PlayerList[i], GameSetup.gs.spawnPoints[i].position, GameSetup.gs.spawnPoints[i].rotation);
        }
        /*
        for (int i = 0; i < photonPlayers.Length; i++)
        {
            photonView.RPC("RPC_StartGame", PhotonNetwork.PlayerList[i], GameSetup.gs.spawnPoints[i].position, GameSetup.gs.spawnPoints[i].rotation);
        }
        */
    }

    [PunRPC]
    void RPC_StartGame(Vector3 spawnPos, Quaternion spawnRot)
    {
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "CarAvatar"), spawnPos, spawnRot, 0);
    }

    private void Button()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            startButton.interactable = true;
            //startButton.GetComponent<GameObject>().SetActive(true);
        }
        else
        {
            startButton.interactable = false;
            //startButton.GetComponent<GameObject>().SetActive(false);
        }
    }
}
