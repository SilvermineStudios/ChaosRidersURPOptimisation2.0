using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayFmodSound : MonoBehaviour
{
    public string EventLocation;
    FMOD.Studio.EventInstance eventInstance;

    public bool playOnAwake = true;

    void Awake()
    {
        eventInstance = FMODUnity.RuntimeManager.CreateInstance(EventLocation);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(eventInstance, this.transform.root.transform, this.transform.root.GetComponent<Rigidbody>());

        if(playOnAwake)
            eventInstance.start();
    }

    // Update is called once per frame
    void Update()
    {
        if(!playOnAwake)
        {
            
        }
    }
}
