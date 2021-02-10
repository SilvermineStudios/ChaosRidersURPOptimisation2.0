using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class Pause : MonoBehaviour
{
    //Audio
    FMOD.Studio.Bus Music;
    FMOD.Studio.Bus SFX;
    FMOD.Studio.Bus Master;
    float MusicVolume = 0.5f;
    float SFXVolume = 0.5f;
    float MasterVolume = 1;

    //Pause Menu Gameobject 
    [SerializeField] private GameObject PauseMenu, audioSettingsPanel, mainPauseMenuPanel, controlsPanel; 
    [SerializeField] private KeyCode pauseMenuButton1, pauseMenuButton2;
    public bool paused = false;
    [SerializeField] private int LobbySceneIndex = 0;
    private PhotonView pv;

    void Awake()
    {
        pv = GetComponent<PhotonView>();
        PauseMenu.SetActive(false); //deactivate pause menu
        Music = FMODUnity.RuntimeManager.GetBus("bus:/Master/Music");
        SFX = FMODUnity.RuntimeManager.GetBus("bus:/Master/SFX");
        Master = FMODUnity.RuntimeManager.GetBus("bus:/Master");
    }

    void Update()
    {
        if (pv.IsMine && IsThisMultiplayer.Instance.multiplayer || !IsThisMultiplayer.Instance.multiplayer)
        {
            // AUDIO SETTINGS
            Music.setVolume(MusicVolume);
            SFX.setVolume(SFXVolume);
            Master.setVolume(MasterVolume);
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

    public void GameControlsButton()
    {
        mainPauseMenuPanel.SetActive(false); // disable the main pause menu panel 
        controlsPanel.SetActive(true); //enable the controls panel
    }

    public void BackButton()
    {
        if (pv.IsMine && IsThisMultiplayer.Instance.multiplayer || !IsThisMultiplayer.Instance.multiplayer)
        {
            mainPauseMenuPanel.SetActive(true); //enable the main pause menu panel 
            audioSettingsPanel.SetActive(false); //disable the audio settings panel
            controlsPanel.SetActive(false); //disable the controls panel
        }
    }

    public void ResumeGameButton()
    {
        if (pv.IsMine && IsThisMultiplayer.Instance.multiplayer || !IsThisMultiplayer.Instance.multiplayer)
        {
            PauseMenu.SetActive(false); //close the pause menu
            paused = false;
        }   
    }
    #endregion

    #region Sliders
    public void SetMasterLevel(float sliderValue)
    {
        if (pv.IsMine && IsThisMultiplayer.Instance.multiplayer || !IsThisMultiplayer.Instance.multiplayer)
            MasterVolume = sliderValue;
    }

    public void SetMusicLevel(float sliderValue)
    {
        if (pv.IsMine && IsThisMultiplayer.Instance.multiplayer || !IsThisMultiplayer.Instance.multiplayer)
            MusicVolume = sliderValue;
    }

    public void SetSFXLevel(float sliderValue)
    {
        if (pv.IsMine && IsThisMultiplayer.Instance.multiplayer || !IsThisMultiplayer.Instance.multiplayer)
            SFXVolume = sliderValue;
    }
    #endregion
}
