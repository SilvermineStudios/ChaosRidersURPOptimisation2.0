using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minig2un : MonoBehaviour
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

    public string sound = "event:/GunFX/Minigun/MinigunShot 2";
    public string bulletWhistle = "event:/GunFX/Minigun/BulletWhistle";
    public string hitmarker = "event:/GunFX/Hitmarker";

    public float maxBulletDeviation;
    public float maxCrosshairDeviation;
    public float bulletDeviationIncrease;
    public float crosshairDeviationIncrease;


    Shooter shooterScript;

    private void Start()
    {
        
    }


    private void Update()
    {

        
    }




    

}
