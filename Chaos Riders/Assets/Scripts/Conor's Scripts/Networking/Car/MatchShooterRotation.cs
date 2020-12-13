using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchShooterRotation : MonoBehaviour
{
    //new refs
    [SerializeField] private Transform carGunBarrel, shooterGunBarrel;

    //CAR TURRET REFS
    [SerializeField] private Transform carModelHolder;
    [SerializeField] private Transform carTurretBase, carTurretBarrels;

    //SHOOTER PLAYER REFS
    private GameObject shooter;
    [SerializeField] private Transform shooterT, modelHolder;
    private Transform turretBase, turretBarrels;
    private bool assignT = false;
    [SerializeField] private bool hasShooter = false;
    private bool canConnect = true;

    
    void AssingTransforms()//getting the shooter players transforms
    {
        modelHolder = shooterT.Find("Model Holder");
        shooterGunBarrel = modelHolder.Find("Gun Barrel");
    }

    void Update()
    {
        if (hasShooter && shooter != null) //only run if there is a shooter in the game
        {
            //assign the shooter transforms
            if (assignT)
            {
                assignT = false; //stop this from repeating
                AssingTransforms();
            }

            carGunBarrel.rotation = shooterGunBarrel.rotation;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!canConnect) return;

        if (other.gameObject.tag == "shooter" && canConnect) //if the car detects a shooter above it
        {
            canConnect = false;
            shooter = other.gameObject;
            shooterT = other.gameObject.transform;
            assignT = true;
            hasShooter = true;
        }
    }
}
