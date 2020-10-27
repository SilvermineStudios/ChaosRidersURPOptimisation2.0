using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameSetup : MonoBehaviour
{
    [SerializeField] GameObject mainCam;
    [SerializeField] bool mainCamera = true;

    public static GameSetup gs;
    public Transform[] spawnPoints;
    public Player[] players = PhotonNetwork.PlayerList;
    public PhotonPlayer[] photonPlayers;

    private void OnEnable()
    {
        if(GameSetup.gs == null)
        {
            GameSetup.gs = this;
        }
    }

    private void Awake()
    {
        if (mainCamera)
            mainCam.SetActive(true);
        else
            mainCam.SetActive(false);
    }
}
