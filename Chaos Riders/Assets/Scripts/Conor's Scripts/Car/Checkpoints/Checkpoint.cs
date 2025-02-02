﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.UI;

public class Checkpoint : MonoBehaviour
{
    [Header("General")]
    [SerializeField] bool isAI;
    private PhotonView pv;
    Rigidbody rb;

    [Header("Laps")]
    private int amountOfLaps;
    [SerializeField] private TMP_Text lapsText;
    public int currentLap { get; private set; }

    [Header("Finished Race Stuff")]
    [SerializeField] private GameObject playerCanvas;
    [SerializeField] private bool canCrossFinish = false;
    public bool youFinishedTheRace = false;
    private MeshRenderer[] meshRenderers;
    private Collider[] colliders;
    private GameObject MainCamera;
    
    [Header("Resetting")]
    [SerializeField] private KeyCode resetKey = KeyCode.R;
    [SerializeField] GameObject resetBar;
    float resetChargeAmount;
    float resetTimer;
    bool isResetting;

    [Header("Checkpoints")]
    [SerializeField] private GameObject[] checkpoints;
    [SerializeField] private float currentCheckpoint = 0f;
    private float previousCheckpoint = -1f;
    public float distanceToNextCheckpoint { get; private set; }
    

    private void Awake()
    {
        MainCamera = GameObject.FindGameObjectWithTag("MainCamera");

        currentLap = 1;
        pv = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody>();

        meshRenderers = this.GetComponentsInChildren<MeshRenderer>();
        colliders = this.GetComponentsInChildren<Collider>();
    }

    private void Start()
    {
        
        //CarUIManager.youWinText.SetActive(false);

        //audioS = GetComponent<AudioSource>();

        amountOfLaps = LapCounter.AmountOfLaps;

        checkpoints = CheckpointManager.checkPoints;
        if (!isAI)
        {
            OnlyDisplayNextCheckpoint();
        }
    }
    GameObject gar;

    private void Update()
    {
        //update currentlap
        if (pv.IsMine && IsThisMultiplayer.Instance.multiplayer || !IsThisMultiplayer.Instance.multiplayer)
        {
            if(youFinishedTheRace)
            {
                Cursor.visible = true;
            }

            if(rb.isKinematic)
            {
                rb.isKinematic = false;
            }
            distanceToNextCheckpoint = Vector3.Distance(transform.position, checkpoints[(int)currentCheckpoint].transform.position);
            if (!isAI)
            {
                OnlyDisplayNextCheckpoint();
                if (Input.GetButtonDown("Select") && resetBar != null)
                {
                    isResetting = true;
                }
                if (Input.GetButtonUp("Select"))
                {
                    isResetting = false;

                }
                if (isResetting && previousCheckpoint != -1)
                {
                    StartCoroutine(UseEquipmentUI(1));
                }
                else if (resetBar != null)
                {
                    resetTimer = 0;
                    resetChargeAmount = 0;
                    resetBar.GetComponent<Image>().fillAmount = 0;
                    resetBar.transform.parent.gameObject.SetActive(false);
                }
            }
            //only let the player cross the finish line if they have gone throug the first check point
            if (currentCheckpoint == 1)
            {
                canCrossFinish = true;
            }

            if (!isAI)
            {
                //CarUIManager.lapsText.text = "Lap " + currentLap + " / " + amountOfLaps;
                lapsText.text = "Lap " + currentLap + " / " + amountOfLaps;
            }

            if (currentLap == amountOfLaps && canCrossFinish && currentCheckpoint == 0)
            {
                LapCounter.FinishLine.SetActive(true);
            }
        }
    }

    public void ResetPos()
    { 
        int chosen = Random.Range(0, 3);
        rb.velocity = Vector3.zero;
        transform.position = checkpoints[(int)previousCheckpoint].transform.GetChild(0).GetChild(chosen).GetChild(0).position;
        transform.rotation = checkpoints[(int)previousCheckpoint].transform.GetChild(0).GetChild(chosen).GetChild(0).rotation;
        //Debug.Log(checkpoints[(int)previousCheckpoint].transform.position + "ee" + transform.position);
        rb.isKinematic = true;
        resetTimer = 0;
    }


    IEnumerator UseEquipmentUI(float timeTomove)
    {
        var t = 0f;
        while (t < 1 && isResetting)
        {
            resetBar.transform.parent.gameObject.SetActive(true);
            t += Time.deltaTime / timeTomove;
            resetChargeAmount = Mathf.Lerp(0, 100, t);
            resetBar.GetComponent<Image>().fillAmount = resetChargeAmount / 100;

            if (resetChargeAmount == 100)
            {
                ResetPos();
                isResetting = false;
            }

            yield return null;
        }

    }


    private void OnTriggerEnter(Collider other)
    {
        if (pv.IsMine && IsThisMultiplayer.Instance.multiplayer || !IsThisMultiplayer.Instance.multiplayer)
        {
            if (other.gameObject.tag == "Checkpoint")
            {

                FMODUnity.RuntimeManager.PlayOneShotAttached("event:/Pickups/Checkpoint", gameObject);

                if (!isAI)
                {
                    other.gameObject.SetActive(false);
                }

                if (currentCheckpoint == checkpoints.Length) //if the car is at the last waypoint
                    currentCheckpoint = 0; //make the next waypoint the first waypoint
                else
                {
                    currentCheckpoint += 1;//if the current waypoint is not the last waypoint in the list then go to the next waypoint in the list
                    previousCheckpoint += 1;
                }
                if (currentCheckpoint == 1)
                {
                    previousCheckpoint = 0;
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
                FinishedTheRaceStuff();
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

    void MakeYourselfInvisible()
    {
        foreach (MeshRenderer mr in meshRenderers)
        {
            mr.enabled = false;
        }

        foreach(Collider col in colliders)
        {
            col.enabled = false;
        }
    }

    void FinishedTheRaceStuff()
    {
        youFinishedTheRace = true;

      

        MainCamera.GetComponent<Animator>().SetBool("Start", true);
        FinishLine.GameWon = true; //send a message to the finish line script that the game has been won
        playerCanvas.SetActive(false); //turn off the players UI
        GameObject.FindGameObjectWithTag("End").SendMessage("Activate");
        MakeYourselfInvisible();

        //turn off all of the players cameras
        foreach (GameObject go in this.GetComponent<MultiplayerCameraCheck>().cameras)
        {
            go.SetActive(false);
        }

        //used to disable ai car if it is connected to you
        if(this.GetComponent<Shooter>()) //if you are a shooter player type
        {
            GameObject car = this.GetComponent<MoveTurretPosition>().car; //get the car you are connected to
            
            if (car.tag == "Player") //check if the car is an ai car
            {
                MeshRenderer[] carMeshes = car.GetComponentsInChildren<MeshRenderer>();
                Collider[] carColliders = car.GetComponentsInChildren<Collider>();

                foreach(MeshRenderer mr in carMeshes)
                {
                    mr.enabled = false; //disable all of the mesh renderers on the ai car
                }
                foreach(Collider col in carColliders)
                {
                    col.enabled = false; //disable all of the colliders on the ai car
                }
            }
        }
        
    }
}
