using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] private bool onlineCar = true;

    [SerializeField] private bool ai = false;
    public bool invincible = false;
    private AIHealth aiHealthScript;
    private Health healthScript;
    private OfflineHealth offlineHealthScript;

    public float health;

    private void Awake()
    {
        if(ai)
        {
            onlineCar = false;
        }
    }

    void Start()
    {
        if (ai)
            aiHealthScript = GetComponent<AIHealth>();
        
        if(!ai && onlineCar)
            healthScript = GetComponent<Health>();

        if (!ai && !onlineCar)
            offlineHealthScript = GetComponent<OfflineHealth>();
    }

    void Update()
    {
        if (ai)
        {
            health = aiHealthScript.health;
            if (health < 0)
                health = 0;
        }
        if (!ai && onlineCar)
        {
            health = healthScript.health;
            if (health < 0)
                health = 0;
        }
        if (!ai && !onlineCar)
        {
            health = offlineHealthScript.health;
            if (health < 0)
                health = 0;
        }
    }

    public void TakeDamage(float amount)
    {
        //if the amount of damage being dealt is more than the health set the amount of damage = to the health
        if (amount > health)
            amount = health;

        if (health > 0)
        {
            if (ai && !invincible)
            {
                aiHealthScript.health -= amount;
            }

            if (!ai && !invincible && onlineCar)
            {
                healthScript.health -= amount;
            }

            if (!ai && !invincible && !onlineCar)
            {
                offlineHealthScript.health -= amount;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Exlosive Barrel damage
        if(collision.gameObject.tag == "Explosive Barrel")
        {
            //Debug.Log("Take explosive barrel damage");
            TakeDamage(ExplosiveBarrel.ExplosiveDamage);
        }
    }
}
