﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Cinemachine;
using TMPro;
using System.IO;
using UnityEngine.UI;

public class Shooter : MonoBehaviour
{

    #region General GameObjects
    [Header("General GameObjects")]
    public GameObject car;
    private Transform barrelToRotate;
    [SerializeField] private Transform minigunBarrel;
    [SerializeField] private Transform rifleBarrel;
    private GameObject carCollision;
    [SerializeField] GameObject rpgGo;
    [SerializeField] GameObject rocketspawn;
    [SerializeField] GameObject rocket;
    [SerializeField] TMP_Text rpgcount;
    Rigidbody rb;
    [SerializeField] private Transform gunBarrel; //barrel that is going to rotate to face the correct direction
    [SerializeField] private GameObject bulletSpawnPoint;
    [SerializeField] GameObject MinigunHolder;
    [SerializeField] GameObject RifleHolder;
    [SerializeField] GameObject MinigunIcon, RifleIcon;
<<<<<<< Updated upstream

=======
    LayerMask everythingButIgnoreBullets;
    [SerializeField] public ParticleSystem muzzleFlash;
>>>>>>> Stashed changes
    #endregion

    #region Camera
    [Header("Camera GameObjects")]
    [SerializeField] private CinemachineVirtualCamera cineCamera;
    #endregion

    #region Bools
    [Header("Bools")]
    public bool connectCar = false;
    public bool RPG;
    [SerializeField] private bool pickedUpRPG = false;
    private bool currentlyShooting;
    private bool shootButtonHeld;
    public bool isShooting { get { return currentlyShooting; } private set { isShooting = currentlyShooting; } }
    public bool isPressingShootbutton { get { return shootButtonHeld; } private set { isPressingShootbutton = shootButtonHeld; } }
    [SerializeField] bool noCarNeeded;
    bool usingAmmo;
    bool rotate;
    #endregion

    #region Floats
    [Header("Floats")]
    [SerializeField] float playerNumber = 1;
    [SerializeField] private float minRotationHeight = -20f, maxRotationHeight = 20f;
    private float xAngle, yAngle; //angle of rotation for the gun axis
    [SerializeField] private float horizontalRotationSpeed = 5f, verticalRotationSpeed = 3f; //rotation speeds for the gun
    private float fireCooldown;
    float fireRate;
    private float weaponDamage;
    private float weaponRange;
    #endregion

    #region Spread
    [Header("Bullet Spread")]
    private float maxBulletDeviation;
    private float maxCrosshairDeviation;
    private float bulletDeviationIncrease;
    private float crosshairDeviationIncrease;
    [SerializeField] float currentBulletSpread = 0;
    [SerializeField] float currentCrosshairSpread = 0;

    #endregion

    #region Input
    [Header("Inputs")]
    [SerializeField] private KeyCode shootButton = KeyCode.Mouse0;
    [SerializeField] private KeyCode RPGButton = KeyCode.Tab;
    [SerializeField] private KeyCode changeWeapon = KeyCode.Mouse1;
    #endregion

    #region Weapons
    [Header("Weapons")]
    [SerializeField] private float amountOfAmmoForRPG = 3;
    private float startAmountOfAmmoForRPG;
    [SerializeField] private Weapon minigunData;
    [SerializeField] private Weapon rifleData;
    Weapon data;
    #endregion

    #region MinigunSpecific
    private float barrelRotationSpeed;
    private float barrelRotationStartSpeed = 100f, barrelRotationMaxSpeed = 800f;
    private float amountOfAmmoForCooldownBar = 1000;
    private float startAmmo; //the amount of ammo for the cooldown bar at the start of the game
    private float ammoNormalized; //normalized the ammo value to be between 0 and 1 for the cooldown bar scale
    #endregion

    #region BulletTrails
    [Header("Bullet Trails")]
    [SerializeField] GameObject trail;
    float trailPercentage;
    #endregion

    #region Photon
    private PhotonView pv;
    #endregion

    #region Scripts
    Pause pauseMenu;
    Controller carController;
    AICarController aiCarController;
    Minig2un minigunScript;
    Rifle rifleScript;
    #endregion

    #region Bullet Decoration
    [Header("Bullet Decoration")]
    public GameObject impactEffect;
    [SerializeField] private Transform impactEffectHolder;
    

    [SerializeField] GameObject CasingSpawn, Casing;

    [SerializeField] Image hitmarker;
    #endregion

    #region ShooterClass
    public enum ShooterClass { minigun, rifle};
    ShooterClass shooterClass;
    public ShooterClass currentShooterClass { get { return shooterClass; } private set { currentShooterClass = shooterClass; } }
    ShooterClass previousShooterClass;
    #endregion

    #region Sound
    string sound;
    string bulletWhistle;
    #endregion

    #region Crosshair
    [Header("Crosshair")]
    [SerializeField] RectTransform reticle; // The RecTransform of reticle UI element.
    [SerializeField] GameObject CrossHairGameobject;
    public float restingSize;
    public float maxSize;
    public float speed;
    public float currentSize;
    private float spreadSize;
    #endregion

    #region UI
    [SerializeField] private Transform coolDownBarUi; //ui bar that shows the cooldown of the minigun
    #endregion

    void Start()
    {
        hitmarker.ChangeAlpha(0);
        pauseMenu = GetComponent<Pause>();
        pv = GetComponent<PhotonView>();
        startAmountOfAmmoForRPG = amountOfAmmoForRPG;
        rb = GetComponent<Rigidbody>();
        previousShooterClass = shooterClass;
        SetupGun(currentShooterClass);
        fireCooldown = fireCooldown - fireRate;

        startAmmo = amountOfAmmoForCooldownBar;
        barrelRotationSpeed = barrelRotationStartSpeed;
    }


    void SetupGun(ShooterClass shooterClass)
    {
<<<<<<< Updated upstream
        MinigunHolder.SetActive(true);
        RifleHolder.SetActive(false);
        minigunScript = GetComponent<Minigun>();
        muzzleFlash = minigunScript.muzzleFlash;
        weaponDamage = minigunScript.minigunDamage;
        weaponRange = minigunScript.range;
        fireRate = minigunScript.minigunFireRate;
        sound = minigunScript.sound;
        bulletWhistle = minigunScript.bulletWhistle;
        maxBulletDeviation = minigunScript.maxBulletDeviation;
        maxCrosshairDeviation = minigunScript.maxCrosshairDeviation;
        bulletDeviationIncrease = minigunScript.bulletDeviationIncrease;
        crosshairDeviationIncrease = minigunScript.crosshairDeviationIncrease;
        usingAmmo = true;
        MinigunIcon.SetActive(true);
        RifleIcon.SetActive(false);
    }

    void SetupRifle()
    {
        MinigunHolder.SetActive(false);
        RifleHolder.SetActive(true);
        rifleScript = GetComponent<Rifle>();
        muzzleFlash = rifleScript.muzzleFlash;
        weaponDamage = rifleScript.rifleDamage;
        weaponRange = rifleScript.range;
        fireRate = rifleScript.rifleFireRate;
        sound = rifleScript.sound;
        bulletWhistle = rifleScript.bulletWhistle;
        maxBulletDeviation = rifleScript.maxBulletDeviation;
        maxCrosshairDeviation = rifleScript.maxCrosshairDeviation;
        bulletDeviationIncrease = rifleScript.bulletDeviationIncrease;
        crosshairDeviationIncrease = rifleScript.crosshairDeviationIncrease;
        usingAmmo = false;
        MinigunIcon.SetActive(false);
        RifleIcon.SetActive(true);
=======
        //What weapon to switch to 
        switch (shooterClass)
        {
            case ShooterClass.minigun:
                data = minigunData;
                barrelToRotate = minigunBarrel;
                MinigunHolder.SetActive(true);
                RifleHolder.SetActive(false);
                MinigunIcon.SetActive(true);
                RifleIcon.SetActive(false);
                break;
            case ShooterClass.rifle:
                data = rifleData;
                barrelToRotate = rifleBarrel;
                MinigunHolder.SetActive(false);
                RifleHolder.SetActive(true);
                MinigunIcon.SetActive(false);
                RifleIcon.SetActive(true);
                break;
            default:
                data = minigunData;
                barrelToRotate = minigunBarrel;
                MinigunHolder.SetActive(true);
                RifleHolder.SetActive(false);
                MinigunIcon.SetActive(true);
                RifleIcon.SetActive(false);
                break;
        }

        //Assign values
        weaponDamage = data.damage;
        weaponRange = data.range;
        fireRate = data.fireRate;
        sound = data.sound;
        bulletWhistle = data.bulletWhistle;
        hitmarkerSound = data.hitmarker;
        maxBulletDeviation = data.maxBulletDeviation;
        maxCrosshairDeviation = data.maxCrosshairDeviation;
        bulletDeviationIncrease = data.bulletDeviationIncrease;
        crosshairDeviationIncrease = data.crosshairDeviationIncrease;
        trailPercentage = data.trailPercentage;
        usingAmmo = data.usingAmmo;
        rotate = data.rotate;
>>>>>>> Stashed changes
    }


    void Update()
    {
<<<<<<< Updated upstream
=======
        //Run General Functions
        CrossHair();
        Hitmarker();
        RotateGunBarrel();
        CooldownBarValues();
        ammoNormalized = amountOfAmmoForCooldownBar / startAmmo; //normalized the ammo value to be between 0 and 1 for the cooldown bar scale
        CoolDownBar(ammoNormalized); //scale the size of the cooldown bar to match the ammo count

        //Pause Menu
>>>>>>> Stashed changes
        if (pauseMenu.paused) { return; }

        //Check to see if we're offline, or if we're online check we are only recieveing instructions from the local player
        if (pv.IsMine && IsThisMultiplayer.Instance.multiplayer || !IsThisMultiplayer.Instance.multiplayer)
        {
            if (connectCar && GetComponentInParent<MoveTurretPosition>() != null)
            {
                connectCar = false;
                car = GetComponentInParent<MoveTurretPosition>().car;
                carCollision = GetComponentInParent<MoveTurretPosition>().car.transform.GetChild(0).gameObject;
            }
            FollowMouse();
            rpgcount.text = amountOfAmmoForRPG + " / " + startAmountOfAmmoForRPG;


            if (!noCarNeeded)
            {
                if (car != null && car.layer == LayerMask.NameToLayer("Cars"))
                {
                    if (car.layer == LayerMask.NameToLayer("Cars"))
                    {
                        //ONLINE PLAYERS
                        if (car.GetComponent<Controller>())
                        {
                            if (car.GetComponent<CarPickup>().hasRPG)
                                RPG = true;

                            carController = car.GetComponent<Controller>();

                            if (amountOfAmmoForRPG <= 0 && car.GetComponent<CarPickup>().hasRPG)
                            {
                                RPG = false;
                                car.GetComponent<CarPickup>().hasRPG = false;
                                amountOfAmmoForRPG = startAmountOfAmmoForRPG;
                            }
                        }

                        //AI CARS
                        if (car.GetComponent<AICarController>())
                        {
                            //Debug.Log("Put AI car RPG STUFF HERE");///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


                            if (car.GetComponent<AICarPickups>().hasRPG)
                            {
                                RPG = true; 
                            }

                            aiCarController = car.GetComponent<AICarController>();

                            if (amountOfAmmoForRPG <= 0 && car.GetComponent<AICarPickups>().hasRPG)
                            {
                                RPG = false;
                                car.GetComponent<AICarPickups>().hasRPG = false;
                                amountOfAmmoForRPG = startAmountOfAmmoForRPG;
                            }
                        }
                    }

                }
            }
        }

        //Shooting
        if ((pv.IsMine && IsThisMultiplayer.Instance.multiplayer )|| !IsThisMultiplayer.Instance.multiplayer)
        {
            //Change Weapons
            if (Input.GetKeyDown(changeWeapon) && ((pv.IsMine && IsThisMultiplayer.Instance.multiplayer) || !IsThisMultiplayer.Instance.multiplayer))
            {
                
                if (shooterClass == ShooterClass.minigun)
                {
                    SetupGun(ShooterClass.rifle);
                    shooterClass = ShooterClass.rifle;
                }
                else if (shooterClass == ShooterClass.rifle)
                {
                    SetupGun(ShooterClass.minigun);
                    shooterClass = ShooterClass.minigun;
                }
                previousShooterClass = shooterClass;
                currentCrosshairSpread = 0;
            }
<<<<<<< Updated upstream
=======
            if (!RPG)
            {
                if(IsThisMultiplayer.Instance.multiplayer)
                {
                    pv.RPC("HideRPG", RpcTarget.All);
                }
                else
                {
                    rpgGo.SetActive(false);
                }
            }

            //Wait for weapons Free
            if (!MasterClientRaceStart.Instance.weaponsFree) { return; }
            
            //Check to see if shootButton is held down for crosshair
>>>>>>> Stashed changes
            if (Input.GetKey(shootButton) && pv.IsMine && usingAmmo)
            {
                shootButtonHeld = true;
            }
            else
            {
                shootButtonHeld = false;
            }

<<<<<<< Updated upstream
            if (!RPG)
            {
                pv.RPC("HideRPG", RpcTarget.All);
            }

            //Wait for weapons Free
            if (!MasterClientRaceStart.Instance.weaponsFree) { return; }
=======
>>>>>>> Stashed changes
            if (shooterClass == ShooterClass.minigun)
            {
                //if you are shooting and have ammo (MINIGUN)
                if (Input.GetKey(shootButton) && amountOfAmmoForCooldownBar > 0 && !RPG)
                {

                    if (Time.time >= fireCooldown + fireRate)
                    {
                        currentlyShooting = true;
                        if (currentBulletSpread < maxBulletDeviation)
                        {
                            currentBulletSpread += bulletDeviationIncrease;
                        }

                        if(currentCrosshairSpread < maxCrosshairDeviation)
                        {
                            currentCrosshairSpread += crosshairDeviationIncrease;
                        }
                        //pv.RPC("Shoot", RpcTarget.All);
                        Shoot();
                        fireCooldown = Time.time;
                    }
                }
                else
                {
                    currentlyShooting = false;
                    if (currentBulletSpread > 0)
                    {
                        currentBulletSpread -= bulletDeviationIncrease;
                    }
                    if (currentCrosshairSpread > 0)
                    {
                        currentCrosshairSpread -= crosshairDeviationIncrease;
                    }
                }
            }

            if (shooterClass == ShooterClass.rifle)
            {
                if (Input.GetKeyDown(shootButton) && !RPG)
                {

                    if (Time.time >= fireCooldown + fireRate)
                    {
                        currentlyShooting = true;
                        if (currentBulletSpread < maxBulletDeviation)
                        {
                            currentBulletSpread += bulletDeviationIncrease;
                        }

                        if (currentCrosshairSpread < maxCrosshairDeviation)
                        {
                            currentCrosshairSpread += crosshairDeviationIncrease;
                        }
                        //pv.RPC("Shoot", RpcTarget.All);
                        Shoot();
                        fireCooldown = Time.time;
                    }
                }
                else
                {
                    currentlyShooting = false;
                    if (currentBulletSpread > 0)
                    {
                        currentBulletSpread -= bulletDeviationIncrease;
                    }
                    if (currentCrosshairSpread > 0)
                    {
                        currentCrosshairSpread -= crosshairDeviationIncrease;
                    }
                }
            }

            if(RPG)
            {
                if (IsThisMultiplayer.Instance.multiplayer)
                {
                    pv.RPC("ShowRPG", RpcTarget.All);
                }
                else
                {
                    rpgGo.SetActive(true);
                }
                if (Input.GetKeyDown(shootButton) && amountOfAmmoForRPG > 0)
                {
                    amountOfAmmoForRPG--;
                    if (IsThisMultiplayer.Instance.multiplayer)
                    {
                        pv.RPC("ShootRPG", RpcTarget.All);
                    }
                    else
                    {
                        OfflineShootRPG();
                    }
                }
                if (amountOfAmmoForRPG <= 0)
                {
                    RPG = false;
                    this.GetComponent<ShooterPickup>().hasRPG = false;
                    amountOfAmmoForRPG = startAmountOfAmmoForRPG;
                }
            }
            
        }
          
        
        if (Input.GetKeyDown(RPGButton) && pickedUpRPG)
        {
            //RPG = !RPG;
        }
        CrossHair();
        Hitmarker();
    }

    void CrossHair()
    {
<<<<<<< Updated upstream
        if (pauseMenu.paused)
=======
        if (pauseMenu.paused || (!MasterClientRaceStart.Instance.weaponsFree && IsThisMultiplayer.Instance.multiplayer))
>>>>>>> Stashed changes
        {
            CrossHairGameobject.SetActive(false);
        }
        else
        {
            CrossHairGameobject.SetActive(true);
        }
        if (isShooting)
        {
            spreadSize = currentCrosshairSpread * 10 + restingSize;
            if (spreadSize < restingSize)
            {
                spreadSize = restingSize;
            }
            currentSize = Mathf.Lerp(currentSize, spreadSize, Time.deltaTime * speed);
        }
        else
        {
            currentSize = Mathf.Lerp(currentSize, restingSize, Time.deltaTime * speed);
        }

        reticle.sizeDelta = new Vector2(currentSize, currentSize);

    }

    #region RPG Functions

    [PunRPC]
    void ShowRPG()
    {
        rpgGo.SetActive(true);
    }

    [PunRPC]
    void HideRPG()
    {
        rpgGo.SetActive(false);
    }

    void OfflineShootRPG()
    {
        FMODUnity.RuntimeManager.PlayOneShotAttached("event:/GunFX/RPG/RPGFire", gameObject);
        GameObject grenade = Instantiate(rocket, rocketspawn.transform.position, rpgGo.transform.rotation);
        grenade.GetComponent<Rigidbody>().AddForce(rpgGo.transform.transform.forward * 100, ForceMode.Impulse);
    }

    [PunRPC]
    void ShootRPG()
    {
        if (pv.IsMine)
        {
            FMODUnity.RuntimeManager.PlayOneShotAttached("event:/GunFX/RPG/RPGFire", gameObject);
            GameObject grenade = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Grenade"), rocketspawn.transform.position, rpgGo.transform.rotation, 0);
            grenade.GetComponent<Rigidbody>().AddForce(rpgGo.transform.transform.forward * 100, ForceMode.Impulse);
        }
    }

    #endregion

    private void FollowMouse()
    {
        xAngle += Input.GetAxis("Mouse X") * horizontalRotationSpeed * Time.deltaTime;
        //xAngle = Mathf.Clamp(xAngle, 0, 180); //use this if you want to clamp the rotation. second var = min angle, third var = max angle

        yAngle += Input.GetAxis("Mouse Y") * verticalRotationSpeed * -Time.deltaTime;
        yAngle = Mathf.Clamp(yAngle, minRotationHeight, maxRotationHeight); //use this if you want to clamp the rotation. second var = min angle, third var = max angle

        gunBarrel.localRotation = Quaternion.Euler(yAngle, xAngle, 0);
    }

    void Hitmarker()
    {
        var tempColor = hitmarker.color;
        tempColor.a -= 0.1f;
        hitmarker.color = tempColor;
    }


    Vector3 Spread(float maxDeviation)
    {
        Vector3 forwardVector = cineCamera.transform.forward;
        float deviation = Random.Range(0f, maxDeviation);
        float angle = Random.Range(0f, 360f);
        forwardVector = Quaternion.AngleAxis(deviation, cineCamera.transform.up) * forwardVector;
        forwardVector = Quaternion.AngleAxis(angle, cineCamera.transform.forward) * forwardVector;
        return forwardVector;
    }
 

    #region Shooting Functions
    
    void Shoot()
    {
        muzzleFlash.Play();
        FMODUnity.RuntimeManager.PlayOneShotAttached(sound, gameObject);

        Vector3 direction = Spread(currentBulletSpread);
        GameObject bulletCasingGO;

        if (IsThisMultiplayer.Instance.multiplayer)
        {
           bulletCasingGO = PhotonNetwork.Instantiate("BulletCasing", CasingSpawn.transform.position, CasingSpawn.transform.rotation);
        }
        else
        {
            bulletCasingGO = Instantiate(Casing, CasingSpawn.transform.position, CasingSpawn.transform.rotation);
        }
        

        if (!noCarNeeded)
        {
            if (car.GetComponent<Controller>())
            {
                bulletCasingGO.GetComponent<Rigidbody>().velocity = carController.rb.velocity;
            }
            else
            {
                bulletCasingGO.GetComponent<Rigidbody>().velocity = aiCarController.rb.velocity;
            }

            bulletCasingGO.GetComponent<Rigidbody>().AddForce((bulletCasingGO.transform.right + (bulletCasingGO.transform.up * 2)) * 0.3f, ForceMode.Impulse);
        }
        RaycastHit hit; 
        if (Physics.Raycast(cineCamera.transform.position, direction, out hit, weaponRange))
        {
            Target target = hit.transform.GetComponent<Target>();
            if (target != null && target.gameObject != car)
            {
                target.TakeDamage(weaponDamage);
            }

            GameObject impactGo;
            if (IsThisMultiplayer.Instance.multiplayer)
            {
                impactGo = PhotonNetwork.Instantiate("Impact Particle Effect", hit.point, Quaternion.LookRotation(hit.normal), 0);
            }
            else
<<<<<<< Updated upstream
                a.GetComponent<Rigidbody>().velocity = aiCarController.rb.velocity;

            a.GetComponent<Rigidbody>().AddForce((a.transform.right + (a.transform.up * 2)) * 0.3f, ForceMode.Impulse);
        }
        RaycastHit hit; //gets the information on whats hit
        if (Physics.Raycast(cineCamera.transform.position, direction, out hit, weaponRange))
        {
            Target target = hit.transform.GetComponent<Target>();
            if (target != null && target.gameObject != car)
            {
                target.TakeDamage(weaponDamage);
                var tempColor = hitmarker.color;
                tempColor.a = 1f;
                hitmarker.color = tempColor;
=======
            {
                impactGo = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
>>>>>>> Stashed changes
            }
            impactGo.transform.parent = impactEffectHolder;
        }

        float chance = Random.Range(0, 100);
        if (chance <= trailPercentage)
        {
            GameObject trailGO;
            if (IsThisMultiplayer.Instance.multiplayer)
            {
                trailGO = PhotonNetwork.Instantiate("Trail", bulletSpawnPoint.transform.position, bulletSpawnPoint.transform.rotation);
            }
            else
            {
                trailGO = Instantiate(trail, bulletSpawnPoint.transform.position, bulletSpawnPoint.transform.rotation);
            }
             
            FMODUnity.RuntimeManager.PlayOneShotAttached(bulletWhistle, trailGO);
            if (!noCarNeeded)
            {
                if (car.GetComponent<Controller>() && pv.IsMine)
                {
                    trailGO.GetComponent<Rigidbody>().velocity = carController.rb.velocity;
                }
                else
                    trailGO.GetComponent<Rigidbody>().velocity = aiCarController.rb.velocity;
            }

            trailGO.GetComponent<Rigidbody>().AddForce(trailGO.transform.forward * 100, ForceMode.Impulse);
            trailGO.GetComponent<DeleteMe>().enabled = true;
        }

    }
    #endregion


    #region BulletImpacts
    private IEnumerator DeleteImpactEffect(float time)
    {
        yield return new WaitForSeconds(time);

    }

    #endregion

    #region MinigunStuff
    private void RotateGunBarrel()
    {
        if (rotate)
        {
            barrelToRotate.Rotate(0, 0, barrelRotationSpeed * Time.deltaTime);
        }
    }

    private void CoolDownBar(float sizeNormalized)
    {
        coolDownBarUi.localScale = new Vector3(sizeNormalized, 1f); //scale the ui cooldown bar to match the ammo count
    }

    void CooldownBarValues()
    {
        //if you are shooting and have ammo
        if (amountOfAmmoForCooldownBar > 0 && isPressingShootbutton)
        {
            amountOfAmmoForCooldownBar--;
            barrelRotationSpeed = barrelRotationMaxSpeed;
        }
        else
            barrelRotationSpeed = barrelRotationStartSpeed;

        //if you are not shooting and the ammo isnt full
        if (amountOfAmmoForCooldownBar < startAmmo && !isPressingShootbutton)
        {
            amountOfAmmoForCooldownBar++;
        }
    }

    #endregion

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "car")
        {
            car = other.gameObject;
        }
    }
}
