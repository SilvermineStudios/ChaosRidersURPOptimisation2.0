using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothFollowCar : MonoBehaviour
{
    [Header("Car Stuff")]
    public GameObject Target;
    private Transform TargetCar;
    
    [Header("My Wheels")]
    public Transform MyFL;
    public Transform MyFR;

    //Wheels to follow
    private Transform FL;
    private Transform FR;

    private void Awake()
    {
        TargetCar = Target.transform;

        FL = Target.GetComponent<CarRefrences>().FL;
        FR = Target.GetComponent<CarRefrences>().FR;
    }

    void LateUpdate()
    {
        if (!Target) return;

        //Move + Rotate Car 
        transform.position = Vector3.Lerp(transform.position, TargetCar.position, Time.deltaTime * 100);
        transform.rotation = Quaternion.Lerp(transform.rotation, TargetCar.rotation, Time.deltaTime * 100);

        //Rotate Wheels
        MyFL.rotation = Quaternion.Lerp(FL.rotation, FL.rotation, Time.deltaTime * 100);
        MyFR.rotation = Quaternion.Lerp(FR.rotation, FR.rotation, Time.deltaTime * 100);
        
    }
}
