using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newWeaponData", menuName = "Data/Weapon Data")]

public class Weapon : ScriptableObject
{


    [Header("Bools")]
    public bool usingAmmo;
    public bool rotate;

    [Header("Floats")]
    public float damage;
    public float range = 9999f;
    public float fireRate;
    public float ammoUsage = 1;
    public float trailPercentage;

    [Header("Sound")]
    public string sound;
    public string bulletWhistle;
    public string hitmarker = "event:/GunFX/Hitmarker";

    [Header("Crosshair")]
    public float maxCrosshairDeviation;
    public float crosshairDeviationIncrease;
    public float crosshairWaitTime;
    public float crossshairIncreaseSpeed = 1;
    public float crossshairResetSpeed = 1;

    [Header("Spread")]
    public float maxBulletDeviation;
    public float bulletDeviationIncrease;


}
