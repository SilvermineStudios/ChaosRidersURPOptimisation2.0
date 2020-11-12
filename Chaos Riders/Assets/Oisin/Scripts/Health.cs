using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Health : MonoBehaviour
{
    [SerializeField] float health;
    [SerializeField] float lastHit;

    Slider healthbar;

    void Start()
    {
        health = 100;
        healthbar = GetComponentInChildren<Slider>();
    }



    void Update()
    {
        healthbar.value = health;
    }


    public void TakeDamage(float[] DamagetoTake)
    {
        health -= DamagetoTake[0];
        lastHit = DamagetoTake[1];

    }
}
