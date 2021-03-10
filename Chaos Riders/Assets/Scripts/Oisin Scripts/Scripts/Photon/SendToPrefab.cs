using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendToPrefab : MonoBehaviour
{
    public PhotonMenuPlayer prefabToGet { get; private set; }



    void Update()
    {
        if(prefabToGet == null)
        {
            if(FindObjectOfType<PhotonMenuPlayer>() != null)
            {
                prefabToGet = FindObjectOfType<PhotonMenuPlayer>();
            }
        }
    }

    public void SendtoPrefab()
    {
        prefabToGet.BackButtonPress();
    }
}
