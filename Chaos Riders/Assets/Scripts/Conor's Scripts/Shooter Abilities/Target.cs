using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] private bool ai = false;
    public bool invincible = false;
    private AIHealth aiHealthScript;
    private Health healthScript;

    [SerializeField] private float health;

    void Start()
    {
        if (ai)
            aiHealthScript = GetComponent<AIHealth>();
        else
            healthScript = GetComponent<Health>();
    }

    /*
    private void OnGUI()
    {
        GUILayout.Label(" ");
        GUILayout.Label(" ");
        GUILayout.Label(" ");
        GUILayout.Label(" ");
        GUILayout.Label(" ");
        GUILayout.Label(" ");
        GUILayout.Label(" ");
        GUILayout.Label(" ");
        GUILayout.Label(" ");
        GUILayout.Label(" ");
        GUILayout.Label("Health: " + health);
    }
    */

    void Update()
    {
        if (ai)
        {
            health = aiHealthScript.health;
            if (health < 0)
                health = 0;
        }
        else
        {
            health = healthScript.health;
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

            if (!ai && !invincible)
            {
                healthScript.health -= amount;
            }
        }
    }
}
