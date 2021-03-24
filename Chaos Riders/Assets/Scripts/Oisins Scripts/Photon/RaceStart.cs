using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class RaceStart : MonoBehaviourPunCallbacks
{


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void InitialiseTimer()
    {
        //Time Stuff

        if(PhotonNetwork.IsMasterClient)
        {
            
        }
    }



    private IEnumerator Timer()
    {
        yield return new WaitForSeconds(1);


    }
}
