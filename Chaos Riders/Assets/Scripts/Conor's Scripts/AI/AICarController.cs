﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AICarController : MonoBehaviour
{
    [SerializeField] private float downforce = 800f;
    public Rigidbody rb;
    [SerializeField] private float maxMotorTorque = 1000f; // maximum torque that can be applied to the wheels
    public float currentSpeed { get; private set; }
    [SerializeField] private float maxSpeed = 2000f; //maximum speed the car can achieve
    [SerializeField] private float maxSteerAngle = 45f; //maximum angle the wheels can rotate +/-

    public int currentWaypoint = 0; //the current waypoint the car is moving towards
    private int nearestWaypoint = 0; //the nearest waypoint to the car
    [SerializeField] private float changeWaypointDistance = 45f; // the distance the car needs to be from its current waypoint before it changes to the next waypoint

    public int currentLap { get; private set; }

    [SerializeField] private WheelCollider FL, FR; // the front tire wheel colliders

    [SerializeField] public Transform[] waypoints;

    [SerializeField] private Vector3 centerOfMass;

    //private AIHealth healthScript;
    private Target healthScript;
    public GameObject healthBar;

    //public TurretTester ShooterAttached;

    private NavMeshAgent navMeshAgent;

    public GameObject Shooter;
    [SerializeField] private float timeBeforeCarFreezes = 1f; //the amount of time before the ai car freezes in place at the start line

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        currentLap = 1;
        GetComponent<Rigidbody>().centerOfMass = centerOfMass;
        //healthScript = GetComponent<AIHealth>();
        healthScript = GetComponent<Target>();

        nearestWaypoint = NearestWP();
        currentWaypoint = nearestWaypoint;

        waypoints = AIWaypointManager.Waypoints;
    }

    private void FixedUpdate()
    {
        //if dead bring to a stop and dont steer or update waypoints
        if (healthScript.isDead) { Die(); return; }

        // if Countdown for race start hasn't finished, dont move
        if (!MasterClientRaceStart.Instance.countdownTimerStart)
            StartCoroutine(SpawnCourotine(timeBeforeCarFreezes)); //makes the rb kinematic so the car doesnt roll at the start of the race
        else
            rb.isKinematic = false;
        // if Countdown for race start hasn't finished, dont move
        if (!MasterClientRaceStart.Instance.countdownTimerStart) { return; }

        ApplySteer();
        Drive();
        CheckWaypointDistance();
        AddDownForce();
        //NavMeshMoveToWaypoint();
    }

    private void NavMeshMoveToWaypoint()
    {
        if(currentWaypoint > -1 && navMeshAgent != null)
        {
            navMeshAgent.SetDestination(waypoints[currentWaypoint].position);
        }
    }


    private void AddDownForce()
    {
        rb.AddForce(-transform.up * downforce * rb.velocity.magnitude);
    }

    private void Die()
    {
        //stops car from moving
        FL.brakeTorque = 1000f;
        FR.brakeTorque = 1000f;
    }


    //steers the car towards the next waypoint
    private void ApplySteer()
    {
        Vector3 reletiveVector = transform.InverseTransformPoint(waypoints[currentWaypoint].position); //get the vector between the car and the nextwaypoint
        float newSteer = (reletiveVector.x / reletiveVector.magnitude) * maxSteerAngle; //generates the angle required for the wheels to turn
        FL.steerAngle = newSteer; //apply the angle to the Front Left Wheel
        FR.steerAngle = newSteer; //apply the angle to the Front Right Wheel
    }

    //moves the car
    private void Drive()
    {
        //allows car to move
        FL.brakeTorque = 0f;
        FR.brakeTorque = 0f;

        currentSpeed = 2 * Mathf.PI * FL.radius * FL.rpm * 60 / 100; //displays current speed based on how fast the wheel is spinning

        //if the car is not at its max speed yet speed up
        if (currentSpeed < maxSpeed)
        {
            FR.motorTorque = maxMotorTorque;
            FL.motorTorque = maxMotorTorque;
        }
        else //if the car is at its max speed stop applying power to the wheels
        {
            FR.motorTorque = 0;
            FL.motorTorque = 0;
        }
    }

    //calculates the distance between the car and the nextwaypoint
    private void CheckWaypointDistance()
    {

        if (Vector3.Distance(transform.position, waypoints[currentWaypoint].position) < changeWaypointDistance) //if the car is within the changewaypointDistance of the currentwaypoint
        {
            if (currentWaypoint == waypoints.Length - 1)
            {//if the car is at the last waypoint
                currentWaypoint = 0; //make the next waypoint the first waypoint
                currentLap++;
            }
            else
                currentWaypoint++;//if the current waypoint is not the last waypoint in the list then go to the next waypoint in the list
        }
    }


    //loop through each waypoint and calculate the nearest one to the ai car
    //used for spawning in an ai car to replace a disconnected driver; ai car wont move towards the first waypoint at the start line it will move to the nearest one to itself
    int NearestWP()
    {
        int nearestWP = 0;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = this.transform.position;

        for (int i = 0; i < waypoints.Length; i++)
        {
            Vector3 directionToTarget = waypoints[i].position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr && i != nearestWaypoint)
            {
                closestDistanceSqr = dSqrToTarget;
                nearestWP = i;
            }
        }
        return nearestWP;
    }
    
    /// /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void ResetPosition(float range, float yOffset)
    {
        if (currentWaypoint == 0)
        {
            //this.transform.position = waypoints[currentWaypoint].transform.position;

            Vector3 waypointPos = waypoints[currentWaypoint].transform.position;
            this.transform.position = new Vector3(waypointPos.x + Random.Range(0, range), waypointPos.y + yOffset, waypointPos.z + Random.Range(0, range));

            this.transform.rotation = waypoints[currentWaypoint].transform.rotation;
            currentWaypoint++;
        }
        else
        {
            //this.transform.position = waypoints[currentWaypoint - 1].transform.position;

            Vector3 waypointPos = waypoints[currentWaypoint - 1].transform.position;
            this.transform.position = new Vector3(waypointPos.x + Random.Range(0, range), waypointPos.y + yOffset, waypointPos.z + Random.Range(0, range));

            this.transform.rotation = waypoints[currentWaypoint - 1].transform.rotation;
        }
    }
    /// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "shooter")
        {
            Shooter = other.gameObject;
        }
    }

    public IEnumerator SpawnCourotine(float time)
    {
        yield return new WaitForSeconds(time);
        rb.isKinematic = true;
    }
}
