using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering;

public class Pause : MonoBehaviour
{
    private PhotonView pv;

    private int menuSceneIndex = 0;

    //Audio
    FMOD.Studio.Bus Music;
    FMOD.Studio.Bus SFX;
    FMOD.Studio.Bus Master;

    //Pause Menu Gameobject 
    private GameObject PauseMenu; 
    private GameObject mainPauseMenuPanel;
    private GameObject sliders;
    private GameObject audioSettingsPanel, videoSettingsPanel, controlsPanel;
    public List<GameObject> menuPanels;
    [SerializeField] private GameObject playerCanvas;

    //Audio stuff
    private Slider MasterSlider, MusicSlider, SFXSlider;
    private Toggle saveChanges;

    //video Stuff
    private GameObject videoSettingsHolder;
    private TMP_Dropdown imageQualityDropDown;
    [SerializeField] private GameObject FPSText;

    float timeSinceLastPush = 0; //dont delete
    [HideInInspector] public bool paused = false;
    private bool saveChangesToAudio;

    

    private void Awake()
    {
        PauseMenu = this.gameObject;
        pv = GetComponent<PhotonView>();

        mainPauseMenuPanel = PauseMenu.transform.Find("MainPauseMenu").gameObject;
        playerCanvas = this.transform.parent.gameObject;

        audioSettingsPanel = PauseMenu.transform.Find("Audio Settings").gameObject;
        videoSettingsPanel = PauseMenu.transform.Find("Video Settings").gameObject;
        controlsPanel = PauseMenu.transform.Find("Controls Panel").gameObject;
        menuPanels.Add(audioSettingsPanel);
        menuPanels.Add(videoSettingsPanel);
        menuPanels.Add(controlsPanel);

        //audio
        sliders = audioSettingsPanel.transform.Find("Sliders").gameObject;
        MasterSlider = sliders.transform.Find("Master Volume Slider").GetComponent<Slider>();
        MusicSlider = sliders.transform.Find("Music Volume Slider").GetComponent<Slider>();
        SFXSlider = sliders.transform.Find("SFX Volume Slider").GetComponent<Slider>();
        saveChanges = sliders.transform.Find("SaveChanges").GetComponent<Toggle>();

        //video
        videoSettingsHolder = videoSettingsPanel.transform.Find("Settings Stuff").gameObject;
        imageQualityDropDown = videoSettingsHolder.transform.Find("Video Quality Dropdown").GetComponent<TMP_Dropdown>();
        imageQualityDropDown.value = QualitySettings.GetQualityLevel();
        FPSText = playerCanvas.transform.Find("FPSText").gameObject;
    }

    void Start()
    {
        //PauseMenu.SetActive(false); //deactivate pause menu
        Music = FMODUnity.RuntimeManager.GetBus("bus:/Master/Music");
        SFX = FMODUnity.RuntimeManager.GetBus("bus:/Master/SFX");
        Master = FMODUnity.RuntimeManager.GetBus("bus:/Master");
        if (!PlayerPrefs.HasKey("MasterVolume") || !PlayerPrefs.HasKey("MusicVolume") || !PlayerPrefs.HasKey("SFXVolume"))
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

    }

    #region Video Settings
    public void VideoSettingsButton()
    {
        mainPauseMenuPanel.SetActive(false); // disable the main pause menu panel 
        videoSettingsPanel.SetActive(true); //enable the audio settings panel
    }

    public void ChangeRenderPipelineAsset(int value)
    {
        QualitySettings.SetQualityLevel(value);
    }

    public void ToggleDisplayFPS(bool toggle)
    {
        if(toggle)
        {

        }
        else
        {

        }
    }

    public void SaveChangesToVideo()
    {

    }

    #endregion

    #region Audio Settings
    public void AudioSettingsButton()
    {
        mainPauseMenuPanel.SetActive(false); // disable the main pause menu panel 
        audioSettingsPanel.SetActive(true); //enable the audio settings panel
    }

    public void SetMasterLevel(float sliderValue)
    {
        if (saveChangesToAudio)
        {
            PlayerPrefs.SetFloat("MasterVolume", sliderValue);
        }

        Master.setVolume(sliderValue);
    }

    public void SetMusicLevel(float sliderValue)
    {
        if (saveChangesToAudio)
        {
            PlayerPrefs.SetFloat("MusicVolume", sliderValue);
        }

        Music.setVolume(sliderValue);
    }

    public void SetSFXLevel(float sliderValue)
    {
        if (saveChangesToAudio)
        {
            PlayerPrefs.SetFloat("SFXVolume", sliderValue);
        }
        SFX.setVolume(sliderValue);
    }

    public void SaveChangesToAudio()
    {
        timeSinceLastPush = Time.time;
        saveChangesToAudio = saveChanges.isOn;
        if (saveChangesToAudio)
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
    #endregion

    #region General Buttons
    public void LeaveRaceButton()
    {
        StartCoroutine(DisconnectAndLoad());
    }
    IEnumerator DisconnectAndLoad()
    {
        PhotonNetwork.LeaveRoom();

        while(PhotonNetwork.InRoom)
            yield return null;

        SceneManager.LoadScene(menuSceneIndex);
    }

    public void QuitGameButton()
    {
        StartCoroutine(Disconnect());
    }
    IEnumerator Disconnect()
    {
        PhotonNetwork.LeaveRoom();

        while (PhotonNetwork.InRoom)
            yield return null;

        Application.Quit();
    }

    public void GameControlsButton()
    {
        mainPauseMenuPanel.SetActive(false); // disable the main pause menu panel 
        controlsPanel.SetActive(true); //enable the controls panel
    }

    public void BackButton()
    {
        mainPauseMenuPanel.SetActive(true); //enable the main pause menu panel 

        foreach(GameObject go in menuPanels)
        {
            go.SetActive(false);
        }
    }

    public void ResumeGameButton()
    {
        PauseMenu.SetActive(false); //close the pause menu
        paused = false;

        this.transform.root.GetComponent<PauseMenu>().paused = false;
        Cursor.visible = false;
    }
    #endregion
}
