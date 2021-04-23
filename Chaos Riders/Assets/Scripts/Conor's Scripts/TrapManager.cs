using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapManager : MonoBehaviour
{
    [Header ("Explosive Barrel")]
    [SerializeField] private float explosiveBarrelDamage = 60f;
    [Range(0.1f, 200f)]
    [SerializeField] private float bounceOffBarrelAmount = 60f;
    

    [Header("Blade Traps")]
    [SerializeField] private float bladeTrapDamage = 6f;

    //public statics
    public static float ExplosiveBarrelDamage;
    public static float BladeTrapDamage;
    public static float BounceOffBarrelAmount;

    void Awake()
    {
        ExplosiveBarrelDamage = explosiveBarrelDamage;
        BladeTrapDamage = bladeTrapDamage;
        BounceOffBarrelAmount = bounceOffBarrelAmount;
    }
}
