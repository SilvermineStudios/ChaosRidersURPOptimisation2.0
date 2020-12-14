﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Cinemachine;

public class Shooter : MonoBehaviour
{
    [SerializeField] private GameObject pointer;
    [SerializeField] private GameObject bulletSpawnPoint;
    [SerializeField] private GameObject car, carCollision;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float minigunDamage;
    [SerializeField] float playerNumber = 1;
    public bool connectCar = false;



    //shooting
    [SerializeField] private float damage = 10f;
    [SerializeField] private float range = 100f;

    [SerializeField] private KeyCode shootButton = KeyCode.Mouse0; 
    [SerializeField] private float amountOfAmmoForCooldownBar = 1000;
    private float startAmmo; //the amount of ammo for the cooldown bar at the start of the game
    private float ammoNormalized; //normalized the ammo value to be between 0 and 1 for the cooldown bar scale
    [SerializeField] private Transform coolDownBarUi; //ui bar that shows the cooldown of the minigun

    [SerializeField] private Transform gunBarrel; //barrel that is going to rotate to face the correct direction
    [SerializeField] private float horizontalRotationSpeed = 5f, verticalRotationSpeed = 3f; //rotation speeds for the gun
    private float xAngle, yAngle; //angle of rotation for the gun axis

    [SerializeField] private ParticleSystem muzzleFlash;

    [SerializeField] private float minRotationHeight = -20f, maxRotationHeight = 20f;

    private PhotonView pv;

    private void Awake()
    {
        pv = GetComponent<PhotonView>();
        startAmmo = amountOfAmmoForCooldownBar;
    }

    void Start()
    {
        if (pv.IsMine && IsThisMultiplayer.Instance.multiplayer || !IsThisMultiplayer.Instance.multiplayer)
        {

        }
    }

    // Update is called once per frame
    void Update()
    {

        if(connectCar)
        {
            connectCar = false;
            car = GetComponentInParent<MoveTurretPosition>().car;
            carCollision = GetComponentInParent<MoveTurretPosition>().car.transform.GetChild(0).gameObject;
        }



        if (pv.IsMine && IsThisMultiplayer.Instance.multiplayer || !IsThisMultiplayer.Instance.multiplayer)
        {
            RotateGunBarrel();

            ammoNormalized = amountOfAmmoForCooldownBar / startAmmo; //normalized the ammo value to be between 0 and 1 for the cooldown bar scale
            CoolDownBar(ammoNormalized); //scale the size of the cooldown bar to match the ammo count

            //if you are shooting and have ammo
            if (Input.GetKey(shootButton) && amountOfAmmoForCooldownBar > 0)
            {
                muzzleFlash.Play();
                amountOfAmmoForCooldownBar--;
                pv.RPC("Shoot", RpcTarget.All);
            }

            //if you are not shooting and the ammo isnt full
            if (amountOfAmmoForCooldownBar < startAmmo && !Input.GetKey(shootButton))
            {
                amountOfAmmoForCooldownBar++;
            }
                
        }
    }

    private void RotateGunBarrel()
    {
        xAngle += Input.GetAxis("Mouse X") * horizontalRotationSpeed * Time.deltaTime;
        //xAngle = Mathf.Clamp(xAngle, 0, 180); //use this if you want to clamp the rotation. second var = min angle, third var = max angle

        yAngle += Input.GetAxis("Mouse Y") * verticalRotationSpeed * -Time.deltaTime;
        yAngle = Mathf.Clamp(yAngle, minRotationHeight, maxRotationHeight); //use this if you want to clamp the rotation. second var = min angle, third var = max angle

        gunBarrel.localRotation = Quaternion.Euler(yAngle, xAngle, 0);
    }

    private void CoolDownBar(float sizeNormalized)
    {
       coolDownBarUi.localScale = new Vector3(sizeNormalized, 1f); //scale the ui cooldown bar to match the ammo count
    }

    [PunRPC]
    void Shoot()
    {
        RaycastHit hit; //gets the information on whats hit
        if (Physics.Raycast(bulletSpawnPoint.transform.position, bulletSpawnPoint.transform.forward, out hit, range))
        {
            Debug.Log("You Hit The: " + hit.transform.name);

            Target target = hit.transform.GetComponent<Target>();
            if(target != null)
            {
                target.TakeDamage(damage);
            }
        }
    }

    /*
    Vector3 raycastDir;
    void FireBullet()
    {
        RaycastHit hit;
        raycastDir = pointer.transform.position - transform.position;


        if (Physics.Raycast(bulletSpawnPoint.transform.position, raycastDir, out hit, Mathf.Infinity, layerMask))
        {
            if (hit.transform.gameObject.layer == 10 && hit.transform.gameObject != car && hit.transform.gameObject != carCollision)
            {
                float[] DamagetoTake = new float[2];
                DamagetoTake[0] = minigunDamage;
                DamagetoTake[1] = playerNumber;
                hit.transform.gameObject.SendMessage("TakeDamage", DamagetoTake);
                Debug.Log("Did Hit");
                Debug.Log(hit.transform.gameObject);
                Debug.Log(car);

            }
            else
            {
                Debug.Log("Did not Hit");
            }
        }
    }
    */
}
