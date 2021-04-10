using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFX : MonoBehaviour
{
    public string buttonSoundRef = "event:/SoundFX/ButtonSound";
    public string changeSoundEffectVolumeRef = "event:/SoundFX/SoundEffectSound";

    public void PlayButtonSound()
    {
        FMODUnity.RuntimeManager.PlayOneShot(buttonSoundRef, this.transform.position);
    }

    public void PlayChangeSoundEffectSound()
    {
        FMODUnity.RuntimeManager.PlayOneShot(changeSoundEffectVolumeRef, this.transform.position);
    }
}
