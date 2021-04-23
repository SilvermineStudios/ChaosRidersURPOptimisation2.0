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

    private string roomName;
    private int roomSize;
    private int playerCount;
    #endregion

    public void JoinRoomOnClick() 
    {
        PhotonNetwork.JoinRoom(roomName);
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
