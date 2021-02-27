using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Cinemachine;
using TMPro;
using System.IO;
using UnityEngine.UI;

public class Minigun : MonoBehaviour
{
    
    [SerializeField] public ParticleSystem muzzleFlash;
    
    //z rotation positive ++
    [SerializeField] private Transform barrelToRotate;
    private float barrelRotationSpeed;
    [SerializeField] private float barrelRotationStartSpeed = 100f, barrelRotationMaxSpeed = 800f;
    [SerializeField] public float minigunDamage;
    [SerializeField] public float range = 100f;
    [SerializeField] public float minigunFireRate;
     public float amountOfAmmoForCooldownBar = 1000;

    private float startAmmo; //the amount of ammo for the cooldown bar at the start of the game
    private float ammoNormalized; //normalized the ammo value to be between 0 and 1 for the cooldown bar scale
    [SerializeField] private Transform coolDownBarUi; //ui bar that shows the cooldown of the minigun

    public string sound = "event:/GunFX/Minigun/MinigunShot";
    public string bulletWhistle = "event:/GunFX/Minigun/BulletWhistle";

    public float maxBulletDeviation;
    public float maxCrosshairDeviation;
    public float bulletDeviationIncrease;
    public float crosshairDeviationIncrease;


    Shooter shooterScript;

    private void Start()
    {
        shooterScript = GetComponent<Shooter>();
        startAmmo = amountOfAmmoForCooldownBar;
        barrelRotationSpeed = barrelRotationStartSpeed;
    }


    private void Update()
    {
        RotateGunBarrel();
        CooldownBarValues();
        ammoNormalized = amountOfAmmoForCooldownBar / startAmmo; //normalized the ammo value to be between 0 and 1 for the cooldown bar scale
        CoolDownBar(ammoNormalized); //scale the size of the cooldown bar to match the ammo count
        if(amountOfAmmoForCooldownBar <= 1)
        {
            shooterScript.hasAmmo = false;
        }
        else
        {
            shooterScript.hasAmmo = true;
        }
        
    }


    private void RotateGunBarrel()
    {
        barrelToRotate.Rotate(0, 0, barrelRotationSpeed * Time.deltaTime);
    }

    private void CoolDownBar(float sizeNormalized)
    {
        coolDownBarUi.localScale = new Vector3(sizeNormalized, 1f); //scale the ui cooldown bar to match the ammo count
    }

    void CooldownBarValues()
    {
        //if you are shooting and have ammo
        if (amountOfAmmoForCooldownBar > 0 && shooterScript.isPressingShootbutton)
        {
            amountOfAmmoForCooldownBar--;
            barrelRotationSpeed = barrelRotationMaxSpeed;
        }
        else
            barrelRotationSpeed = barrelRotationStartSpeed;

        //if you are not shooting and the ammo isnt full
        if (amountOfAmmoForCooldownBar < startAmmo && !shooterScript.isPressingShootbutton)
        {
            amountOfAmmoForCooldownBar++;
        }
    }

    

}
