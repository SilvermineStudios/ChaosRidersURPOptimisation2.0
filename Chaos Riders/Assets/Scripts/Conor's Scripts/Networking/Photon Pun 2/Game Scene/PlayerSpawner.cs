using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.IO;
using UnityEngine.UI;

public class PlayerSpawner : MonoBehaviourPunCallbacks
{
    private Button startButton;
    public GameObject startButtonGO;

    public int spawnTimer = 5;

    public GameObject playerCar;

    private void Awake()
    {
        startButton = startButtonGO.GetComponent<Button>();
        Button();
    }

    private void Start()
    {
        startButton.onClick.AddListener(StartGame);
    }


    private IEnumerator Timer(float time, int i)
    {
        Debug.Log("Timer started");

        yield return new WaitForSeconds(time);

        //StartGame();
    }

    void StartGame()
    {
        startButtonGO.SetActive(false);

        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            photonView.RPC("RPC_StartGame", PhotonNetwork.PlayerList[i], GameSetup.gs.spawnPoints[i].position, GameSetup.gs.spawnPoints[i].rotation);
        }
    }

    [PunRPC]
    void RPC_StartGame(Vector3 spawnPos, Quaternion spawnRot)
    {
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "CarAvatar"), spawnPos, spawnRot, 0);
    }

    private void Button()
    {
        if (PhotonNetwork.IsMasterClient)
            startButtonGO.SetActive(true);
        else
            startButtonGO.SetActive(false);
    }
}
