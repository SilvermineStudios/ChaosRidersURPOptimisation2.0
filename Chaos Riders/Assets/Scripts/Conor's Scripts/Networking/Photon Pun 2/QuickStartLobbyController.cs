using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class QuickStartLobbyController : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private GameObject quickStartButton; //button used for creating and joining a game.
    [SerializeField]
    private GameObject quickCancelButton; //button used to stop searching for a game to join.
    [SerializeField]
    private int RoomSize; //Manual set the number of players in the room at one time.

    public override void OnConnectedToMaster()//callback function for when the first connection is established
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        quickStartButton.SetActive(true);
    }

    #region Quick Start Button
    public void QuickStartButton() //funciton for the quickstart button
    {
        quickStartButton.SetActive(false);
        quickCancelButton.SetActive(true);
        PhotonNetwork.JoinRandomRoom(); //First trys to join an existing room
        Debug.Log("Quick Start");
    }
    public override void OnJoinRandomFailed(short returnCode, string message) //function that plays when join random fails
    {
        Debug.Log("Failed to join a room");
        CreateRoom();
    }

    void CreateRoom() //trying to create our own room
    {
        Debug.Log("Creating room now");
        int randomRoomNumber = Random.Range(0, 10000); //creating a random name for the room
        RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte)RoomSize }; //requirements for the room
        PhotonNetwork.CreateRoom("Room" + randomRoomNumber, roomOps); //attempting to create a new room
        Debug.Log("Random room number: " + randomRoomNumber);
    }

    public override void OnCreateRoomFailed(short returnCode, string message) //function that plays when creating a room fails
    {
        Debug.Log("Failed to create room... trying again");
        CreateRoom(); //trying to create a different room with a new name
    }
    #endregion

    public void QuickCancelButton() //function for the cancel search button
    {
        quickCancelButton.SetActive(false);
        quickStartButton.SetActive(true);
        PhotonNetwork.LeaveRoom(); //leave the room you were searching for
    }
}
