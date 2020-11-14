using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Checkpoint : MonoBehaviour
{
    //script for what happens when a player drives through a checkpoint
    [SerializeField] private GameObject[] checkpoints;
    [SerializeField] private float currentCheckpoint = 0f;

    [SerializeField] private bool canCollect = true;

    [SerializeField] private PhotonView pv;

    private void Start()
    {
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
    }

    private void OnTriggerExit(Collider other)
    {
        //canCollect = true;
    }



    private void Update()
    {
        OnlyDisplayNextCheckpoint();
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

        if (currentCheckpoint == checkpoints.Length) //if the car is at the last waypoint
            currentCheckpoint = 0; //make the next waypoint the first waypoint
    }

}
