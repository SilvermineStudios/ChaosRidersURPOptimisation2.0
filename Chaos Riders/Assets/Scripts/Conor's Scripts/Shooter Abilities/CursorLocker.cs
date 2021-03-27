using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CursorLocker : MonoBehaviour
{
    //private Pause pauseVars;
    private PauseMenu pauseVars;
    private bool paused;

    private PhotonView pv;

    private void Awake()
    {
        pv = GetComponent<PhotonView>();
        //pauseVars = GetComponent<Pause>();
        pauseVars = GetComponent<PauseMenu>();
    }

    void Start()
    {
        if (pv.IsMine && IsThisMultiplayer.Instance.multiplayer || !IsThisMultiplayer.Instance.multiplayer)
        {

        }
    }

    void Update()
    {
        if (pv.IsMine && IsThisMultiplayer.Instance.multiplayer || !IsThisMultiplayer.Instance.multiplayer)
        {
            paused = pauseVars.paused;

            if (paused)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }
}
