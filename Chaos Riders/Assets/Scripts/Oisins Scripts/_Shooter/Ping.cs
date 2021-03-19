using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Ping : MonoBehaviour
{
    RaycastHit hit;
    CinemachineVirtualCamera cineCamera;
    float height = 2;
    float radius = 2;

    void Start()
    {
        cineCamera = GetComponent<Shooter>().cineCamera;
    }

    void Update()
    {


        Vector3 p1 = cineCamera.transform.position + Vector3.up * -height;
        Vector3 p2 = p1 + Vector3.up * height;

        float distanceToObstacle = 0;


        Physics.Raycast(cineCamera.transform.position, transform.forward, out hit, 999);

        // Cast character controller shape 10 meters forward to see if it is about to hit anything.
        if (Physics.CapsuleCast(p1, p2, radius, transform.forward, out hit, 999))
        {
            distanceToObstacle = hit.distance;
            if (hit.transform.gameObject.tag != "Untagged")
            {
                Debug.Log(distanceToObstacle + hit.transform.gameObject.tag);
            }
        }
    }
}
