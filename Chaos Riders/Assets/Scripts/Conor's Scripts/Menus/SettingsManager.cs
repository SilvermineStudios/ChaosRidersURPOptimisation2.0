using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using TMPro;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [Header("Audio Settings")]
    [SerializeField] private Slider MasterSlider;
    [SerializeField] private Slider MusicSlider;
    [SerializeField] private Slider SFXSlider;
    FMOD.Studio.Bus Music;
    FMOD.Studio.Bus SFX;
    FMOD.Studio.Bus Master;

    [Header("Video Settings")]
    [SerializeField] private TMP_Dropdown imageQualityDropDown;
    [SerializeField] private GameObject FPSText;
    public RenderPipelineAsset[] qualityLevels;

    private void Awake()
    {
        //audio
        Music = FMODUnity.RuntimeManager.GetBus("bus:/Master/Music");
        SFX = FMODUnity.RuntimeManager.GetBus("bus:/Master/SFX");
        Master = FMODUnity.RuntimeManager.GetBus("bus:/Master");

        MasterSlider.value = PlayerPrefs.GetFloat("MasterVolume");
        MusicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        SFXSlider.value = PlayerPrefs.GetFloat("SFXVolume");

        Music.setVolume(PlayerPrefs.GetFloat("MusicVolume"));
        SFX.setVolume(PlayerPrefs.GetFloat("SFXVolume"));
        Master.setVolume(PlayerPrefs.GetFloat("MasterVolume"));


        //video
        imageQualityDropDown.value = QualitySettings.GetQualityLevel() - 1; //REVERT IF USING VERY LOW SETTINGS
        //QualitySettings.renderPipeline = qualityLevels[QualitySettings.GetQualityLevel() - 1];
    }

    #region video
    public void ChangeRenderPipelineAsset(int value) //changes the video quality settings
    {
        //Debug.Log("Value is: " + value);
        //Debug.Log("Quality setting = " + qualityLevels[value].name);
        //QualitySettings.renderPipeline = qualityLevels[value];

        int test = value + 1;
        //QualitySettings.SetQualityLevel(value); 
        QualitySettings.SetQualityLevel(test); //REVERT IF USING VERY LOW SETTINGS
    }

    public void ToggleDisplayFPS(bool toggle)
    {
        if (toggle)
        {
            FPSText.SetActive(true);
        }
        else
        {
            FPSText.SetActive(false);
        }
    }
    #endregion

    #region Audio
    public void SetMasterLevel(float sliderValue)
    {
        PlayerPrefs.SetFloat("MasterVolume", sliderValue);
        Master.setVolume(sliderValue);
    }

    public void SetMusicLevel(float sliderValue)
    {
        PlayerPrefs.SetFloat("MusicVolume", sliderValue);
        Music.setVolume(sliderValue);
    }

    public void SetSFXLevel(float sliderValue)
    {
        PlayerPrefs.SetFloat("SFXVolume", sliderValue);
        SFX.setVolume(sliderValue);
    }
    #endregion
}
