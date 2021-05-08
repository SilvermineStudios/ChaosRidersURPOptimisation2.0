using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFX : MonoBehaviour
{
    public string buttonSound = "event:/SoundFX/ButtonSound";
    public string changeSoundEffectVolume = "event:/SoundFX/SoundEffectSound";
    public string startGameButtonSound = "event:/SoundFX/StartGameSound";

    bool canPlaySound = true;

    public void PlayButtonSound()
    {
        FMODUnity.RuntimeManager.PlayOneShot(buttonSound, this.transform.position);
    }

    public void PlayStartButtonSound()
    {
        FMODUnity.RuntimeManager.PlayOneShot(startGameButtonSound, this.transform.position);
    }


    #region sound effect bar sound
    public void PlayChangeSoundEffectSound()
    {
        if(canPlaySound)
        {
            FMODUnity.RuntimeManager.PlayOneShot(changeSoundEffectVolume, this.transform.position);
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
    #endregion
}
