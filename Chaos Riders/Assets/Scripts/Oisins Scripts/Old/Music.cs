using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Music : MonoBehaviour
{
    public TMP_Dropdown trackSelector;

    FMOD.Studio.EventInstance track1;
    FMOD.Studio.EventInstance track2;
    FMOD.Studio.EventInstance track3;
    FMOD.Studio.EventInstance track4;

    public string track1Ref = "event:/Music/MainTrack";
    public string track2Ref = "event:/Music/MainTrack 2";
    public string track3Ref = "event:/Music/MainTrack 3";
    public string track4Ref = "event:/Music/MainTrack 4";

    public bool track1paused = false;
    public bool track2paused = false;
    public bool track3paused = false;
    public bool track4paused = false;

    private void Start()
    {
        track1 = FMODUnity.RuntimeManager.CreateInstance(track1Ref);
        track2 = FMODUnity.RuntimeManager.CreateInstance(track2Ref);
        track3 = FMODUnity.RuntimeManager.CreateInstance(track3Ref);
        track4 = FMODUnity.RuntimeManager.CreateInstance(track4Ref);

        PickRandomTrack();
    }

    public void PickRandomTrack()
    {
        int randomNumber = Random.Range(1, 5);

        if (randomNumber == 1)
        {
            PlayTrack1();
            trackSelector.value = 0;
        }

        if(randomNumber == 2)
        {
            PlayTrack2();
            trackSelector.value = 1;
        }

        if (randomNumber == 3)
        {
            PlayTrack3();
            trackSelector.value = 2;
        }

        if (randomNumber == 4)
        {
            PlayTrack4();
            trackSelector.value = 3;
        }
    }

    public void PlayMusic(int val)
    {
        //track 1
        if(val == 0)
            PlayTrack1();

        //track 2
        if (val == 1)
            PlayTrack2();

        //track 3
        if (val == 2)
            PlayTrack3();

        //track 4
        if (val == 3)
            PlayTrack4();
    }

    void PlayTrack1()
    {
        track1.setPaused(false);
        track2.setPaused(true);
        track3.setPaused(true);
        track4.setPaused(true);

        track1paused = false;
        track2paused = true;
        track3paused = true;
        track4paused = true;

        track1.start();
    }

    void PlayTrack2()
    {
        track1.setPaused(true);
        track2.setPaused(false);
        track3.setPaused(true);
        track4.setPaused(true);

        track1paused = true;
        track2paused = false;
        track3paused = true;
        track4paused = true;

        track2.start();
    }

    void PlayTrack3()
    {
        track1.setPaused(true);
        track2.setPaused(true);
        track3.setPaused(false);
        track4.setPaused(true);

        track1paused = true;
        track2paused = true;
        track3paused = false;
        track4paused = true;

        track3.start();
    }

    void PlayTrack4()
    {
        track1.setPaused(true);
        track2.setPaused(true);
        track3.setPaused(true);
        track4.setPaused(false);

        track1paused = true;
        track2paused = true;
        track3paused = true;
        track4paused = false;

        track4.start();
    }
}
