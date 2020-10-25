using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TractionHelperMultiplayer : MonoBehaviour
{
    // assign car's front wheels here.
    public WheelMultiplayer[] front;

    // how strong oversteer is compensated for
    public float compensationFactor = 0.1f;

    // state
    float oldGrip;
    float angle;
    float angularVelo;

    void Start()
    {
        oldGrip = front[0].grip;
    }

    void Update()
    {
        Vector3 driveDir = transform.forward;
        Vector3 veloDir = GetComponent<Rigidbody>().velocity;
        veloDir -= transform.up * Vector3.Dot(veloDir, transform.up);
        veloDir.Normalize();

        angle = -Mathf.Asin(Vector3.Dot(Vector3.Cross(driveDir, veloDir), transform.up));

        angularVelo = GetComponent<Rigidbody>().angularVelocity.y;

        foreach (WheelMultiplayer w in front)
        {
            if (angle * w.steering < 0)
            {
                w.grip = oldGrip * (1.0f - Mathf.Clamp01(compensationFactor * Mathf.Abs(angularVelo)));
            }
            else
            {
                w.grip = oldGrip;
            }
        }

    }
}
