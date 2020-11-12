using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Health : MonoBehaviour
{
    [SerializeField] float health;
    [SerializeField] float lastHit;

    Slider healthbar;
    [SerializeField] GameObject deathParticles;

    
    public bool isDead { get { return dead; } private set { isDead = dead; } }

    bool dead;

    void Start()
    {

        health = 100;
        healthbar = GetComponentInChildren<Slider>();

    }



    void Update()
    {
        healthbar.value = health;

        if(health <= 0 && !dead)
        {
            dead = true;
        }
        if(dead)
        {
            Die();
        }

    }

    public float timeSinceDeath, deathTimer;

    void Die()
    {
        deathParticles.SetActive(true);
        
        if (timeSinceDeath > deathTimer)
        {
            dead = false;
            health = 100;
            deathParticles.SetActive(false);
            timeSinceDeath = 0;
        }
        else
        {
            timeSinceDeath += Time.deltaTime;
        }  
    }


    public void TakeDamage(float[] DamagetoTake)
    {
        health -= DamagetoTake[0];
        lastHit = DamagetoTake[1];

    }
}
