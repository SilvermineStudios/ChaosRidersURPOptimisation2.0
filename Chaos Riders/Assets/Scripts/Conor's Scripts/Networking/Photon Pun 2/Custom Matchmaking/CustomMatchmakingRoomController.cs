using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;

public class CustomMatchmakingRoomController : MonoBehaviourPunCallbacks
{
    #region Variables
    [SerializeField] private int multiplayerSceneIndex; //scene index for loading multiplayer scene

    [SerializeField] private GameObject lobbyPanel; //display for when in lobby
    [SerializeField] private GameObject roomPanel; //display for when in room

    [SerializeField] private GameObject startButton; //only for the master client

    [SerializeField] private Transform playersContainer; //used to display all the players in the current room
    [SerializeField] private GameObject playerListingPrefab; //Instansiate to display each player in the room

    [SerializeField] private TMP_Text roomNameDisplay; //display for the name of the room
    #endregion
   
    void ClearPlayerListings()
    {
        for(int i = playersContainer.childCount -1; i >= 0; i--)
        {
            Destroy(playersContainer.GetChild(i).gameObject);
        }
    }

    void ListPlayers()
    {
        foreach(Player player in PhotonNetwork.PlayerList) //loop through each player 
        {
            GameObject tempListing = Instantiate(playerListingPrefab, playersContainer);
            TMP_Text tempText = tempListing.transform.GetChild(0).GetComponent<TMP_Text>();              
            tempText.text = player.NickName;
        }
    }

    public override void OnJoinedRoom()
    {
        roomPanel.SetActive(true); //activate the display for being in a room
        lobbyPanel.SetActive(false); //hide the display for being in a lobby
        roomNameDisplay.text = PhotonNetwork.CurrentRoom.Name; //update room name display

        if (PhotonNetwork.IsMasterClient) //for the host
        {
            startButton.SetActive(true);
        }
        else
        {
            startButton.SetActive(false);
        }
        //photonPlayers = PhotonNetwork.playerList;
        ClearPlayerListings(); //remove all old player listings
        ListPlayers();
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
            startButton.SetActive(true);
        }
    }

    public void StartGame() //paired to the start button
    {
        if(PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false; //close the room
            PhotonNetwork.LoadLevel(multiplayerSceneIndex);
        }
    }

    IEnumerator rejoinLobby()
    {
        yield return new WaitForSeconds(1);
        PhotonNetwork.JoinLobby();
    }

    public void BackClick() //paired to the back button in the room panel
    {
        lobbyPanel.SetActive(true);
        roomPanel.SetActive(false);
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LeaveLobby();
        StartCoroutine(rejoinLobby());
    }
}
