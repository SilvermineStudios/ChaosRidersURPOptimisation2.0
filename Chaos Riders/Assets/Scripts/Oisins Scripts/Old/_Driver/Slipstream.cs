using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slipstream : MonoBehaviour
{
    [SerializeField] private bool onlineCar = true;

    Controller playerController;
    OfflineController offlinePlayerController;
    public TrailRenderer trL,trR;


    void Start()
    {
        if (onlineCar)
            playerController = GetComponent<Controller>();
        else
            offlinePlayerController = GetComponent<OfflineController>();

        trL.enabled = false;
        trR.enabled = false;
    }

    int counter = 0;
    void Update()
    {
        if(onlineCar)
        {
            if (playerController.currentSpeed > 70)
            {
                trL.enabled = true;
                trR.enabled = true;
            }
            else
            {
                trL.enabled = false;
                trR.enabled = false;
            }
        }
        else
        {
            if (offlinePlayerController.currentSpeed > 70)
            {
                trL.enabled = true;
                trR.enabled = true;
            }
            else
            {
                trL.enabled = false;
                trR.enabled = false;
            }
        }
    }
}
