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
    [SerializeField] private GameObject photonMenuPlayer; //each player will be given one of these when they join the room

    [Header("Buttons")]
    [SerializeField] private GameObject startButton; //only for the master client

    [Header("Loading")]
    [SerializeField] private bool startLoadingBars = false;
    [SerializeField] private float loadingTime = 6;
    [SerializeField] private GameObject loadingScreen; //activate when the start button is pressed
    [SerializeField] private Image loadingCircle; //increase the fill amount to complete
    [SerializeField] private Image loadingBar; //increase the fill amount to complete
    [SerializeField] private TextMeshProUGUI loadingPercentageText;
    private float loadAmount = 0;
    private float loadingValueNormalized;
    [SerializeField] private float loadPercentage = 0;
    #endregion


    private void Awake()
    {
        pv = GetComponent<PhotonView>();
        loadingScreen.SetActive(false);
        loadingCircle.fillAmount = 0;
        loadingBar.fillAmount = 0;
        loadingPercentageText.text = "0";
    }

    private void Update()
    {
        //counter for the loading bars
        if(startLoadingBars && loadAmount < loadingTime)
        {
            loadAmount += 1 *Time.deltaTime;
            loadingValueNormalized = (loadAmount / loadingTime);
            loadPercentage = loadingValueNormalized * 100;
        }
        //fill the loading bars with the normalized loading value
        loadingCircle.fillAmount = loadingValueNormalized;
        loadingBar.fillAmount = loadingValueNormalized;

        //update load percentage
        loadingPercentageText.text = (loadingValueNormalized * 100).ToString("f0");
    }


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

        ///////////////////////////////////////////////////////////////////////////////////////////////////Spawn photon menu player
        foreach(Player p in PhotonNetwork.PlayerList)
        {
            if(p == PhotonNetwork.LocalPlayer)
            {
                //GameObject player = 
                    PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", photonMenuPlayer.name), this.transform.position, this.transform.rotation, 0);
                //player.transform.parent = playerHolder;
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

        loadingScreen.SetActive(true);
        pv.RPC("RPC_TurnOnLoadingScreen", RpcTarget.AllBuffered);
    }

    [PunRPC]
    private void RPC_TurnOnLoadingScreen()
    {
        loadingScreen.SetActive(true);
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

    private IEnumerator LoadingCouroutine(float time)
    {
        startLoadingBars = true;

        yield return new WaitForSeconds(time);

        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false; //close the room
            PhotonNetwork.LoadLevel(multiplayerSceneIndex);
        }
    }
    #endregion
}
