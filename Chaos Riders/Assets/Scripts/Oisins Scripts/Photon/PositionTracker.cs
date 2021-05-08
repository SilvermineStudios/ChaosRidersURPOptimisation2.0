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
using ExitGames.Client.Photon;


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
    [SerializeField] GameObject Holder;
    [SerializeField] TextMeshProUGUI myPositionText;
    int currentPlaceForFinsh = 1;

    private void Awake()
    {
        pv = GetComponent<PhotonView>();
        playerDataManager = FindObjectOfType<PlayerDataManager>();
    }

    void Start()
    {
        StartCoroutine(WaitForSpawns());
        //InvokeRepeating("SortTeams", 5, 0.5f);
    }

    private void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
    }

    private void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= OnEvent;
    }

    public void OnEvent(EventData photonEvent)
    {
        Debug.Log("Received");
        byte eventCode = photonEvent.Code;

        if (eventCode == 1)
        {
            object[] data = (object[])photonEvent.CustomData;

            CreateEntry((string)data[0],(string)data[1]);
            Debug.Log((string)data[0]);
        }
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
        if (teamPositions.Count > 0 && currentPlaceForFinsh < 5)
        {
            foreach (Position p in teamPositions)
            {
                if (p.finishedRace)
                {
                    CreateEntry(p);
                    teamPositions.Remove(p);
                    break;
                }
            }
        }
        
        text[0].text = "(Driver) " + teamPositions[0].driverName;// + ", " + teamPositions[i].checkpointNumber; 
        text[1].text = "(Shooter) " + teamPositions[0].shooterName;// + ", " + teamPositions[i].checkpointNumber; 
        text[2].text = "(Driver) " + teamPositions[1].driverName;// + ", " + teamPositions[i].checkpointNumber; 
        text[3].text = "(Shooter) " + teamPositions[1].shooterName;// + ", " + teamPositions[i].checkpointNumber; 
        text[4].text = "(Driver) " + teamPositions[2].driverName;// + ", " + teamPositions[i].checkpointNumber; 
        text[5].text = "(Shooter) " + teamPositions[2].shooterName;// + ", " + teamPositions[i].checkpointNumber; 
        */
    }

    void CreateEntry(string driverName, string shooterName)
    {
        GameObject Entry = PhotonNetwork.Instantiate("Entry", Vector3.zero, Quaternion.identity);
        Entry.transform.parent = Holder.transform;
        Entry.GetComponent<EntryScript>().Setup(driverName, shooterName, places[currentPlaceForFinsh]);
        currentPlaceForFinsh++;
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
