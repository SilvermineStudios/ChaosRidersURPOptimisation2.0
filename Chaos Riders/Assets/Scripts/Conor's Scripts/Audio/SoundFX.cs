using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFX : MonoBehaviour
{
    public string buttonSoundRef = "event:/SoundFX/ButtonSound";
    public string changeSoundEffectVolumeRef = "event:/SoundFX/SoundEffectSound";

    bool canPlaySound = true;

    public void PlayButtonSound()
    {
        FMODUnity.RuntimeManager.PlayOneShot(buttonSoundRef, this.transform.position);
    }

    public void PlayChangeSoundEffectSound()
    {
        if(canPlaySound)
        {
            FMODUnity.RuntimeManager.PlayOneShot(changeSoundEffectVolumeRef, this.transform.position);
            canPlaySound = false;
        }
        else
            StartCoroutine(PlaySoundEffectSoundCorotine(0.5f));
    }

    private IEnumerator PlaySoundEffectSoundCorotine(float time)
    {
        yield return new WaitForSeconds(time);

        canPlaySound = true;
    }

}
