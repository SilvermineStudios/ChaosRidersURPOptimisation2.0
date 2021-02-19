using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShredUltimate : MonoBehaviour
{
    Target myHealth;
    Collider[] nearbyCars;
    [SerializeField] float damage;
    [SerializeField] float radius;
    void Start()
    {
        myHealth = GetComponentInParent<Target>();
    }

    

    void FixedUpdate()
    {

        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

        foreach (Collider nearby in colliders)
        {
            Target health = nearby.GetComponent<Target>();
            if (health != null && health != myHealth)
            {
                health.TakeDamage(damage);
            }

            AIHealth aiHealth = GetComponent<AIHealth>();
            if (aiHealth != null)
            {
                aiHealth.TakeDamage(damage);
            }
        }
        
    }


}
