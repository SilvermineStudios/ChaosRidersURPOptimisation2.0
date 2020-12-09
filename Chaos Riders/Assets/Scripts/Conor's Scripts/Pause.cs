using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Pause : MonoBehaviour
{
    [SerializeField] private GameObject PauseMenu; //Pause Menu Gameobject 
    [SerializeField] private KeyCode pauseMenuButton1, pauseMenuButton2;
    [SerializeField] private bool paused = false;
    private PhotonView pv;

    void Awake()
    {
        pv = GetComponent<PhotonView>();
    }

    void Start()
    {
        if (pv.IsMine && IsThisMultiplayer.Instance.multiplayer || !IsThisMultiplayer.Instance.multiplayer)
        {
            PauseMenu.SetActive(false);
        }
    }

    void Update()
    {
        if (pv.IsMine && IsThisMultiplayer.Instance.multiplayer || !IsThisMultiplayer.Instance.multiplayer)
        {
            if(Input.GetKeyDown(pauseMenuButton1) || Input.GetKeyDown(pauseMenuButton2))
            {
                PauseButton();
                paused = !paused;
            }
        }
    }

    void PauseButton()
    {
        if(paused)
        {
            PauseMenu.SetActive(true);
            paused = true;
        }
        
        if(!paused)
        {
            PauseMenu.SetActive(false);
            paused = false;
        }
    }
}
