using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;

public class CustomMatchmakingLobbyController : MonoBehaviourPunCallbacks
{
    #region Variables
    [SerializeField] private GameObject lobbyConnectButton; //button used for joining lobby
    [SerializeField] private GameObject lobbyPanel; //panel for displaying lobby
    [SerializeField] private GameObject mainPanel; //panel for displaying main menu

    public TMP_InputField playerNameInput; //input field so player can change their NickName

    private string roomName; //string for saving room name
    private int roomSize; //int for saving room size

    private List<RoomInfo> roomLisings; //list of current rooms
    [SerializeField] Transform roomsContainer; //container for holding all the room listings
    [SerializeField] GameObject roomListingPrefab; //prefab for displaying each room in the lobby
    #endregion

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        lobbyConnectButton.SetActive(true); //activate the button for connecting to the lobby
        roomLisings = new List<RoomInfo>();

        //check for player name saved to player prefs
        if(PlayerPrefs.HasKey("NickName"))
        {
            if(PlayerPrefs.GetString("NickName") == "")
            {
                PhotonNetwork.NickName = "Player " + Random.Range(0, 1000); //random player name when not chosen
            }
            else
            {
                PhotonNetwork.NickName = PlayerPrefs.GetString("NickName"); // get the saved player name
            }
        }
        else
        {
            PhotonNetwork.NickName = "Player " + Random.Range(0, 1000); //random player name when not chosen
        }
        //playerNameInput.text = PhotonNetwork.NickName; //update input field with player name
    }

    public void PlayerNameUpdate(string nameInput)
    {
        PhotonNetwork.NickName = nameInput;
        PlayerPrefs.SetString("NickName", nameInput);
        playerNameInput.text = nameInput;
    }

    public void JoinLobbyOnClick() // paired to the delay start button
    {
        mainPanel.SetActive(false);
        lobbyPanel.SetActive(true);
        PhotonNetwork.JoinLobby(); //first tries to join an existing room
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        int tempIndex;
        foreach (RoomInfo room in roomList) //loop through each room in the room list
        {
            if(roomLisings != null) // try to find an existing room listing
            {
                tempIndex = roomLisings.FindIndex(ByName(room.Name));
            }
            else
            {
                tempIndex = -1;
            }
            if(tempIndex != -1) //remove listing because it has been closed
            {
                roomLisings.RemoveAt(tempIndex);
                Destroy(roomsContainer.GetChild(tempIndex).gameObject);
            }
            if(room.PlayerCount > 0) //add room listing because its new
            {
                roomLisings.Add(room);
                ListRoom(room);
            }
        }
    }

    static System.Predicate<RoomInfo> ByName(string name) //predicate function for searching for search through room
    {
        return delegate (RoomInfo room)
        {
            return room.Name == name;
        };
    }

    void ListRoom(RoomInfo room) //siaplays new room listing for the current room
    {
        if(room.IsOpen && room.IsVisible)
        {
            GameObject tempListing = Instantiate(roomListingPrefab, roomsContainer);
            RoomButton tempButton = tempListing.GetComponent<RoomButton>();
            tempButton.SetRoom(room.Name, room.MaxPlayers, room.PlayerCount);
        }
    }

    public void OnRoomNameChanged(string nameIn) //input function for changing room name
    {
        roomName = nameIn;
    }
    public void OnRoomSizeChanged(string sizeIn) //input function for changing room size
    {
        roomSize = int.Parse(sizeIn);
    }

    public void CreateRoom() //function paired to the create room button
    {
        //Debug.Log("Creating Room Now");
        RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte)roomSize };
        PhotonNetwork.CreateRoom(roomName, roomOps); //attempting to create a new room
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Tried to create a new room but failed, try change the name of the room!");
    }



    public void MatchmakingCancel() //paired to the cancel button / used to go back to main menu
    {
        mainPanel.SetActive(true);
        lobbyPanel.SetActive(false);
        PhotonNetwork.LeaveLobby();
    }
}
