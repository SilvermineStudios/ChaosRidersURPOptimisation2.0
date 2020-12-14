using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MultiplayerCameraCheck : MonoBehaviour
{
    public List<GameObject> cameras = new List<GameObject>(); //cameras to disable if you dont own them
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
                foreach (GameObject camera in cameras)
                {
                    camera.SetActive(true); //turn on the cameras if they dont belong to you
                }
            }

            if (!pv.IsMine)
            {
                foreach (GameObject camera in cameras)
                {
                    camera.SetActive(false); //turn off the cameras if they dont belong to you
                }
            }
        }
        else
            return;
    }
}
