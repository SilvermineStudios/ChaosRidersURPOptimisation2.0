using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;

public class CustomPhotonTransformView : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.SendRate = 40; //usually 20
        PhotonNetwork.SerializationRate = 30; //usually 10
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
