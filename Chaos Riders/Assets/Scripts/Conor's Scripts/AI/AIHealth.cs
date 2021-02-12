using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AIHealth : MonoBehaviour
{
    public GameObject myHealthBar;
    public float health;
    private float startHealth;
    private float healthNormalized;

    [SerializeField] float lastHit;

    public bool isProtected;

    Slider healthbar;
    [SerializeField] GameObject deathParticles;


    public bool isDead { get { return dead; } private set { isDead = dead; } }

    public bool dead;

    private void Awake()
    {
        startHealth = health;
    }

    void Start()
    {
        healthbar = GetComponentInChildren<Slider>();
    }



    void Update()
    {
        healthNormalized = (health / startHealth);
        healthbar.value = healthNormalized;

        if (health <= 0 && !dead)
        {
            dead = true;
        }
        if (dead)
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
            health = startHealth;
            deathParticles.SetActive(false);
            timeSinceDeath = 0;
        }
        else
        {
            timeSinceDeath += Time.deltaTime;
        }
    }


    public void TakeDamage(float DamagetoTake)
    {
        if (!isProtected)
        {
            health -= DamagetoTake;
        }
    }
}