using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncyWall : MonoBehaviour
{
    private Rigidbody rb;

    private Vector3 lastVelocity;

    [Range (0, 100)]
    public float bounceSpeedPercentage = 60f; //the percentage of your speed that you bounce with

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        lastVelocity = rb.velocity;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Bouncy")
        {
            
            var direction = Vector3.Reflect(lastVelocity.normalized, collision.contacts[0].normal);
            var speed = lastVelocity.magnitude;
            float bounceSpeed = (speed / 100) * bounceSpeedPercentage;

            rb.velocity = direction * Mathf.Max(bounceSpeed, 0f);
        }
    }
}
