using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private AudioClip soundEffect;

    private int amountOfLaps;
    [SerializeField] private int currentLap = 1;
    [SerializeField] private TMP_Text lapsText;
    [SerializeField] private KeyCode resetKey = KeyCode.R;
    float resetTimer;
    [SerializeField] private bool canCrossFinish = false; //remove from inspector <--------------------------------------------------

    //script for what happens when a player drives through a checkpoint
    [SerializeField] private GameObject[] checkpoints;
    [SerializeField] private float currentCheckpoint = 0f;
    private float previousCheckpoint = -1f;
    [SerializeField] private GameObject youWinText;

    [SerializeField] private bool canCollect = true;

    [SerializeField] GameObject Music;

    private PhotonView pv;
    Rigidbody rb;

    private void Awake()
    {
        pv = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        youWinText.SetActive(false);
        //CarUIManager.youWinText.SetActive(false);

        //audioS = GetComponent<AudioSource>();

        amountOfLaps = LapCounter.AmountOfLaps;

        checkpoints = CheckpointManager.checkPoints;

        OnlyDisplayNextCheckpoint();
    }


    private void Update()
    {
        //update currentlap
        if (pv.IsMine && IsThisMultiplayer.Instance.multiplayer || !IsThisMultiplayer.Instance.multiplayer)
        {
            OnlyDisplayNextCheckpoint();
            if(Input.GetKey(resetKey) && previousCheckpoint != -1)
            {
                if(resetTimer == 0)
                {
                    resetTimer = Time.time;
                }
                ResetPos();
            }
            else
            {
                resetTimer = 0;
            }

            //only let the player cross the finish line if they have gone throug the first check point
            if (currentCheckpoint == 1)
            {
                canCrossFinish = true;
            }


            //CarUIManager.lapsText.text = "Lap " + currentLap + " / " + amountOfLaps;
            lapsText.text = "Lap " + currentLap + " / " + amountOfLaps;


            //Start the music on the last lap
            if(currentLap == amountOfLaps && !Music.activeInHierarchy)
            {
                Music.SetActive(true);
            }

            if (currentLap == amountOfLaps && canCrossFinish && currentCheckpoint == 0)
            {
                LapCounter.FinishLine.SetActive(true);
            }
        }
    }

    Vector3 sdad;
    Quaternion asdadad;
    private void ResetPos()
    {
        Debug.Log((resetTimer + 3) - Time.time);
        if (Time.time >= resetTimer + 3)
        {
            transform.position = checkpoints[(int)previousCheckpoint].transform.position;
            Debug.Log(checkpoints[(int)previousCheckpoint].transform.position + "ee" + transform.position);
            sdad = new Vector3(checkpoints[(int)previousCheckpoint].transform.rotation.x, checkpoints[(int)previousCheckpoint].transform.rotation.y + 90, checkpoints[(int)previousCheckpoint].transform.rotation.z);
            asdadad.eulerAngles = sdad;
            transform.rotation = asdadad;
            rb.velocity = Vector3.zero;
            resetTimer = 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (pv.IsMine && IsThisMultiplayer.Instance.multiplayer || !IsThisMultiplayer.Instance.multiplayer)
        {
            if (other.gameObject.tag == "Checkpoint")
            {

                FMODUnity.RuntimeManager.PlayOneShotAttached("event:/Pickups/Checkpoint", gameObject);

                other.gameObject.SetActive(false);

                if (currentCheckpoint == checkpoints.Length) //if the car is at the last waypoint
                    currentCheckpoint = 0; //make the next waypoint the first waypoint
                else
                {
                    previousCheckpoint += 1;
                    if (previousCheckpoint > checkpoints.Length)
                    {
                        previousCheckpoint = 0;
                    }
                    currentCheckpoint += 1;//if the current waypoint is not the last waypoint in the list then go to the next waypoint in the list
                }
            }



            //only let the player cross the line if they have collected the first check point then gone through the rest of the checkpoints
            if (other.gameObject.tag == "StartLine" && canCrossFinish && currentCheckpoint == 0 && currentLap < amountOfLaps)
            {
                canCrossFinish = false;
                currentLap++;
                Debug.Log("Crossed");
            }

            if (other.gameObject.tag == "FinishLine")// && !FinishLine.GameWon)
            {
                //CarUIManager.youWinText.SetActive(true);
                youWinText.SetActive(true);
                FinishLine.GameWon = true;
                Debug.Log("YOU WIN");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //canCollect = true;
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
