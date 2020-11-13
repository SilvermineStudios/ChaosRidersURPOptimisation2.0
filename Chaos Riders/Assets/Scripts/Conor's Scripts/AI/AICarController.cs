using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICarController : MonoBehaviour
{
    [SerializeField] private float maxMotorTorque = 80f; // maximum torque that can be applied to the wheels
    [SerializeField] private float currentSpeed; //cars current speed
    [SerializeField] private float maxSpeed = 100f; //maximum speed the car can achieve
    [SerializeField] private float maxSteerAngle = 45f; //maximum angle the wheels can rotate +/-

    [SerializeField] private int currentWaypoint = 0; //the current waypoin the car is moving towards
    [SerializeField] private float changeWaypointDistance = 45f; // the distance the car needs to be from its current waypoint before it changes to the next waypoint

    [SerializeField] private WheelCollider FL, FR; // the front tire wheel colliders
     
    [SerializeField] private AIWaypointManager waypointManager;
    [SerializeField] private Transform[] waypoints;

    [SerializeField] private Vector3 centerOfMass;
    

    private void Awake()
    {
        waypointManager = FindObjectOfType<AIWaypointManager>(); //find the waypoint manager
    }

    void Start()
    {
        waypoints = new Transform[waypointManager.waypoints.Length];
        waypoints = waypointManager.waypoints;

        GetComponent<Rigidbody>().centerOfMass = centerOfMass;
    }

    private void FixedUpdate()
    {
        ApplySteer();
        Drive();
        CheckWaypointDistance();
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
        if(Vector3.Distance(transform.position, waypoints[currentWaypoint].position) < changeWaypointDistance) //if the car is within the changewaypointDistance of the currentwaypoint
        {
            if (currentWaypoint == waypoints.Length - 1) //if the car is at the last waypoint
                currentWaypoint = 0; //make the next waypoint the first waypoint
            else
                currentWaypoint ++;//if the current waypoint is not the last waypoint in the list then go to the next waypoint in the list
        }
    }

}
