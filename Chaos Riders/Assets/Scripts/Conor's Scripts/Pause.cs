using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    [SerializeField] private GameObject PauseMenu; //Pause Menu Gameobject 
    [SerializeField] private KeyCode pauseMenuButton1, pauseMenuButton2;
    [SerializeField] private bool paused = false;
    [SerializeField] private int LobbySceneIndex = 0;
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
            PhotonNetwork.Disconnect();
            SceneManager.LoadScene(LobbySceneIndex);
        }
    }

    public void QuitGame()
    {
        if (pv.IsMine && IsThisMultiplayer.Instance.multiplayer || !IsThisMultiplayer.Instance.multiplayer)
        {
            PhotonNetwork.Disconnect();
            Application.Quit();        
        }
    }
}
