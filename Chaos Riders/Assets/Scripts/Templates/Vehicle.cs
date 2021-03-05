using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newVehicleData", menuName = "Data/Vehicle Data")]

public class Vehicle : ScriptableObject
{
    #region Floats
    [Header("Floats")]
    public float[] gearRatio = new float[5];
    public int numOfGears = 5;
    public float revRangeBoundary = 1f;
    public float fullTorqueOverAllWheels;
    public float boostFullTorqueOverAllWheels;
    public float brakeTorque;
    public float reverseTorque;
    public float topSpeed;
    public float boostSpeed;
    public float downforce;
    public float maxSteerAngle = 30;
    public float maxFOV;
    public float boostFOV;
    public float fovChangeSpeed = 0.002f;
    #endregion

    #region Steering and Traction
    [Header("Steering and Traction")]
    public float steerHelper;
    public float steerHelperDrift;
    public float tractionControl;
    #endregion

    #region Skidmarks
    [Header("Skidmarks")]
    public float slipLimit = 0.3f;
    public float forwardSkidLimit = 0.5f;
    public float sideSkidLimit = 0.5f;
    public WheelFrictionCurve drift;
    public WheelFrictionCurve normal;
    #endregion

    #region Camera
    [Header("Camera")]
    public Vector3 stationaryCamOffset;
    public Vector3 movingCamOffset;
    public Vector3 boostCamOffset;
    #endregion

    #region Car Physics
    [Header("Car Physics")]
    public Vector3 centerOfMass;
    public float vehicleMass;
    public float vehicleDrag;
    public float vehicleAngularDrag;
    #endregion

    #region Main Wheel Physics
    [Header("Main Wheel Physics")]
    public float wheelMass = 20;
    public float wheelRadius = 0.325f;
    public float wheelDampingRate = 0.05f;
    public float wheelSuspensionDistance = 0.2f;
    public float wheelForceAppPontDistance = 0.1f;
    public Vector3 wheelCenter;
    #endregion

    #region Wheel Suspension Physics
    [Header("Wheel Suspension Physics")]
    public JointSpring suspension;
    public float spring = 70000;
    public float damper = 5500;
    public float targetPosition = 0.1f;
    #endregion

    #region Wheel Forward Friction Physics
    [Header("Wheel Forward Friction Physics")]
    public WheelFrictionCurve forwardFriction;
    public float forwardExtremumSlip = 0.2f;
    public float forwardExtremumValue = 1;
    public float forwardAsymptoteSlip = 0.8f;
    public float forwardAsymptoteValue = 0.5f;
    public float forwardStiffness = 1.5f;
    #endregion

    #region Wheel Side Friction Physics
    [Header("Wheel Side Friction Physics")]
    public WheelFrictionCurve sideFriction;
    public float sideExtremumSlip = 0.3f;
    public float sideExtremumValue = 1;
    public float sideAsymptoteSlip = 0.5f;
    public float sideAsymptoteValue = 0.75f;
    public float sideStiffness = 1;
    #endregion

    //Setup Values
    private void OnEnable()
    {
        //Suspension
        suspension.spring = spring;
        suspension.damper = damper;
        suspension.targetPosition = targetPosition;

        //Forward Friction
        forwardFriction.extremumSlip = forwardExtremumSlip;
        forwardFriction.extremumValue = forwardExtremumValue;
        forwardFriction.asymptoteSlip = forwardAsymptoteSlip;
        forwardFriction.asymptoteValue = forwardAsymptoteValue;
        forwardFriction.stiffness = forwardStiffness;

        //Side Friction
        sideFriction.extremumSlip = sideExtremumSlip;
        sideFriction.extremumValue = sideExtremumValue;
        sideFriction.asymptoteSlip = sideAsymptoteSlip;
        sideFriction.asymptoteValue = sideAsymptoteValue;
        sideFriction.stiffness = sideStiffness;
    }

}
