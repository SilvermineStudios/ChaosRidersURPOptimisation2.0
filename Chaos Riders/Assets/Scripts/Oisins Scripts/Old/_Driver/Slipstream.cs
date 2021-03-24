using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slipstream : MonoBehaviour
{

    Controller playerController;
    public TrailRenderer trL,trR;


    void Start()
    {
        playerController = GetComponent<Controller>();

        trL.enabled = false;
        trR.enabled = false;
    }

    int counter = 0;
    void Update()
    {
        if(playerController.currentSpeed > 70)
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
