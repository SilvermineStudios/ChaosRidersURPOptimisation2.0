using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] private bool ai = false;
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
        //TakeDamage(1);

        if (ai)
        {
            health = aiHealthScript.health;
        }
        else
        {
            health = healthScript.health;
        }
    }

    public void TakeDamage(float amount)
    {
        if (health > 0)
        {
            if (ai)
            {
                aiHealthScript.health -= amount;
            }
            else
            {
                healthScript.health -= amount;
            }
        }
    }
}
