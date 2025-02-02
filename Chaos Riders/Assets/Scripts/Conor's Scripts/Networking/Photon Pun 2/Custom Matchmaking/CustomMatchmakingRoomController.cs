﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class CustomMatchmakingRoomController : MonoBehaviourPunCallbacks
{
    #region Variables
    private PhotonView pv;
    public GameObject thisRoomListing;
    [SerializeField] private float disconnectTime = 2f;
    private bool isHost = false;
    [SerializeField] private TMP_Text roomNameDisplay; //display for the name of the room
    //[SerializeField] private int multiplayerSceneIndex; //scene index for loading multiplayer scene
    [SerializeField] private int daySceneIndex = 1; //scene index for loading multiplayer scene
    [SerializeField] private int nightSceneIndex = 2; //scene index for loading multiplayer scene

    [Header("Panels")]
    [SerializeField] private GameObject mainMenuPanel; 
    [SerializeField] private GameObject lobbyPanel; //display for when in lobby
    [SerializeField] private GameObject roomPanel; //display for when in room
    [SerializeField] private GameObject disconnectPanel; //display for when in room

    [Header("Player Listings")]
    [SerializeField] private Transform playersContainer; //used to display all the players in the current room
    [SerializeField] private GameObject playerListingPrefab; //Instansiate to display each player in the room
    public List<GameObject> playerNameBoxes = new List<GameObject>();
    [SerializeField] private GameObject photonMenuPlayer; //each player will be given one of these when they join the room
    [SerializeField] private GameObject playerDataManager; //
    [SerializeField] private GameObject playerDataManagerForCurrentRoom;

    [Header("Buttons")]
    public GameObject startButton; //only for the master client

    [Header("Loading")]
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private float loadingTime;
    public static float LoadingTime;
    [SerializeField] public TMP_Text hintText;

    [Header("Starting Race Stuff")]
    [SerializeField] private TMP_Text gameMessageText;
    [SerializeField] private string cantStartMsg;
    [SerializeField] private string loadingDots;
    [SerializeField] private string readyToStartMsg;
    [SerializeField] private bool canStartRace = false;
    private bool CanStartRace()
    {
        if (PhotonNetwork.IsMasterClient && playerDataManagerForCurrentRoom != null)
        {
            PlayerDataManager pdm = playerDataManagerForCurrentRoom.GetComponent<PlayerDataManager>();
            if (pdm.photonMenuPlayers.Length > 0)
            {
                for (int i = 0; i < pdm.photonMenuPlayers.Length; i++)
                {
                    if (pdm.photonMenuPlayers[i].picked == false)
                        return false;
                }
            }
        }
        return true;
    }
    #endregion



    private void Awake()
    {
        pv = GetComponent<PhotonView>();
        loadingScreen.SetActive(false);
        disconnectPanel.SetActive(false);
        LoadingTime = loadingTime;
        startButton.SetActive(false);
    }

    private void Update()
    {
        playerNameBoxes.RemoveAll(x => x == null);

        CheckIfCanStart();
    }

    void CheckIfCanStart()
    {
        //checking if the race can start
        canStartRace = CanStartRace();

        if(PhotonNetwork.IsMasterClient)
        {
            if(canStartRace)
            {
                startButton.SetActive(true);
                pv.RPC("UpdateGameMessageText", RpcTarget.All, canStartRace);
            }
            else
            {
                startButton.SetActive(false);
                pv.RPC("UpdateGameMessageText", RpcTarget.All, canStartRace);
            }
        }   
    }

    [PunRPC]
    void UpdateGameMessageText(bool ready)
    {
        if(ready)
        {
            gameMessageText.text = readyToStartMsg;
        }
        else
        {
            gameMessageText.text = cantStartMsg + loadingDots;
            LoadingDots();
        }
    }

    public float elipseCooldown, elipseRate;
    bool timeRecorded;
    private void LoadingDots()
    {
        if(!timeRecorded)
        {
            timeRecorded = true;
            elipseCooldown = Time.time;
        }
        loadingDots = "";

        if (Time.time >= elipseCooldown + elipseRate)
        {
            loadingDots = ".";

            if (Time.time >= elipseCooldown + 2 * elipseRate)
            {
                loadingDots = "..";
                if (Time.time >= elipseCooldown + 3 * elipseRate)
                {
                    loadingDots = "..";
                    if (Time.time >= elipseCooldown + 3 * elipseRate)
                    {
                        loadingDots = "...";
                        if (Time.time >= elipseCooldown + 4 * elipseRate)
                        {
                            timeRecorded = false;
                        }
                    }
                }
            }
        }
        
    }

    void ClearPlayerListings()
    {
        for(int i = playersContainer.childCount -1; i >= 0; i--)
        {
            playerNameBoxes.Remove(playerNameBoxes[i]);
            Destroy(playersContainer.GetChild(i).gameObject);
        }
    }

    void ListPlayers()
    {
        foreach(Player player in PhotonNetwork.PlayerList) //loop through each player 
        {
            //GameObject tempListing = Instantiate(playerListingPrefab, playersContainer);
            GameObject tempListing = PhotonNetwork.Instantiate(playerListingPrefab.name, this.transform.position, this.transform.rotation, 0);
            
            TMP_Text tempText = tempListing.transform.GetChild(1).GetComponent<TMP_Text>();
            tempListing.transform.parent = playersContainer;
            tempListing.transform.localScale = new Vector3(5.289001f, 1, 1);
            //Debug.Log("Players nickname is: " + player.NickName);
            tempText.text = player.NickName;
            
            playerNameBoxes.Add(tempListing);
        }
    }


    public override void OnJoinedRoom()
    {
        roomPanel.SetActive(true); //activate the display for being in a room
        lobbyPanel.SetActive(false); //hide the display for being in a lobby
        roomNameDisplay.text = PhotonNetwork.CurrentRoom.Name; //update room name display

        if (PhotonNetwork.IsMasterClient) //for the host
        {
            //startButton.SetActive(true);
            playerDataManagerForCurrentRoom = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", playerDataManager.name), this.transform.position, this.transform.rotation, 0);
            isHost = true;
        }
        else
        {
            //startButton.SetActive(false);
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////Spawn photon menu player
        foreach(Player p in PhotonNetwork.PlayerList)
        {
            if(p == PhotonNetwork.LocalPlayer)
            {
                PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", photonMenuPlayer.name), this.transform.position, this.transform.rotation, 0);
            }
        }

        //photonPlayers = PhotonNetwork.playerList;
        ClearPlayerListings(); //remove all old player listings
        ListPlayers();

        //InvokeRepeating("CheckIfCanStart", 0, 1);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer) 
    {
        ClearPlayerListings(); //remove all old player listings
        ListPlayers(); //relist all current player listings

    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        ClearPlayerListings(); //remove all old player listings
        ListPlayers(); //relist all current player listings

        if (PhotonNetwork.IsMasterClient) //if the host leaves give the new host access to the start button
        {
            // startButton.SetActive(true);

            if(!isHost)
            {
                PhotonNetwork.CurrentRoom.IsOpen = false; // makes room close 
                PhotonNetwork.CurrentRoom.IsVisible = false; // makes room invisible to random match
                StartCoroutine(DisconnectedCouroutine(disconnectTime));
            }
        }
    }

    private void DisconnectEveryone()
    {
        if (thisRoomListing != null)
            Destroy(thisRoomListing);

        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LeaveLobby();
        StartCoroutine(rejoinLobby());

        disconnectPanel.SetActive(false);
        lobbyPanel.SetActive(true);
        roomPanel.SetActive(false);
    }

    #region Buttons
    public void StartGame() //paired to the start button
    {
        StartCoroutine(LoadingCouroutine(loadingTime));
        pv.RPC("RPC_TurnOnLoadingScreen", RpcTarget.All);
    }
    [PunRPC]
    private void RPC_TurnOnLoadingScreen ()
    {
        loadingScreen.SetActive(true);
        //Debug.Log("ACTIVATING LOADING SCREEN");
    }

    [PunRPC]
    private void Activate(int viewId)
    {
        Debug.Log("Activate");
        PhotonView view = PhotonView.Find(viewId);
        view.GetComponent<GameObject>().SetActive(true);
    }

    public void BackClick() //paired to the back button in the room panel
    {
        lobbyPanel.SetActive(true);
        roomPanel.SetActive(false);
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LeaveLobby();
        StartCoroutine(rejoinLobby());
        isHost = false;

        if (thisRoomListing != null)
            Destroy(thisRoomListing);
    }
    #endregion
    #region Couroutines
    private IEnumerator DisconnectedCouroutine(float time)
    {
        disconnectPanel.SetActive(true);

        yield return new WaitForSeconds(time);

        DisconnectEveryone();
    }


    IEnumerator rejoinLobby()
    {
        yield return new WaitForSeconds(1);
        PhotonNetwork.JoinLobby();
    }

    //countdown until the game starts
    private IEnumerator LoadingCouroutine(float time)
    {
        yield return new WaitForSeconds(time);

        if (PhotonNetwork.IsMasterClient)
        {
            if(GameVariables.Daytime)
            {
                PhotonNetwork.CurrentRoom.IsOpen = false; //close the room
                PhotonNetwork.LoadLevel(daySceneIndex);
            }
            else
            {
                PhotonNetwork.CurrentRoom.IsOpen = false; //close the room
                PhotonNetwork.LoadLevel(nightSceneIndex);
            }
        }
    }
    #endregion

}
