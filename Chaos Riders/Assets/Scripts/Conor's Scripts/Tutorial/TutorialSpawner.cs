using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialSpawner : MonoBehaviour
{
    public Transform playerSpawnPoint;
    public Transform aiSpawnPointHolder;
    public Transform[] aiSpawnPoints;

   
    void Start()
    {
        aiSpawnPoints = new Transform[aiSpawnPointHolder.childCount]; //make the array the same length as the amount of children waypoints
        for (int i = 0; i < aiSpawnPointHolder.childCount; i++) //put every waypoint(Child) in the array
        {
            aiSpawnPoints[i] = aiSpawnPointHolder.GetChild(i);
        }
    }


    void Update()
    {
        
    }
}
