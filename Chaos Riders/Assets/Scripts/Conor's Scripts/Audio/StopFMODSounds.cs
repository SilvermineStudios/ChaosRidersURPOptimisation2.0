using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD;
using FMODUnity;

public class StopFMODSounds : MonoBehaviour
{
    private FMOD.Studio.Bus MasterBus;

    private void Start()
    {
        MasterBus = FMODUnity.RuntimeManager.GetBus("Bus:/");
    }

    public void StopAllSounds(float time)
    {
        if(time <= 0)
            MasterBus.stopAllEvents(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        else
            StartCoroutine(StopSoundsCorotine(time));

        //MasterBus.stopAllEvents(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    private IEnumerator StopSoundsCorotine(float time)
    {
        yield return new WaitForSeconds(time);

        MasterBus.stopAllEvents(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }
}
