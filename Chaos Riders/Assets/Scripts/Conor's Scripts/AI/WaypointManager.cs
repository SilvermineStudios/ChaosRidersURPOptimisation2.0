using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointManager : MonoBehaviour
{
    [SerializeField] private bool drawLines = true;
    [SerializeField] private bool drawSpheres = true;
    [SerializeField] private Color lineColour;
    [SerializeField] private float sphereRadius = 0.3f;
    public Transform[] waypoints;


    [SerializeField] private GameObject waypointPrefab;
    [SerializeField] private float yOffset = -6f;

    


    private void Awake()
    {
        AddCarWaypoints();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = lineColour;

        waypoints = new Transform[this.transform.childCount]; //make the array the same length as the amount of children waypoints
        for (int i = 0; i < this.transform.childCount; i++) //put every waypoint(Child) in the array
        {
            waypoints[i] = transform.GetChild(i);
        }

        DrawLinesAndBalls();
    }

    private void DrawLinesAndBalls()
    {
        //draw a line between all the waypoints
        for (int i = 0; i < waypoints.Length; i++)
        {
            Vector3 currentWaypoint = waypoints[i].position;
            Vector3 previosWaypoint = Vector3.zero;

            //calculate the previous waypoint
            if (i > 0)
                previosWaypoint = waypoints[i - 1].position;
            else if (i == 0 && waypoints.Length > 1)
                previosWaypoint = waypoints[waypoints.Length - 1].position;

            if (drawLines)
                Gizmos.DrawLine(previosWaypoint, currentWaypoint);
            if (drawSpheres)
                Gizmos.DrawSphere(currentWaypoint, sphereRadius);
        }
    }

    private void AddCarWaypoints()
    {
        //spawns the waypoint prefab at each waypoint
        for (int i = 0; i < waypoints.Length; i++)
        {
            Vector3 spawnPos = new Vector3(waypoints[i].position.x, waypoints[i].position.y + yOffset, waypoints[i].position.z);  
            Instantiate(waypointPrefab, spawnPos, waypoints[i].rotation);
        }
    }










    #region code
    /*
     * 
     * 
     * 
     * LOADS OF CODE HERE!!
     * 
     * 
     * 
     * 
     * 
     */
    #endregion
}
