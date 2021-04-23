using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapManager : MonoBehaviour
{
    [Header ("Trap Damage")]
    [SerializeField] private float explosiveBarrelDamage = 60f;
    [SerializeField] private float bladeTrapDamage = 6f;

    //public statics
    public static float ExplosiveBarrelDamage;
    public static float BladeTrapDamage;

    void Awake()
    {
        ExplosiveBarrelDamage = explosiveBarrelDamage;
        BladeTrapDamage = bladeTrapDamage;
    }
}
