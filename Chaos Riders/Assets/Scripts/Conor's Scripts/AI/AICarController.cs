using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICarController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float distanceFromNextWaypoint;

    [SerializeField] private WaypointManager waypointManager;
    [SerializeField] private Transform[] waypoints;

    private void Awake()
    {
        waypointManager = FindObjectOfType<WaypointManager>(); //find the waypoint manager
    }

    void Start()
    {
        waypoints = new Transform[waypointManager.waypoints.Length];
        waypoints = waypointManager.waypoints;
    }

    void Update()
    {
        
    }
}
