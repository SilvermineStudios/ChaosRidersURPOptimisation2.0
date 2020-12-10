using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class Pause : MonoBehaviour
{
    [SerializeField] private GameObject PauseMenu, audioSettingsPanel, mainPauseMenuPanel; //Pause Menu Gameobject 
    [SerializeField] private KeyCode pauseMenuButton1, pauseMenuButton2;
    [SerializeField] private bool paused = false;
    [SerializeField] private int LobbySceneIndex = 0;
    private PhotonView pv;
    public AudioMixer mixer;

    void Awake()
    {
        pv = GetComponent<PhotonView>();
        PauseMenu.SetActive(false); //deactivate pause menu
    }

    void Update()
    {
        if (pv.IsMine && IsThisMultiplayer.Instance.multiplayer || !IsThisMultiplayer.Instance.multiplayer)
        {
            //check if the pause buttons are pressed
            if(Input.GetKeyDown(pauseMenuButton1) || Input.GetKeyDown(pauseMenuButton2))
            {
                paused = !paused; //flip if the game is paused when the pause buttons are pressed 


                if (paused)
                {
                    PauseMenu.SetActive(true);
                    BackButton(); //deactivate all the other settings pages and bring up the front of the pause menu everytime you reopen it
                }
                else
                {
                    PauseMenu.SetActive(false);
                }
            }
        }
    }

    #region Buttons
    public void LeaveRaceButton()
    {
        if (pv.IsMine && IsThisMultiplayer.Instance.multiplayer || !IsThisMultiplayer.Instance.multiplayer)
        {
            PhotonNetwork.Disconnect(); //disconnect the player from the photon network before removing them from the game
            SceneManager.LoadScene(LobbySceneIndex); //return to the custom matchmaking lobby
        }
    }

    public void QuitGameButton()
    {
        if (pv.IsMine && IsThisMultiplayer.Instance.multiplayer || !IsThisMultiplayer.Instance.multiplayer)
        {
            PhotonNetwork.Disconnect(); //disconnect the player from the photon network before removing them from the game
            Application.Quit(); //close the game for the player         
        }
    }

    public void AudioSettingsButton()
    {
        if (pv.IsMine && IsThisMultiplayer.Instance.multiplayer || !IsThisMultiplayer.Instance.multiplayer)
        {
            mainPauseMenuPanel.SetActive(false); // disable the main pause menu panel 
            audioSettingsPanel.SetActive(true); //enable the audio settings panel
        }
    }

    public void BackButton()
    {
        if (pv.IsMine && IsThisMultiplayer.Instance.multiplayer || !IsThisMultiplayer.Instance.multiplayer)
        {
            mainPauseMenuPanel.SetActive(true); //enable the main pause menu panel 
            audioSettingsPanel.SetActive(false); //disable the audio settings panel
        }
    }

    public void ResumeGameButton()
    {
        if (pv.IsMine && IsThisMultiplayer.Instance.multiplayer || !IsThisMultiplayer.Instance.multiplayer)
        {
            PauseMenu.SetActive(false); //close the pause menu
        }   
    }
    #endregion

    #region Sliders
    public void SetMasterLevel(float sliderValue)
    {
        if (pv.IsMine && IsThisMultiplayer.Instance.multiplayer || !IsThisMultiplayer.Instance.multiplayer)
            mixer.SetFloat("Master", Mathf.Log10(sliderValue) * 20);
    }

    public void SetMusicLevel(float sliderValue)
    {
        if (pv.IsMine && IsThisMultiplayer.Instance.multiplayer || !IsThisMultiplayer.Instance.multiplayer)
            mixer.SetFloat("Music", Mathf.Log10(sliderValue) * 20);
    }

    public void SetSFXLevel(float sliderValue)
    {
        if (pv.IsMine && IsThisMultiplayer.Instance.multiplayer || !IsThisMultiplayer.Instance.multiplayer)
            mixer.SetFloat("SFX", Mathf.Log10(sliderValue) * 20);
    }
    #endregion
}
