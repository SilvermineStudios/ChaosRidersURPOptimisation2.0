﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchShooterRotation : MonoBehaviour
{
    //CAR TURRET REFS
    [SerializeField] private Transform carModelHolder;
    [SerializeField] private Transform carTurretBase, carTurretBarrels;

    //SHOOTER PLAYER REFS
    private GameObject shooter;
    private Transform shooterT, modelHolder;
    private Transform turretBase, turretBarrels;
    private bool assignT = false;
    private bool hasShooter = false;

    
    void AssingTransforms()//getting the shooter players transforms
    {
        modelHolder = shooterT.Find("Model Holder");
        turretBase = modelHolder.Find("Base");
        turretBarrels = turretBase.Find("Target");
    }

    void Update()
    {
        if (hasShooter) //only run if there is a shooter in the game
        {
            //assign the shooter transforms
            if (assignT)
            {
                assignT = false; //stop this from repeating
                AssingTransforms();
            }

            carTurretBase.transform.rotation = turretBase.transform.rotation; /////////////make the car turret base rotation be the same as the shooter turret base rotation
            carTurretBarrels.transform.rotation = turretBarrels.transform.rotation;/////////////make the car turret barrel rotation be the same as the shooter turret barrel rotation
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "shooter") //if the car detects a shooter above it
        {
            shooter = other.gameObject;
            shooterT = other.gameObject.transform;
            assignT = true;
            hasShooter = true;
        }
    }
}
