using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapManager : MonoBehaviour
{
    [Header ("Explosive Barrel")]
    [SerializeField] private float explosiveBarrelDamage = 60f;
    [Range(0.1f, 200f)]
    [SerializeField] private float bounceOffBarrelAmount = 60f;
    [Range(1, 8)]
    [SerializeField] private int barrelHealth = 3; //amount of bullets it takes to destroy
    [Range(1, 8)]
    [SerializeField] private int explodedForTime = 3; //amount of bullets it takes to destroy

    //public statics
    public static float ExplosiveBarrelDamage;
    public static float BounceOffBarrelAmount;
    public static int BarrelHealth;
    public static int ExplodedForTime;


    [Header("Traps")]
    [SerializeField] private float bladeTrapDamage = 6f;
    [SerializeField] private float axeTrapDamage = 10f;

    //public statics
    public static float BladeTrapDamage;
    public static float AxeTrapDamage;
    

    void Awake()
    {
        ExplosiveBarrelDamage = explosiveBarrelDamage;
        BounceOffBarrelAmount = bounceOffBarrelAmount;
        BarrelHealth = barrelHealth;
        ExplodedForTime = explodedForTime;

        BladeTrapDamage = bladeTrapDamage;
        AxeTrapDamage = axeTrapDamage;
    }
}
