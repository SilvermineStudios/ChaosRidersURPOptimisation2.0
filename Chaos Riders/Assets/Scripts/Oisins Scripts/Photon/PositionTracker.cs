using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class PositionTracker : MonoBehaviourPun
{
    PhotonView pv;
    PlayerDataManager playerDataManager;

    string[] playerNames = new string[20];
    float[] teamNumbersToPositions = new float[20];



    List<int> teamNumbers = new List<int>();
    public string[] playerTeams;
    public Text[] playerNameTexts;
    public int[] playerScores;
    public int scoreTotal;
    public Text[] playerScoreText;
    public Transform[] scoreHolder;



    private void Awake()
    {
        pv = GetComponent<PhotonView>();
        playerDataManager = FindObjectOfType<PlayerDataManager>();
    }



    void Start()
    {
        if (!PhotonNetwork.IsMasterClient) { return; }
        foreach (PhotonMenuPlayer p in PlayerDataManager.PhotonMenuPlayers)
        {
            Debug.Log(p.teamNumber);
            if(playerNames[p.teamNumber] == null)
            {
                playerNames[p.teamNumber] = p.Player.NickName;
            }
            else
            {
                playerNames[p.teamNumber] = playerNames[p.teamNumber] + " + " + p.Player.NickName;
            }
        }
    }


    private void FixedUpdate()
    {
        if (!PhotonNetwork.IsMasterClient) { return; }
        foreach (PhotonMenuPlayer p in PlayerDataManager.PhotonMenuPlayers)
        {
            if (p.driver)
            {
                teamNumbersToPositions[p.teamNumber] = p.myCheckpoint.distanceToNextCheckpoint;
            }
        }



    }
}
