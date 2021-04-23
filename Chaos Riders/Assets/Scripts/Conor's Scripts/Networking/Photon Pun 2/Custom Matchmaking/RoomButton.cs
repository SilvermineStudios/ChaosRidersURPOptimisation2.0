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
    [SerializeField] private TMP_Text nameText; //display for room name
    [SerializeField] private TMP_Text sizeText; //display for room size
    private CustomMatchmakingRoomController roomContoller;

    private string roomName;
    private int roomSize;
    private int playerCount;
    #endregion

    private void Awake()
    {
        roomContoller = FindObjectOfType<CustomMatchmakingRoomController>();
    }

    public void JoinRoomOnClick() 
    {
        PhotonNetwork.JoinRoom(roomName);
        roomContoller.currentRoomListing = this.gameObject;
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
