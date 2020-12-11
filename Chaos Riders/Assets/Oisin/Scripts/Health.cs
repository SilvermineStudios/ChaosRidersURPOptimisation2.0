using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Health : MonoBehaviour
{
    [SerializeField] private Transform healthBarUi; 

    public float health;
    private float healthNormalized;
    private float startHealth;
    [SerializeField] float lastHit;

    public bool isProtected;

    Slider healthbar;
    [SerializeField] GameObject deathParticles;

    
    public bool isDead { get { return dead; } private set { isDead = dead; } }

    public bool dead;

    void Start()
    {
        startHealth = health;
        healthbar = GetComponentInChildren<Slider>();
    }



    void Update()
    {
        healthNormalized = (health / startHealth);
        SetHealthBarUiSize(healthNormalized);

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

    float timeSinceDeath, deathTimer = 5;

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
        if (!isProtected)
        {
            health -= DamagetoTake[0];
            lastHit = DamagetoTake[1];
        }
    }

    private void SetHealthBarUiSize(float sizeNormalized)
    {
        healthBarUi.localScale = new Vector3(1f, sizeNormalized);
    }
}
