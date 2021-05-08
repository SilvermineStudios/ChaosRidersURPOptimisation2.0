using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;

public class RoomButton : MonoBehaviour
{
    #region Variables
    private CustomMatchmakingRoomController roomController;
    [SerializeField] private TMP_Text nameText; //display for room name
    [SerializeField] private TMP_Text sizeText; //display for room size

    private string roomName;
    private int roomSize;
    private int playerCount;
    #endregion

    private void Awake()
    {
        roomController = FindObjectOfType<CustomMatchmakingRoomController>();
    }

    public void JoinRoomOnClick() 
    {
        PhotonNetwork.JoinRoom(roomName);
        roomController.thisRoomListing = this.gameObject;
    }

    public void SetRoom(string nameInput, int sizeInput, int countInput)
    {
        roomName = nameInput;
        roomSize = sizeInput;
        playerCount = countInput;
        nameText.text = nameInput;
        sizeText.text = countInput + "/" + sizeInput;
    }
}
