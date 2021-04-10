using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [Header("Audio Settings")]
    [SerializeField] private Slider MasterSlider;
    [SerializeField] private Slider MusicSlider;
    [SerializeField] private Slider SFXSlider;

    [Header("Video Settings")]
    [SerializeField] private TMP_Dropdown imageQualityDropDown;
    [SerializeField] private GameObject FPSText;

    private void Awake()
    {
        imageQualityDropDown.value = QualitySettings.GetQualityLevel() - 1; //REVERT IF USING VERY LOW SETTINGS
    }

    public void ChangeRenderPipelineAsset(int value)
    {
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
}
