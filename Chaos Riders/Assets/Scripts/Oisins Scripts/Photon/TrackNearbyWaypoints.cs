using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TrackNearbyWaypoints : MonoBehaviour
{

    public List<Collider> nearbyWaypoints = new List<Collider>();

    PhotonView pv;
    Collider nearestWaypoint;
    Vector3 currentPosition;

    public float distToNearest { get; private set; }
    public int nearestNum { get; private set; }
    int currentNum;
    private void Start()
    {
        pv = GetComponentInParent<PhotonView>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name != transform.name)
        {
            nearbyWaypoints.Add(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name != transform.name)
        {
            nearbyWaypoints.Remove(other);
        }
    }


    private void FixedUpdate()
    {
        if (!pv.IsMine) { return; }
        currentNum = -1;
        foreach (Collider potentialTarget in nearbyWaypoints)
        {
            int i = int.Parse(potentialTarget.name);
            if (i > currentNum && i - nearestNum >= -5)
            {
                nearestWaypoint = potentialTarget;
            }
        }
        if (nearestWaypoint != null)
        {
            nearestNum = int.Parse(nearestWaypoint.name);
            distToNearest = Vector3.Distance(transform.position, nearestWaypoint.transform.position);
        }
    }

}


/*
 * 
 *             Vector3 directionToTarget = potentialTarget.transform.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                nearestWaypoint = potentialTarget;
            }

        public float closestDistanceSqr { get; private set; } 
        closestDistanceSqr = Mathf.Infinity;
                currentPosition = transform.position;
*/
