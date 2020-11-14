﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class Checkpoint : MonoBehaviour
{
    private int amountOfLaps;
    [SerializeField] private int currentLap = 1;

    [SerializeField] private bool canCrossFinish = false; //remove from inspector <--------------------------------------------------

    //script for what happens when a player drives through a checkpoint
    [SerializeField] private GameObject[] checkpoints;
    [SerializeField] private float currentCheckpoint = 0f;

    [SerializeField] private bool canCollect = true;

    [SerializeField] private PhotonView pv;

    private void Start()
    {
        amountOfLaps = LapCounter.AmountOfLaps;

        //pv = GetComponent<PhotonView>();
        checkpoints = CheckpointManager.checkPoints;

        OnlyDisplayNextCheckpoint();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Checkpoint")
        {
            if(pv.IsMine && IsThisMultiplayer.Instance.multiplayer)
            {
                other.gameObject.SetActive(false);

                if (currentCheckpoint == checkpoints.Length) //if the car is at the last waypoint
                    currentCheckpoint = 0; //make the next waypoint the first waypoint
                else
                    currentCheckpoint += 1;//if the current waypoint is not the last waypoint in the list then go to the next waypoint in the list
            }

            if(!IsThisMultiplayer.Instance.multiplayer)
            {
                other.gameObject.SetActive(false);

                if (currentCheckpoint == checkpoints.Length) //if the car is at the last waypoint
                    currentCheckpoint = 0; //make the next waypoint the first waypoint
                else
                    currentCheckpoint += 1;//if the current waypoint is not the last waypoint in the list then go to the next waypoint in the list
            }
        }


        //only let the player cross the line if they have collected the first check point then gone through the rest of the checkpoints
        if (other.gameObject.tag == "StartLine" && canCrossFinish && currentCheckpoint == 0 && currentLap < amountOfLaps)
        {
            canCrossFinish = false;
            currentLap ++;
            Debug.Log("Crossed");
        }

        if(other.gameObject.tag == "FinishLine")// && !FinishLine.GameWon)
        {
            LapCounter.YouWinText.SetActive(true);
            FinishLine.GameWon = true;
            Debug.Log("YOU WIN");
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //canCollect = true;
    }



    private void Update()
    {
        //update currentlap
        if (pv.IsMine && IsThisMultiplayer.Instance.multiplayer)
        {
            OnlyDisplayNextCheckpoint();

            //only let the player cross the finish line if they have gone throug the first check point
            if (currentCheckpoint == 1)
            {
                canCrossFinish = true;
            }


            LapCounter.lapsText.text = "Lap " + currentLap + " / " + amountOfLaps;
            if(currentLap == amountOfLaps && canCrossFinish && currentCheckpoint == 0)
            {
                LapCounter.FinishLine.SetActive(true);
            }
        }

        if (!IsThisMultiplayer.Instance.multiplayer)
        {
            OnlyDisplayNextCheckpoint();

            //only let the player cross the finish line if they have gone throug the first check point
            if (currentCheckpoint == 1)
            {
                canCrossFinish = true;
            }


            LapCounter.lapsText.text = "Lap " + currentLap + " / " + amountOfLaps;
            if (currentLap == amountOfLaps && canCrossFinish && currentCheckpoint == 0)
            {
                LapCounter.FinishLine.SetActive(true);
            }
        }
    }


    void OnlyDisplayNextCheckpoint()
    {
        for (int i = 0; i < checkpoints.Length; i++)
        {
            if (currentCheckpoint == i)
                checkpoints[i].SetActive(true);
            else
                checkpoints[i].SetActive(false);
        }

        if (currentCheckpoint == checkpoints.Length - 1) //if the car is at the last waypoint
            currentCheckpoint = 0; //make the next waypoint the first waypoint
    }
}
