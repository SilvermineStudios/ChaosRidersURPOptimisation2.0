using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newVehicleData", menuName = "Data/Vehicle Data")]

public class Vehicle : ScriptableObject
{
    #region Car Physics
    [Header("Car Physics")]
    public Vector3 centerOfMass;
    public float vehicleMass;
    public float vehicleDrag;
    public float vehicleAngularDrag;
    #endregion

    #region Wheel Physics
    [Header("Wheel Physics")]
    public float wheelMass = 20;
    #endregion

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

}
