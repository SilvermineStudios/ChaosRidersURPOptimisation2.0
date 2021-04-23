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

        //Debug.Log(text.Length);
        //Debug.Log(teamPositions.Count);
        text[0].text = "(Driver) " + teamPositions[0].driverName;// + ", " + teamPositions[i].checkpointNumber; 
        text[1].text = "(Shooter) " + teamPositions[0].shooterName;// + ", " + teamPositions[i].checkpointNumber; 
        text[2].text = "(Driver) " + teamPositions[1].driverName;// + ", " + teamPositions[i].checkpointNumber; 
        text[3].text = "(Shooter) " + teamPositions[1].shooterName;// + ", " + teamPositions[i].checkpointNumber; 
        text[4].text = "(Driver) " + teamPositions[2].driverName;// + ", " + teamPositions[i].checkpointNumber; 
        text[5].text = "(Shooter) " + teamPositions[2].shooterName;// + ", " + teamPositions[i].checkpointNumber; 



        for (int i = 0; i < 5; i+=2)
        {
            //Debug.Log("Name: " + i.teamName +", Waypoint: " + i.checkpointNumber + ", Distance: " + i.currentPosition);
            //Debug.Log(text.Length);
            //text[i-1].text ="(Driver) " + teamPositions[i].driverName;// + ", " + teamPositions[i].checkpointNumber; 
            //text[i].text = "(Shooter) " + teamPositions[i].shooterName;// + ", " + teamPositions[i].checkpointNumber; 
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
