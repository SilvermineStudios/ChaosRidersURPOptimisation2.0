using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AttachHealth : MonoBehaviour
{

    private GameObject shooter;
    private bool canConnect = true;
    [SerializeField] private bool hasShooter = false;
    Health healthScript;

    private void Start()
    {
        healthScript = GetComponent<Health>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (!canConnect) return;

        if (other.gameObject.tag == "shooter" && canConnect) //if the car detects a shooter above it
        {
            canConnect = false;
            shooter = other.gameObject;
            hasShooter = true;

            shooter.GetComponentInChildren<RaycastMinigun>().healthScript = healthScript; //CONSOLE ERROR
            
        }
    }
}
