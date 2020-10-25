using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntiRollBarMultiplayer : MonoBehaviour
{
    // The two wheels connected by the anti-roll bar. These should be on the same axle.
    public WheelMultiplayer wheel1;
    public WheelMultiplayer wheel2;

    // Coeefficient determining how much force is transfered by the bar.
    public float coefficient = 10000;

    void FixedUpdate()
    {
        float force = (wheel1.compression - wheel2.compression) * coefficient;
        wheel1.suspensionForceInput = +force;
        wheel2.suspensionForceInput = -force;
    }
}
