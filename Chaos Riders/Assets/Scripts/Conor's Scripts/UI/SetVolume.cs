using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SetVolume : MonoBehaviour
{
    public AudioMixer mixer;

    public void SetMasterLevel(float sliderValue)
    {
        mixer.SetFloat("Master", Mathf.Log10(sliderValue) * 20);
        //mixer.SetFloat ("SFX", sliderValue);
    }

    public void SetMusicLevel(float sliderValue)
    {
        mixer.SetFloat("Music", Mathf.Log10(sliderValue) * 20);
    }

    public void SetSFXLevel(float sliderValue)
    {
        mixer.SetFloat ("SFX", Mathf.Log10(sliderValue) * 20);
    }
}
