using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class RegionSelector : MonoBehaviour
{
    public TMP_Dropdown regionDropdown;

    private void Start()
    {
        HandleInput(0);

        PhotonNetwork.NetworkingClient.AppId = PhotonNetwork.PhotonServerSettings.AppSettings.AppIdRealtime;
    }

    public void HandleInput(int val)
    {
        val = regionDropdown.value;

        if (val == 0)
            ConnectToEurope();

        if (val == 1)
            ConnectToRussia();
    }


    

    public void ConnectToEurope()
    {
        PhotonNetwork.Disconnect();
        PhotonNetwork.PhotonServerSettings.AppSettings.FixedRegion = "eu";
        PhotonNetwork.ConnectUsingSettings();
        Debug.Log("EUROPE");
    }

    public void ConnectToRussia()
    {
        PhotonNetwork.Disconnect();
        PhotonNetwork.PhotonServerSettings.AppSettings.FixedRegion = "ru";
        PhotonNetwork.ConnectUsingSettings();
        Debug.Log("RUSSIA");
    }
}
