using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    //Audio
    FMOD.Studio.Bus Music;
    FMOD.Studio.Bus SFX;
    FMOD.Studio.Bus Master;

    //Pause Menu Gameobject 
    [SerializeField] private GameObject PauseMenu, audioSettingsPanel, mainPauseMenuPanel, controlsPanel; 
    [SerializeField] private KeyCode pauseMenuButton1, pauseMenuButton2;
    [SerializeField] Slider MasterSlider;
    [SerializeField] Slider MusicSlider;
    [SerializeField] Slider SFXSlider;
    [SerializeField] Toggle saveChanges;

    float timeSinceLastPush = 0;
    public bool paused = false;
    public bool saveChangesToAudio;
    [SerializeField] private int LobbySceneIndex = 0;
    private PhotonView pv;

    void Start()
    {
        pv = GetComponent<PhotonView>();
        PauseMenu.SetActive(false); //deactivate pause menu
        Music = FMODUnity.RuntimeManager.GetBus("bus:/Master/Music");
        SFX = FMODUnity.RuntimeManager.GetBus("bus:/Master/SFX");
        Master = FMODUnity.RuntimeManager.GetBus("bus:/Master");
        if (!PlayerPrefs.HasKey("MasterVolume"))
        {
            PlayerPrefs.SetFloat("MasterVolume", 1);
            PlayerPrefs.SetFloat("SFXVolume", 0.5f);
            PlayerPrefs.SetFloat("MusicVolume", 0.5f);
            PlayerPrefs.SetInt("SaveChanges", 1);
        }
        else
        {
            MasterSlider.value = PlayerPrefs.GetFloat("MasterVolume");
            MusicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
            SFXSlider.value = PlayerPrefs.GetFloat("SFXVolume");
            if(PlayerPrefs.GetInt("SaveChanges") == 1)
            {
                saveChanges.isOn = true;
                saveChangesToAudio = true;
            }
            else
            {
                saveChanges.isOn = false;
                saveChangesToAudio = false;
            }
        }

        // AUDIO SETTINGS
        Music.setVolume(PlayerPrefs.GetFloat("MusicVolume"));
        SFX.setVolume(PlayerPrefs.GetFloat("SFXVolume"));
        Master.setVolume(PlayerPrefs.GetFloat("MasterVolume"));
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
    
    public void SaveChangesToAudio()
    {
        if (pv.IsMine && IsThisMultiplayer.Instance.multiplayer || !IsThisMultiplayer.Instance.multiplayer && Time.timeSinceLevelLoad > 5)
        {
            timeSinceLastPush = Time.time;
            saveChangesToAudio = saveChanges.isOn;
            if(saveChangesToAudio)
            {
                PlayerPrefs.SetFloat("MasterVolume", MasterSlider.value);
                PlayerPrefs.SetFloat("SFXVolume", SFXSlider.value);
                PlayerPrefs.SetFloat("MusicVolume", MusicSlider.value);
            }
            if (PlayerPrefs.GetInt("SaveChanges") == 1)
            {
                PlayerPrefs.SetInt("SaveChanges", 0);
            }
            else
            {
                PlayerPrefs.SetInt("SaveChanges", 1);
            }
        }
    }

    #endregion

    #region Sliders
    public void SetMasterLevel(float sliderValue)
    {
        if (pv.IsMine && IsThisMultiplayer.Instance.multiplayer || !IsThisMultiplayer.Instance.multiplayer)
        {
            if(saveChangesToAudio)
            {
                PlayerPrefs.SetFloat("MasterVolume", sliderValue);
            }

            Master.setVolume(sliderValue);
            
        }
            
    }

    public void SetMusicLevel(float sliderValue)
    {
        if (pv.IsMine && IsThisMultiplayer.Instance.multiplayer || !IsThisMultiplayer.Instance.multiplayer)
        {
            if (saveChangesToAudio)
            {
                PlayerPrefs.SetFloat("MusicVolume", sliderValue);
            }


             Music.setVolume(sliderValue);
            
        }
    }

    public void SetSFXLevel(float sliderValue)
    {
        if (pv.IsMine && IsThisMultiplayer.Instance.multiplayer || !IsThisMultiplayer.Instance.multiplayer)
        {
            if (saveChangesToAudio)
            {
                PlayerPrefs.SetFloat("SFXVolume", sliderValue);
            }
             SFX.setVolume(sliderValue);
              
        }
    }
    #endregion
}
