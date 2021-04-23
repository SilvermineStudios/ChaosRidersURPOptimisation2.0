using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIWaypointManager : MonoBehaviour
{
    [SerializeField] private Color lineColour;
    [SerializeField] private float sphereRadius = 0.3f;
    public Transform[] waypoints;
    public static Transform[] Waypoints;


    private void OnDrawGizmos()
    {
        Gizmos.color = lineColour;

        waypoints = new Transform[this.transform.childCount]; //make the array the same length as the amount of children waypoints
        for (int i = 0; i < this.transform.childCount; i++) //put every waypoint(Child) in the array
        {
            waypoints[i] = transform.GetChild(i);
            waypoints[i].name = i.ToString();
        }

        //draw a line between all the waypoints
        for (int i = 0; i < waypoints.Length; i++)
        {
            Vector3 currentWaypoint = waypoints[i].position;
            Vector3 previosWaypoint = Vector3.zero;

            //calculate the previous waypoint
            if (i > 0)
            {
                previosWaypoint = waypoints[i - 1].position;
            }

            else if (i == 0 && waypoints.Length > 1)
            {
                previosWaypoint = waypoints[waypoints.Length - 1].position;
            }


            Gizmos.DrawLine(previosWaypoint, currentWaypoint);
            Gizmos.DrawSphere(currentWaypoint, sphereRadius);

            
        }
    }

    private void Awake()
    {
        waypoints = new Transform[this.transform.childCount]; //make the array the same length as the amount of children waypoints
        for (int i = 0; i < this.transform.childCount; i++) //put every waypoint(Child) in the array
        {
            waypoints[i] = transform.GetChild(i);
        }

        Waypoints = waypoints;
    }

    private void DrawLinesAndBalls()
    {
        
    }

}
