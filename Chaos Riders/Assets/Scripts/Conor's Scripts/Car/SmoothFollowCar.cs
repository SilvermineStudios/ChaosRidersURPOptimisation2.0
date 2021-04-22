using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothFollowCar : MonoBehaviour
{
    [Range(0f, 1f)]
    public float lerpPercentage = 0.5f;

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
    /*
    void LateUpdate()
    {
        if (!Target) return;

        //Move + Rotate Car 
        transform.position = Vector3.Lerp(transform.position, TargetCar.position, Time.deltaTime * lerpPercentage);
        transform.rotation = Quaternion.Lerp(transform.rotation, TargetCar.rotation, Time.deltaTime * lerpPercentage);

        //Rotate Wheels
        MyFL.rotation = Quaternion.Lerp(MyFL.rotation, FL.rotation, Time.deltaTime * lerpPercentage);
        MyFR.rotation = Quaternion.Lerp(MyFR.rotation, FR.rotation, Time.deltaTime * lerpPercentage);
    }
    */
    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, TargetCar.position, lerpPercentage);
        transform.rotation = Quaternion.Lerp(transform.rotation, TargetCar.rotation, lerpPercentage);

        //Rotate Wheels
        MyFL.rotation = Quaternion.Lerp(MyFL.rotation, FL.rotation, lerpPercentage);
        MyFR.rotation = Quaternion.Lerp(MyFR.rotation, FR.rotation, lerpPercentage);
    }
}
