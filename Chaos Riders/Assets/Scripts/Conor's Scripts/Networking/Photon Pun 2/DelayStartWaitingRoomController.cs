using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DelayStartWaitingRoomController : MonoBehaviourPunCallbacks
{
    //photon view for sending rpc that updates timer
    private PhotonView myPhotonView;

    //scene navigation indexes
    [SerializeField] private int multiplayerSceneIndex;
    [SerializeField] private int menuSceneIndex;
    [SerializeField] private int minPlayersToStart;
    private int playerCount;
    private int roomSize;

    //ui text variables
    [SerializeField] private Text roomCountDisplay;
    [SerializeField] private Text timerToStartDisplay;

    //bool values for if the timer can countdown
    private bool readyToCountDown;
    private bool readyToStart;
    private bool startingGame;
    //countdown timer variables
    private float timerToStartGame;
    private float notFullGameTimer;
    private float fullGameTimer;
    //countdown timer reset variables
    [SerializeField] private float maxWaitTime; 
    [SerializeField] private float maxFullGameWaitTime; 

    void Start()
    {
        //initialize variables
        myPhotonView = GetComponent<PhotonView>();
        fullGameTimer = maxFullGameWaitTime;
        notFullGameTimer = maxWaitTime;
        timerToStartGame = maxWaitTime;

        PlayerCountUpdate();
    }

    
    void Update()
    {
        WaitingForMorePlayers();
    }

    public void PlayerCountUpdate()
    {
        //updates player count when players join the room
        //displays player count
        //triggers countdown timer
        playerCount = PhotonNetwork.PlayerList.Length; //getting the amount of players currently in the room
        roomSize = PhotonNetwork.CurrentRoom.MaxPlayers; //getting the max players of the room you are in
        roomCountDisplay.text = playerCount + ":" + roomSize + " players"; //display how many players are currently in the room out of the max players for the room

        if(playerCount == roomSize) //if max players are in
        {
            readyToStart = true;
        }
        else if(playerCount >= minPlayersToStart) //if there are more than the minimum players needed to start
        {
            readyToCountDown = true;
        }
        else //if there are less than the minimum players needed to start
        {
            readyToCountDown = false;
            readyToStart = false;
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer) //called whenever a new player joins the room
    {
        PlayerCountUpdate();

        //send master clients countdown timer to all other players in order to sync time
        if (PhotonNetwork.IsMasterClient)
            myPhotonView.RPC("RPC_SendTimer", RpcTarget.Others, timerToStartGame);
    }

    [PunRPC]
    private void RPC_SendTimer(float timeIn)
    {
        //RPC for syncing the countdown timer to those that join after it has started the countdown
        timerToStartGame = timeIn;
        notFullGameTimer = timeIn;
        if(timeIn < fullGameTimer)
        {
            fullGameTimer = timeIn;
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer) //called whenever a player leaves the room
    {
        PlayerCountUpdate();
    }

    private void WaitingForMorePlayers()
    {
        if(playerCount <= 1) //if there is only 1 player the timer will stop and reset
        {
            ResetTimer();
        }
        if(readyToStart) //when there is enough players in the room the countdown timer will start
        {
            fullGameTimer -= Time.deltaTime;
            timerToStartGame = fullGameTimer;
        }
        else if(readyToCountDown)
        {
            notFullGameTimer -= Time.deltaTime;
            timerToStartGame = notFullGameTimer;
        }

        //format and display countdown timer
        string tempTimer = string.Format("{0:00}", timerToStartGame);
        timerToStartDisplay.text = tempTimer;

        if (timerToStartGame <= 0f) //if the countdown timer reaches 0 the game will then start
        {
            if (startingGame)
                return;
            StartGame();
        }
    }

    private void ResetTimer() //resets the countdown timer
    {
        timerToStartGame = maxWaitTime;
        notFullGameTimer = maxWaitTime;
        fullGameTimer = maxFullGameWaitTime;
    }

    public void StartGame() //multiplayer scene is loaded to start the game
    {
        startingGame = true;
        if (!PhotonNetwork.IsMasterClient)
            return;
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.LoadLevel(multiplayerSceneIndex);
    }

    public void DelayCancelButton() //paired to the cancelbutton in the waiting room
    {
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene(menuSceneIndex);
    }
}
