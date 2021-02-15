using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIWheel : MonoBehaviour
{
    [SerializeField] private WheelCollider targetWheel;
    private Vector3 wheelPosition = new Vector3();
    private Quaternion wheelRotation = new Quaternion();

    // Update is called once per frame
    void Update()
    {
        // set the wheelPosition and wheelRotation = position and rotation of the wheel collider
        targetWheel.GetWorldPose(out wheelPosition, out wheelRotation); 
        transform.position = wheelPosition;
        transform.rotation = wheelRotation;
    }
}
