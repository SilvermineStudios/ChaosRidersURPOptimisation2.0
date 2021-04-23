using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;


public class PositionTracker : MonoBehaviourPun
{
    PhotonView pv;
    PlayerDataManager playerDataManager;

    CarPositionHolder[] carPositionHolders;
    List<Position> teamPositions = new List<Position>();
    bool doneWaiting;
    [SerializeField] TextMeshProUGUI[] text;

    private void Awake()
    {
        pv = GetComponent<PhotonView>();
        playerDataManager = FindObjectOfType<PlayerDataManager>();
    }

    void Start()
    {
        if (!PhotonNetwork.IsMasterClient) { return; }
        StartCoroutine(WaitForSpawns());
    }


    private void FixedUpdate()
    {
        if (!PhotonNetwork.IsMasterClient) { return; }
        if(!doneWaiting) { return; }
        teamPositions.Sort();
        for(int i = 0; i < 3; i++)
        {
            //Debug.Log("Name: " + i.teamName +", Waypoint: " + i.checkpointNumber + ", Distance: " + i.currentPosition);
            //Debug.Log(i);
            text[i].text = teamPositions[i].teamName + ", " + teamPositions[i].checkpointNumber; 
        }
    }

    IEnumerator WaitForSpawns()
    {
        yield return new WaitForSeconds(2);
        carPositionHolders = FindObjectsOfType<CarPositionHolder>();
        foreach (CarPositionHolder c in carPositionHolders)
        {
            teamPositions.Add(c.myPosition);
        }
        doneWaiting = true;
    }
}
