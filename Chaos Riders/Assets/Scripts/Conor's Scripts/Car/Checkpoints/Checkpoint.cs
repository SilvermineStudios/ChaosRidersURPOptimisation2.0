using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.UI;

public class Checkpoint : MonoBehaviour
{
    public float distanceToNextCheckpoint { get; private set; }

    [SerializeField] private AudioClip soundEffect;
    [SerializeField] GameObject resetBar;
    float resetChargeAmount;
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
    bool isResetting;
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
            distanceToNextCheckpoint = Vector3.Distance(transform.position, checkpoints[(int)currentCheckpoint].transform.position);

            OnlyDisplayNextCheckpoint();
            if(Input.GetKeyDown(resetKey) && resetBar != null)
            {
                isResetting = true;
            }
            if(Input.GetKeyUp(resetKey))
            {
                isResetting = false;

            }
            if (isResetting && previousCheckpoint != -1)
            {
                StartCoroutine(UseEquipmentUI(3));
            }
            else if (resetBar != null)
            {
                resetTimer = 0;
                resetChargeAmount = 0;
                resetBar.GetComponent<Image>().fillAmount = 0;
                resetBar.transform.parent.gameObject.SetActive(false);
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

    private void ResetPos()
    { 
        int chosen = Random.Range(0, 5);
        rb.velocity = Vector3.zero;
        transform.position = checkpoints[(int)previousCheckpoint].transform.GetChild(0).GetChild(chosen).GetChild(0).position;
        transform.rotation = checkpoints[(int)previousCheckpoint].transform.GetChild(0).GetChild(chosen).GetChild(0).rotation;
        //Debug.Log(checkpoints[(int)previousCheckpoint].transform.position + "ee" + transform.position);
        rb.velocity = Vector3.zero;
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
