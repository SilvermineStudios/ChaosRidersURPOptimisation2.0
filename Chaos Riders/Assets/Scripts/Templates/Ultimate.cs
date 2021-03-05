using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newUltimateData", menuName = "Data/Ultimate Data")]

public class Ultimate : ScriptableObject
{

    public float ultimatePrepTime = 0;
    public float ultimateUsetime;
    public float chargeRate;
    public string sound;
    public float damage;
    public float damageRadius;

}
