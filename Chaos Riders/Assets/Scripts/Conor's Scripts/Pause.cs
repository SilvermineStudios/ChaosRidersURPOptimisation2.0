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
        PauseMenu.SetActive(false); //deactivate pause menu
    }

    void Update()
    {
        if (pv.IsMine && IsThisMultiplayer.Instance.multiplayer || !IsThisMultiplayer.Instance.multiplayer)
        {
            if(Input.GetKeyDown(pauseMenuButton1) || Input.GetKeyDown(pauseMenuButton2))
            {
                paused = !paused;

                if (paused)
                {
                    PauseMenu.SetActive(true);
                }
                else
                {
                    PauseMenu.SetActive(false);
                }
            }
        }
    }

    public void LeaveRace()
    {
        if (pv.IsMine && IsThisMultiplayer.Instance.multiplayer || !IsThisMultiplayer.Instance.multiplayer)
        {
            //Debug.Log("Leave Race");
        }
    }

    public void QuitGame()
    {
        if (pv.IsMine && IsThisMultiplayer.Instance.multiplayer || !IsThisMultiplayer.Instance.multiplayer)
        {
            //Debug.Log("Quit Game");
            Application.Quit();        
        }
    }
}
