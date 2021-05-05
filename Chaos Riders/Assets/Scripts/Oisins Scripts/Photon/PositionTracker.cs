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

    Dictionary<int, string> places = new Dictionary<int, string>()
    {
        {1, "1st" },
        {2, "2nd" },
        {3, "3rd" },
        {4, "4th" },
        {5, "5th" },
        {6, "6th" },
        {7, "7th" },
        {8, "8th" },
        {9, "9th" },
        {10, "10th" },
    };

    CarPositionHolder[] carPositionHolders;
    List<Position> teamPositions = new List<Position>();
    bool doneWaiting;
    [SerializeField] TextMeshProUGUI[] text;

    [SerializeField] TextMeshProUGUI myPositionText;


    private void Awake()
    {
        pv = GetComponent<PhotonView>();
        playerDataManager = FindObjectOfType<PlayerDataManager>();
    }

    void Start()
    {
        StartCoroutine(WaitForSpawns());
        InvokeRepeating("SortTeams", 5, 0.5f);
    }



    void SortTeams()
    {
        doneWaiting = true;
        teamPositions.Sort();
        ChangeDisplayNum();
    }


    private void FixedUpdate()
    {
        if(Clientpv == null && FindObjectOfType<myPV>() != null)
        {
            Clientpv = FindObjectOfType<myPV>().pv;
        }

        /*
        text[0].text = "(Driver) " + teamPositions[0].driverName;// + ", " + teamPositions[i].checkpointNumber; 
        text[1].text = "(Shooter) " + teamPositions[0].shooterName;// + ", " + teamPositions[i].checkpointNumber; 
        text[2].text = "(Driver) " + teamPositions[1].driverName;// + ", " + teamPositions[i].checkpointNumber; 
        text[3].text = "(Shooter) " + teamPositions[1].shooterName;// + ", " + teamPositions[i].checkpointNumber; 
        text[4].text = "(Driver) " + teamPositions[2].driverName;// + ", " + teamPositions[i].checkpointNumber; 
        text[5].text = "(Shooter) " + teamPositions[2].shooterName;// + ", " + teamPositions[i].checkpointNumber; 
        */
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


    void ChangeDisplayNum()
    {
        for (int i = 1; i < teamPositions.Count + 1; i++)
        {
            if (teamPositions[i - 1].pv == Clientpv || teamPositions[i - 1].pvS == Clientpv)
            {
                myPositionText.text =  places[i] ;
                //Debug.Log("(Place) " + i);
                break;
            }
        }
    }
}
