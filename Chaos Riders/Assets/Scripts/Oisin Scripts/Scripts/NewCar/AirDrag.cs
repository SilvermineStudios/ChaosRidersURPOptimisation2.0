using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirDrag : MonoBehaviour
{
    public Rigidbody rb;

    public GameObject centerOfMass;

    // lift coefficient (use negative values for downforce).
    public float liftCoefficient;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = centerOfMass.transform.localPosition;
    }

    void Update()
    {
        float lift = liftCoefficient * rb.velocity.sqrMagnitude;
        //rb.AddForceAtPosition(lift * transform.up, transform.position);
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(rb.centerOfMass, 10);

        
    }

}
