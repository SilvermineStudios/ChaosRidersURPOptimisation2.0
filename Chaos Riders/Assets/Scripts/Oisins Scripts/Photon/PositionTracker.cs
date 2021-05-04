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
    PhotonView Clientpv;
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
        InvokeRepeating("SortTeams", 5, 1);
    }



    void SortTeams()
    {
        doneWaiting = true;
        teamPositions.Sort();
    }


    private void FixedUpdate()
    {
        if(Clientpv == null && FindObjectOfType<myPV>() != null)
        {
            Clientpv = FindObjectOfType<myPV>().pv;
        }
        if (!PhotonNetwork.IsMasterClient) { return; }
        if(!doneWaiting) { return; }


        //Debug.Log(text.Length);
        //Debug.Log(teamPositions.Count);
        foreach(Position p in teamPositions)
        {
            if(p.pv.IsMine)
            {
                Debug.Log(234234234);
                break;
            }
        }

        for(int i = 0; i < teamPositions.Count; i ++)
        {
            if (teamPositions[i].pv == Clientpv)
            {
                //text[0].text = "(Place) " + i;
                Debug.Log("(Place) " + i);
                break;
            }
        }

        //text[2].text = "(Driver) " + teamPositions[0].driverName;// + ", " + teamPositions[i].checkpointNumber;

        // + ", " + teamPositions[i].checkpointNumber; 
        
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
        
    }
}
