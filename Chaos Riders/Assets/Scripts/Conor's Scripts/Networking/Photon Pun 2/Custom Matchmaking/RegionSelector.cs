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
        PhotonNetwork.PhotonServerSettings.AppSettings.FixedRegion = null; //reset the fixed region to be blank
        PhotonNetwork.ConnectUsingSettings(); // connets to the region with the best ping

        PhotonNetwork.NetworkingClient.AppId = PhotonNetwork.PhotonServerSettings.AppSettings.AppIdRealtime;

        regionDropdown.captionText.text = "Select Region";
    }




    //controller used by the drop down menu
    public void HandleInput(int val)
    {
        val = regionDropdown.value;

        if (val == 0)
            ConnectToAsia();

        if (val == 1)
            ConnectToAustralia();

        if (val == 2)
            ConnectToCanadaEast();

        if (val == 3)
            ConnectToEurope();

        if (val == 4)
            ConnectToIndia();

        if (val == 5)
            ConnectToJapan();

        if (val == 6)
            ConnectToRussia();

        if (val == 7)
            ConnectToRussiaEast();

        if (val == 8)
            ConnectToSouthAfrica();

        if (val == 9)
            ConnectToSouthAmerica();

        if (val == 10)
            ConnectToSouthKorea();

        if (val == 11)
            ConnectToUSAEast();

        if (val == 12)
            ConnectToUSAWest();
    }





    #region Voids to Connect to different regions
    //0
    public void ConnectToAsia() 
    {
        PhotonNetwork.Disconnect(); //disconnect from the current region 
        PhotonNetwork.PhotonServerSettings.AppSettings.FixedRegion = "asia"; //select the region you want to join
        PhotonNetwork.ConnectUsingSettings(); //connect to that region with all the other settings
    }

    //1
    public void ConnectToAustralia()
    {
        PhotonNetwork.Disconnect();
        PhotonNetwork.PhotonServerSettings.AppSettings.FixedRegion = "au";
        PhotonNetwork.ConnectUsingSettings();
    }

    //2
    public void ConnectToCanadaEast()
    {
        PhotonNetwork.Disconnect();
        PhotonNetwork.PhotonServerSettings.AppSettings.FixedRegion = "cae";
        PhotonNetwork.ConnectUsingSettings();
    }

    //3
    public void ConnectToEurope()
    {
        PhotonNetwork.Disconnect();
        PhotonNetwork.PhotonServerSettings.AppSettings.FixedRegion = "eu";
        PhotonNetwork.ConnectUsingSettings();
    }

    //4
    public void ConnectToIndia()
    {
        PhotonNetwork.Disconnect();
        PhotonNetwork.PhotonServerSettings.AppSettings.FixedRegion = "in";
        PhotonNetwork.ConnectUsingSettings();
    }

    //5
    public void ConnectToJapan()
    {
        PhotonNetwork.Disconnect();
        PhotonNetwork.PhotonServerSettings.AppSettings.FixedRegion = "jp";
        PhotonNetwork.ConnectUsingSettings();
    }

    //6
    public void ConnectToRussia()
    {
        PhotonNetwork.Disconnect();
        PhotonNetwork.PhotonServerSettings.AppSettings.FixedRegion = "ru";
        PhotonNetwork.ConnectUsingSettings();
    }

    //7
    public void ConnectToRussiaEast()
    {
        PhotonNetwork.Disconnect();
        PhotonNetwork.PhotonServerSettings.AppSettings.FixedRegion = "rue";
        PhotonNetwork.ConnectUsingSettings();
    }

    //8
    public void ConnectToSouthAfrica()
    {
        PhotonNetwork.Disconnect();
        PhotonNetwork.PhotonServerSettings.AppSettings.FixedRegion = "za";
        PhotonNetwork.ConnectUsingSettings();
    }

    //9
    public void ConnectToSouthAmerica()
    {
        PhotonNetwork.Disconnect();
        PhotonNetwork.PhotonServerSettings.AppSettings.FixedRegion = "sa";
        PhotonNetwork.ConnectUsingSettings();
    }

    //10
    public void ConnectToSouthKorea()
    {
        PhotonNetwork.Disconnect();
        PhotonNetwork.PhotonServerSettings.AppSettings.FixedRegion = "kr";
        PhotonNetwork.ConnectUsingSettings();
    }

    //11
    public void ConnectToUSAEast()
    {
        PhotonNetwork.Disconnect();
        PhotonNetwork.PhotonServerSettings.AppSettings.FixedRegion = "us";
        PhotonNetwork.ConnectUsingSettings();
    }

    //12
    public void ConnectToUSAWest()
    {
        PhotonNetwork.Disconnect();
        PhotonNetwork.PhotonServerSettings.AppSettings.FixedRegion = "usw";
        PhotonNetwork.ConnectUsingSettings();
    }
    #endregion
}
