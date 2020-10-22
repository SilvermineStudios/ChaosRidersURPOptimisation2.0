using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NetworkController : MonoBehaviourPunCallbacks
{
    //https://doc-api.photonengine.com/en/pun/v2/index.html
    //https://doc.photonengine.com/en-us/pun/current/getting-started/pun-intro

    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings(); // connects to photon master servers
    }


    public override void OnConnectedToMaster() //callback function for when the first connection is established
    {
        Debug.Log("We are now connected to the " + PhotonNetwork.CloudRegion + " server"); //outputs the connected region to the console
    }

    void Update()
    {
        
    }
}
