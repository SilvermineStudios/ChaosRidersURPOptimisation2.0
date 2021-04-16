using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.UI;

public class AIResetPos : MonoBehaviour
{
    private PhotonView pv;
    Rigidbody rb;

    public float zVelocity;
    [SerializeField] private float tooSlowAmount = 0.5f;
    [SerializeField] private float maxTimeToBeStuck = 5f;
    [SerializeField] private bool checkingIfTooSlow = false;

    [Header("Reset Stuff")]
    [SerializeField] GameObject resetBar;
    [SerializeField] private KeyCode resetKey = KeyCode.R;
    [SerializeField] private bool stuck = false;
    float resetChargeAmount;
    float resetTimer;
    bool isResetting;

    [Header ("Checkpoint / Lap Stuff")]
    [SerializeField] private int currentLap = 1;
    [SerializeField] private TMP_Text lapsText;
    [SerializeField] private GameObject[] checkpoints;
    [SerializeField] private float currentCheckpoint = 0f;
    private int amountOfLaps;
    private float previousCheckpoint = -1f;
    private bool canCrossFinish = false;

    [Header("Moving Stuff")]
    [SerializeField] private bool isMoving;
    private Transform objectTransfom;
    private float noMovementThreshold = 0.0001f;
    private const int noMovementFrames = 3;
    Vector3[] previousLocations = new Vector3[noMovementFrames];
    public bool IsMoving  //Let other scripts see if the object is moving
    {
        get { return isMoving; }
    }



    private void Awake()
    {
        pv = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody>();
        objectTransfom = this.transform;

        //For good measure, set the previous locations
        for (int i = 0; i < previousLocations.Length; i++)
        {
            previousLocations[i] = Vector3.zero;
        }
    }

    private void Start()
    {
        amountOfLaps = LapCounter.AmountOfLaps;

        checkpoints = CheckpointManager.checkPoints;

        OnlyDisplayNextCheckpoint();
    }

    

    private void Update()
    {
        zVelocity = rb.velocity.z;

        if (zVelocity <= tooSlowAmount && !checkingIfTooSlow)
        {
            Debug.Log("Not Moving");
            StartCoroutine(CarNotMovingCheck(maxTimeToBeStuck));
            checkingIfTooSlow = true;
        }

        



        //update currentlap
        if (pv.IsMine && IsThisMultiplayer.Instance.multiplayer || !IsThisMultiplayer.Instance.multiplayer)
        {
            if (rb.isKinematic)
            {
                rb.isKinematic = false;
            }

            OnlyDisplayNextCheckpoint();


            if (Input.GetKeyDown(resetKey) && resetBar != null)
            {
                isResetting = true;
            }
            if (Input.GetKeyUp(resetKey))
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

            //only let the player cross the finish line if they have gone throug the first check point
            if (currentCheckpoint == 1)
            {
                canCrossFinish = true;
            }


            //CarUIManager.lapsText.text = "Lap " + currentLap + " / " + amountOfLaps;
            lapsText.text = "Lap " + currentLap + " / " + amountOfLaps;


            if (currentLap == amountOfLaps && canCrossFinish && currentCheckpoint == 0)
            {
                LapCounter.FinishLine.SetActive(true);
            }
        }
    }


    private IEnumerator CarNotMovingCheck(float time)
    {
        Debug.Log("Starting...");

        yield return new WaitForSeconds(time);

        checkingIfTooSlow = false;

        //stuck
        if (zVelocity <= tooSlowAmount)
        {
            Debug.Log("Call Reset Shit here");
        }
        else //not stuck
        {
            Debug.Log("Not Stuck");
        }
    }




    private void ResetPos()
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

                other.gameObject.SetActive(false);

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
                currentLap++;
                Debug.Log("Crossed");
            }

            if (other.gameObject.tag == "FinishLine")// && !FinishLine.GameWon)
            {
                FinishLine.GameWon = true;
                Debug.Log("YOU WIN");
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
