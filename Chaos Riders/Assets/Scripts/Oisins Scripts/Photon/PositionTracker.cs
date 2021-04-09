using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon;
using Photon.Realtime;
using UnityEngine.UI;

public class PositionTracker : MonoBehaviourPun
{
    PhotonView pv;
    PlayerDataManager playerDataManager;

    string[] playerNames = new string[20];


    //                                                  Team Number     Position
    float[][] teamNumbersToPositions = new float[][] { new float[20], new float[1] };



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

    float f;

    void Start()
    {
        //GetAvailableTeams()
        if (!PhotonNetwork.IsMasterClient) { return; }
        foreach (PhotonMenuPlayer p in PlayerDataManager.PhotonMenuPlayers)
        {
            {
                /*
                if(!teamNumbers.Contains(p.teamNumber))
                {
                    teamNumbers.Add(p.teamNumber);
                }
                p.driverAndShooterNames = p.Player.NickName;
                if (p.driver)
                {
                    foreach (PhotonMenuPlayer p2 in PlayerDataManager.PhotonMenuPlayers)
                    {
                        if(p2.shooter && p.teamNumber == p2.teamNumber)
                        {


                            //p.driverAndShooterNames = p.Player.NickName + " + " + p2.Player.NickName;
                            //p2.driverAndShooterNames = p.Player.NickName + " + " + p2.Player.NickName;



                        }
                    }
                }*/
            }
            if(!teamNumbers.Contains(p.teamNumber))
            {
                teamNumbers.Add(p.teamNumber);
                
            }
            //p.Player.JoinTeam((byte)p.teamNumber);
            //Debug.Log(p.teamNumber);
            f = p.teamNumber;
        }
    }


    private void FixedUpdate()
    {
        if (!PhotonNetwork.IsMasterClient) { return; }


        



        // Debug.Log(PhotonTeamsManager.Instance.TryGetTeamMembers((byte)1));

        foreach (PhotonMenuPlayer p in PlayerDataManager.PhotonMenuPlayers)
        {
            //teamNumbersToPositions[p.teamNumber][0] = p.myCheckpoint.distanceToNextCheckpoint;
        }

        foreach(int i in teamNumbers)
        {
            //Debug.Log(teamNumbersToPositions[i][0]);
        }
        


        //var sortedPlayerList = (from p in playerList orderby p.GetMyScore() descending select p).ToList();
    }
}
