using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    [SerializeField] private bool drawLines = true;
    [SerializeField] private bool drawSpheres = true;
    [SerializeField] private Color lineColour;
    [SerializeField] private float sphereRadius = 0.3f;

    [SerializeField] private Transform[] waypointLocations;
    [SerializeField] private Transform[] waypoints;


    [SerializeField] private GameObject checkpointPrefab;
    [SerializeField] private float yOffset = -6f;




    private void Awake()
    {
        AddCarWaypoints();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = lineColour;

        waypointLocations = new Transform[this.transform.childCount]; //make the array the same length as the amount of children waypoints
        waypoints = new Transform[waypointLocations.Length]; // set the amount of waypoints as the same size of the amount of waypoint locations

        for (int i = 0; i < this.transform.childCount; i++) //put every waypoint(Child) in the array
        {
            waypointLocations[i] = transform.GetChild(i);
        }

        DrawLinesAndBalls();
    }

    private void DrawLinesAndBalls()
    {
        //draw a line between all the waypoints
        for (int i = 0; i < waypointLocations.Length; i++)
        {
            Vector3 currentWaypoint = waypointLocations[i].position;
            Vector3 previosWaypoint = Vector3.zero;

            //calculate the previous waypoint
            if (i > 0)
                previosWaypoint = waypointLocations[i - 1].position;
            else if (i == 0 && waypointLocations.Length > 1)
                previosWaypoint = waypointLocations[waypointLocations.Length - 1].position;

            if (drawLines)
                Gizmos.DrawLine(previosWaypoint, currentWaypoint);
            if (drawSpheres)
                Gizmos.DrawSphere(currentWaypoint, sphereRadius);
        }
    }

    //spawns the waypoint prefab at each waypoint
    private void AddCarWaypoints()
    {
        //spawns the waypoint prefab at each waypoint
        for (int i = 0; i < waypointLocations.Length; i++)
        {
            Vector3 spawnPos = new Vector3(waypointLocations[i].position.x, waypointLocations[i].position.y + yOffset, waypointLocations[i].position.z);
            waypoints[i] = Instantiate(checkpointPrefab, spawnPos, waypointLocations[i].rotation).transform;
        }
    }


    private void CalculateCurrentWaypoint()
    {

    }


    private void Update()
    {

    }
}
