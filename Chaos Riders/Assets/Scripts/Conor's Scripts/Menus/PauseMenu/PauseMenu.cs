using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PauseMenu : MonoBehaviour
{
    private PhotonView pv;

    [SerializeField] private GameObject PauseScreen;
    private KeyCode pauseMenuButton1, pauseMenuButton2;


    [HideInInspector] public bool paused = false;
    [SerializeField] private bool online = true;

    void Awake()
    {
        pv = GetComponent<PhotonView>();
        PauseScreen.SetActive(false);

        Cursor.visible = false;

        pauseMenuButton1 = KeyCode.Escape;
        pauseMenuButton2 = KeyCode.P;
    }

    void Update()
    {
        if(!online)
            Pause();

        if(online)
        {
            if (pv.IsMine && IsThisMultiplayer.Instance.multiplayer)
            {
                Pause();
            }
        }
    }

    void Pause()
    {
        if (Input.GetKeyDown(pauseMenuButton1) || Input.GetKeyDown(pauseMenuButton2))
        {
            paused = !paused; //flip if the game is paused when the pause buttons are pressed 

            if (paused)
            {
                PauseScreen.SetActive(true);
                PauseScreen.GetComponent<Pause>().BackButton(); //deactivate all the other settings pages and bring up the front of the pause menu everytime you reopen it
                Cursor.visible = true;
            }
            else
            {
                PauseScreen.SetActive(false);
                Cursor.visible = false;
            }
        }
    }
}
