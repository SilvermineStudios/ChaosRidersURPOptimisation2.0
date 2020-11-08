using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchShooterRotation : MonoBehaviour
{
    [SerializeField] private Transform carModelHolder;
    [SerializeField] private Transform carTurretBase, carTurretBarrels;

    private GameObject shooter;
    private Transform shooterT, modelHolder;
    private Transform turretBase, turretBarrels;
    private bool assignT = false;

    void Start()
    {
        
    }

    void AssingTransforms()
    {
        modelHolder = shooterT.Find("Model Holder");
        turretBase = modelHolder.Find("Base");
        turretBarrels = turretBase.Find("Target");
    }

    void Update()
    {
        //assign the shooter transforms
        if(assignT)
        {
            assignT = false;
            AssingTransforms();
        }

        carTurretBase.transform.rotation = turretBase.transform.rotation;
        carTurretBarrels.transform.rotation = turretBarrels.transform.rotation;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "shooter") //if the car detects a shooter above it
        {
            shooter = other.gameObject;
            shooterT = other.gameObject.transform;
            assignT = true;
        }
    }
}
