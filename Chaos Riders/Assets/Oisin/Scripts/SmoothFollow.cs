using UnityEngine;
using System.Collections;

public class SmoothFollow : MonoBehaviour {
    public Transform target;
    public float distance = 1.6f;
    public float height = 0.4f;
    public float damping = 0.7f;
    public bool smoothRotation = true;
    public bool smoothMove = false;
    public bool followBehind = true;
    public float rotationDamping = 50.0f;


    public bool driver = false;


    void FixedUpdate()
    {
        if(!driver) { return; }

        cameraMethod();

    }

    private void Update()
    {
        if (driver) { return; }

        cameraMethod();
    }


    void cameraMethod()
    {
        Vector3 wantedPosition;
        if (followBehind)
            wantedPosition = target.TransformPoint(0, height, -distance);
        else
            wantedPosition = target.TransformPoint(0, height, distance);

        if (smoothMove)
        {
            transform.position = Vector3.Lerp(transform.position, wantedPosition, damping);
        }
        else
        {
            transform.position = wantedPosition;
        }
        if (smoothRotation)
        {
            Quaternion wantedRotation = Quaternion.LookRotation(target.position - transform.position, target.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, wantedRotation, Time.deltaTime * rotationDamping);
        }
        else
        {
            transform.LookAt(target, target.up);
        }
    }
}