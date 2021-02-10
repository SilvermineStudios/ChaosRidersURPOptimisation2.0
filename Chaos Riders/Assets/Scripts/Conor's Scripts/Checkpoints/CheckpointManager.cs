using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    

    [SerializeField] private bool drawLines = true;
    [SerializeField] private bool drawSpheres = true;
    [SerializeField] private Color lineColour;
    [SerializeField] private float sphereRadius = 0.3f;

    [SerializeField] private Transform[] checkPointLocations;
    [SerializeField] public static GameObject[] checkPoints;


    [SerializeField] private GameObject checkpointPrefab;
    [SerializeField] private float yOffset = -6f;

    private bool addToArray = false;




    private void Awake()
    {
        AddCarWaypoints();
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = lineColour;

        checkPointLocations = new Transform[this.transform.childCount]; //make the array the same length as the amount of children waypoints
        

        for (int i = 0; i < this.transform.childCount; i++) //put every waypoint(Child) in the array
        {
            checkPointLocations[i] = transform.GetChild(i);
        }

        DrawLinesAndBalls();
    }

    private void DrawLinesAndBalls()
    {
        //draw a line between all the waypoints
        for (int i = 0; i < checkPointLocations.Length; i++)
        {
            Vector3 currentWaypoint = checkPointLocations[i].position;
            Vector3 previosWaypoint = Vector3.zero;

            //calculate the previous waypoint
            if (i > 0)
                previosWaypoint = checkPointLocations[i - 1].position;
            else if (i == 0 && checkPointLocations.Length > 1)
                previosWaypoint = checkPointLocations[checkPointLocations.Length - 1].position;

            if (drawLines)
                Gizmos.DrawLine(previosWaypoint, currentWaypoint);
            if (drawSpheres)
                Gizmos.DrawSphere(currentWaypoint, sphereRadius);
        }
    }

    //spawns the waypoint prefab at each waypoint
    private void AddCarWaypoints()
    {
        checkPoints = new GameObject[checkPointLocations.Length]; // set the amount of waypoints as the same size of the amount of waypoint locations
        //spawns the waypoint prefab at each waypoint
        for (int i = 0; i < checkPointLocations.Length; i++)
        {
            Vector3 spawnPos = new Vector3(checkPointLocations[i].position.x, checkPointLocations[i].position.y + yOffset, checkPointLocations[i].position.z);
            checkPoints[i] = Instantiate(checkpointPrefab, spawnPos, checkPointLocations[i].rotation); //instanciate a checkpoint and add it to the array
            checkPoints[i].name = "Checkpoint " + i;
        }
    }
}
