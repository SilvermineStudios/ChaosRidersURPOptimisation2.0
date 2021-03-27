using System.Collections;
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
    [SerializeField] private TMP_Text roomNameDisplay; //display for the name of the room
    [SerializeField] private int multiplayerSceneIndex; //scene index for loading multiplayer scene

    [Header("Panels")]
    [SerializeField] private GameObject lobbyPanel; //display for when in lobby
    [SerializeField] private GameObject roomPanel; //display for when in room#

    [Header("Player Listings")]
    [SerializeField] private Transform playersContainer; //used to display all the players in the current room
    [SerializeField] private GameObject playerListingPrefab; //Instansiate to display each player in the room
    public List<GameObject> playerNameBoxes = new List<GameObject>();
    [SerializeField] private GameObject photonMenuPlayer; //each player will be given one of these when they join the room
    [SerializeField] private GameObject playerDataManager; //

    [Header("Buttons")]
    public GameObject startButton; //only for the master client

    [Header("Loading")]
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private float loadingTime;
    public static float LoadingTime;
    [SerializeField] public TMP_Text hintText;

    #endregion

    private void Awake()
    {
        pv = GetComponent<PhotonView>();
        loadingScreen.SetActive(false);
        LoadingTime = loadingTime;
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
            GameObject tempListing = Instantiate(playerListingPrefab, playersContainer);
            TMP_Text tempText = tempListing.transform.GetChild(1).GetComponent<TMP_Text>();
            Debug.Log("Players nickname is: " + player.NickName);
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
            startButton.SetActive(true);
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", playerDataManager.name), this.transform.position, this.transform.rotation, 0);
        }
        else
        {
            startButton.SetActive(false);
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
    }
    #endregion
    #region Couroutines
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
            PhotonNetwork.CurrentRoom.IsOpen = false; //close the room
            PhotonNetwork.LoadLevel(multiplayerSceneIndex);
        }
    }
    #endregion
}
