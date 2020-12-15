using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class DisableMultiplayerStuff : MonoBehaviour
{
    public List<GameObject> objectsToDisableIfNotYours = new List<GameObject>(); //objects to disable if you dont own them
    private PhotonView pv;

    private void Awake()
    {
        pv = GetComponent<PhotonView>();
    }

    private void Start()
    {
        if (IsThisMultiplayer.Instance.multiplayer)
        {
            if (pv.IsMine)
            {
                foreach (GameObject camera in objectsToDisableIfNotYours)
                {
                    camera.SetActive(true); //turn on the objects if they dont belong to you
                }
            }

            if (!pv.IsMine)
            {
                foreach (GameObject camera in objectsToDisableIfNotYours)
                {
                    camera.SetActive(false); //turn off the objects if they dont belong to you
                }
            }
        }
        else
            return;
    }
}
