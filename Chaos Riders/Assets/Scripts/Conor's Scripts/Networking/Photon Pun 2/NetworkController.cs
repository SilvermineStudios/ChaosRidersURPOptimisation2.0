﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class NetworkController : MonoBehaviourPunCallbacks
{
    //https://doc-api.photonengine.com/en/pun/v2/index.html
    //https://doc.photonengine.com/en-us/pun/current/getting-started/pun-intro
    [SerializeField] private TMP_Text servertext; //display for the name of the server you connect to

    [SerializeField] private bool useRegionSelector = false; //choose if you want to manually select your region or not

    // Start is called before the first frame update
    void Start()
    {
        //IF YOU ARE NOT USING THE REGION SELECTOR AND WANT TO AUTOCONNECT TO NEAREST SERVER
        if(!useRegionSelector)
            PhotonNetwork.ConnectUsingSettings(); // connects to photon master servers
    }


    public override void OnConnectedToMaster() //callback function for when the first connection is established
    {
        Debug.Log("We are now connected to the " + PhotonNetwork.CloudRegion + " server"); //outputs the connected region to the console
        servertext.text = "You are connected to the: " + PhotonNetwork.CloudRegion + " server";
    }

    void Update()
    {

    }
}
