using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Cinemachine;
using TMPro;
using System.IO;
using UnityEngine.UI;

public class Shooter : MonoBehaviourPun
{
    public GameObject car { get; private set; }
    #region General GameObjects
    [Header("General GameObjects")]

    
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
    [SerializeField] LayerMask everythingButIgnoreBullets;
    //[SerializeField] public ParticleSystem muzzleFlash;
    [SerializeField] private GameObject muzzleFlash;

    #endregion

    #region Camera  
    public CinemachineVirtualCamera cineCamera { get; private set; }
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
    bool shootingDecorationStuffOn = false;
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
    private float weaponAmmoUsage;
    float mouseScrollFactor;
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
    private string[] Controllers;
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

    #region RifleSpecific
    [SerializeField] GameObject scopeOverlay;
    [SerializeField] GameObject UI;
    bool isScoped;

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
    //Pause pauseMenu;
    PauseMenu pauseMenu;
    Controller carController;
    AICarController aiCarController;
    #endregion

    #region Bullet Decoration
    [Header("Bullet Decoration")]
    public GameObject impactEffect;
    [SerializeField] private Transform impactEffectHolder;

    [SerializeField] GameObject CasingSpawn, Casing;
    [SerializeField] GameObject VFXBulletGo;

    [SerializeField] Image hitmarker;
    #endregion

    #region ShooterClass
    [SerializeField] ShooterClass shooterClass;
    public MinigunClass minigunClass;
    public ShooterClass currentShooterClass { get { return shooterClass; } private set { currentShooterClass = shooterClass; } }
    ShooterClass previousShooterClass;
    #endregion

    #region Sound
    FMOD.Studio.EventInstance minigunLoopSoundInstance;
    FMOD.Studio.EventInstance rifleSoundInstance;
    string rifleSoundString = "event:/GunFX/Rifle/RifleShot2";
    string sound;
    string bulletWhistle;
    string hitmarkerSound;
    #endregion

    #region Crosshair
    [Header("Crosshair")]
    [SerializeField] RectTransform reticle; // The RecTransform of reticle UI element.
    [SerializeField] GameObject CrossHairGameobject;
    float restingSize = 50;
    float maxSize;
    private float increaseSpeed;
    private float resetSpeed;
    [SerializeField] float currentSize;
    [SerializeField] private float spreadSize;
    float crosshairWaitTime;
    #endregion

    #region UI
    [SerializeField] private Transform coolDownBarUi; //ui bar that shows the cooldown of the minigun
    #endregion

    #region Golden
    [Header("GoldenGun")]
    [SerializeField] Material gold;
    [SerializeField] GameObject gunHolder;
    [SerializeField] GameObject standHolder;
    MeshRenderer[] RenderersG;
    MeshRenderer[] RenderersS;
    #endregion


    private void Awake()
    {
        RenderersG = gunHolder.GetComponentsInChildren<MeshRenderer>();
        RenderersS = standHolder.GetComponentsInChildren<MeshRenderer>();
        //pauseMenu = GetComponent<Pause>();
        pauseMenu = GetComponent<PauseMenu>();
        cineCamera = transform.GetChild(0).GetChild(0).GetChild(0).gameObject.GetComponent<CinemachineVirtualCamera>();
        pv = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody>();
        VFXBulletGo.SetActive(false);
        muzzleFlash.SetActive(false);
        minigunLoopSoundInstance = FMODUnity.RuntimeManager.CreateInstance("event:/GunFX/Minigun/MinigunLoop");
        rifleSoundInstance = FMODUnity.RuntimeManager.CreateInstance(rifleSoundString);
        //FMODUnity.RuntimeManager.AttachInstanceToGameObject(minigunLoopSoundInstance, transform, rb);
    }

    void Start()
    {
        hitmarker.ChangeAlpha(0);
        startAmountOfAmmoForRPG = amountOfAmmoForRPG; 
        previousShooterClass = shooterClass;
        SetupGun(currentShooterClass);
        startAmmo = amountOfAmmoForCooldownBar;
        barrelRotationSpeed = barrelRotationStartSpeed;
        if (minigunClass == MinigunClass.gold)
        {
            foreach (MeshRenderer mr in RenderersG)
            {
                mr.material = gold;
            }
            foreach (MeshRenderer mr in RenderersS)
            {
                //mr.material = gold;
            }
        }
    }

    



    void SetupGun(ShooterClass shooterClass)
    {
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
        crosshairWaitTime = data.crosshairWaitTime;
        increaseSpeed = data.crossshairIncreaseSpeed;
        resetSpeed = data.crossshairResetSpeed;
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
        weaponAmmoUsage = data.ammoUsage;
        usingAmmo = data.usingAmmo;
        rotate = data.rotate;
        fireCooldown = fireCooldown - fireRate;
    }

    private void FixedUpdate()
    {
        if (pv.IsMine && IsThisMultiplayer.Instance.multiplayer || !IsThisMultiplayer.Instance.multiplayer)
        {
            //Run General Functions
            //BulletCasing();
            CrossHair();
            Hitmarker();
            RotateGunBarrel();
            CooldownBarValues();
            ammoNormalized = amountOfAmmoForCooldownBar / startAmmo; //normalized the ammo value to be between 0 and 1 for the cooldown bar scale
            CoolDownBar(ammoNormalized); //scale the size of the cooldown bar to match the ammo count
        }
    }

    void Update()
    {
        Controllers = Input.GetJoystickNames();
        //Pause Menu
        if (pauseMenu.paused) { return; }

        //Check to see if we're offline, or if we're online check we are only recieveing instructions from the local player
        if (pv.IsMine && IsThisMultiplayer.Instance.multiplayer || !IsThisMultiplayer.Instance.multiplayer)
        {
            
            if (connectCar && GetComponentInParent<MoveTurretPosition>() != null)
            {
                connectCar = false;
                car = GetComponentInParent<MoveTurretPosition>().car;
                carCollision = GetComponentInParent<MoveTurretPosition>().car.transform.GetChild(0).gameObject;

                //FMODUnity.RuntimeManager.AttachInstanceToGameObject(minigunLoopSoundInstance, this.transform, car.GetComponent<Rigidbody>());
            }

            FollowMouse();
            rpgcount.text = amountOfAmmoForRPG + " / " + startAmountOfAmmoForRPG;


            if (!noCarNeeded)
            {
                if (car != null && car.layer == LayerMask.NameToLayer("Cars"))
                {
                    if (car.layer == LayerMask.NameToLayer("Cars"))
                    {
                        //PLAYERS
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
        if ((pv.IsMine && IsThisMultiplayer.Instance.multiplayer) || !IsThisMultiplayer.Instance.multiplayer)
        {
            //Change Weapons
            {
                if (Input.GetButtonDown("Y") && !isScoped)
                {
                    if (shooterClass == ShooterClass.minigun)
                    {
                        SetupGun(ShooterClass.rifle);
                        shooterClass = ShooterClass.rifle;
                        currentCrosshairSpread = 0;
                    }
                    else if (shooterClass == ShooterClass.rifle)
                    {
                        SetupGun(ShooterClass.minigun);
                        shooterClass = ShooterClass.minigun;
                        currentCrosshairSpread = 0;
                    }
                }
                else
                {
                    mouseScrollFactor = Input.mouseScrollDelta.y;
                    if (mouseScrollFactor < 0 && shooterClass == ShooterClass.minigun && !isScoped)
                    {
                        SetupGun(ShooterClass.rifle);
                        shooterClass = ShooterClass.rifle;
                        currentCrosshairSpread = 0;
                    }
                    else if (mouseScrollFactor > 0 && shooterClass == ShooterClass.rifle && !isScoped)
                    {
                        SetupGun(ShooterClass.minigun);
                        shooterClass = ShooterClass.minigun;
                        currentCrosshairSpread = 0;
                    }
                    mouseScrollFactor = 0;
                }
            }
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
            if ((Input.GetAxis("RT") > 0 || Input.GetKey(shootButton)) && usingAmmo && (pv.IsMine || !IsThisMultiplayer.Instance.multiplayer))
            {
                shootButtonHeld = true;
            }
            else
            {
                shootButtonHeld = false;
            }

            if (!RPG)
            {
                if (IsThisMultiplayer.Instance.multiplayer)
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

            
            // Weapon Specific functions <-------------------------------------------------------------- MINIGUN DECORATIONS / AUDIO
            if (shooterClass == ShooterClass.minigun)
            {
                //if you are shooting the minigun
                if (shootButtonHeld && !RPG)
                {
                    if(!shootingDecorationStuffOn)
                    {
                        barrelRotationSpeed = barrelRotationMaxSpeed;
                        VFXBulletGo.SetActive(true);
                        muzzleFlash.SetActive(true);

                        FMODUnity.RuntimeManager.AttachInstanceToGameObject(minigunLoopSoundInstance, transform, rb);
                        minigunLoopSoundInstance.setParameterByName("on", 0);
                        minigunLoopSoundInstance.start();

                        shootingDecorationStuffOn = true;
                    }
                }
                else
                {
                    if(shootingDecorationStuffOn)
                    {
                        barrelRotationSpeed = barrelRotationStartSpeed;
                        VFXBulletGo.SetActive(false);
                        muzzleFlash.SetActive(false);
                        minigunLoopSoundInstance.setParameterByName("on", 1);

                        shootingDecorationStuffOn = false;
                    }
                }
            }
            //---------------------------------------------------------------------------------------------------------------------


            if (shooterClass == ShooterClass.rifle)
            {
                //shooting Rifle
                if(Input.GetKey(shootButton) && amountOfAmmoForCooldownBar > weaponAmmoUsage && !RPG && Time.time >= fireCooldown + fireRate)
                {
                    Debug.Log("Playing rifle sound");

                    //play rifle shot sound
                    FMODUnity.RuntimeManager.PlayOneShot(rifleSoundString, transform.position);
                    //FMODUnity.RuntimeManager.AttachInstanceToGameObject(rifleSoundInstance, transform, rb);
                    //minigunLoopSoundInstance.start();
                }

                if(Input.GetAxis("LT") > 0 || Input.GetKey(KeyCode.Mouse1))
                {
                    scopeOverlay.SetActive(true);
                    RifleHolder.SetActive(false);
                    UI.SetActive(false);
                    isScoped = true;
                    cineCamera.m_Lens.FieldOfView = 20;
                }
                else
                {
                    scopeOverlay.SetActive(false);
                    RifleHolder.SetActive(true);
                    UI.SetActive(true);
                    isScoped = false;
                    cineCamera.m_Lens.FieldOfView = 60;
                }
            }

            //if you are shooting and have ammo 
            if (shootButtonHeld && amountOfAmmoForCooldownBar > weaponAmmoUsage && !RPG && Time.time >= fireCooldown + fireRate)
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
                Shoot();
                amountOfAmmoForCooldownBar -= weaponAmmoUsage;
                fireCooldown = Time.time;
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

            


            if (RPG)
            {
                if (IsThisMultiplayer.Instance.multiplayer)
                {
                    pv.RPC("ShowRPG", RpcTarget.All);
                }
                else
                {
                    rpgGo.SetActive(true);
                }
                if (shootButtonHeld && amountOfAmmoForRPG > 0 && Time.time >= fireCooldown + 1)
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
                    fireCooldown = Time.time;
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
            RPG = !RPG;
        }
    }
    
    void BulletCasing()
    {
        if (!MasterClientRaceStart.Instance.weaponsFree) { return; }

        //bulletcasing
        if (shootButtonHeld && amountOfAmmoForCooldownBar > weaponAmmoUsage && !RPG)
        {
            VFXBulletGo.SetActive(true);

            bool on = false;
            if(!on)
            {
                minigunLoopSoundInstance.setParameterByName("on", 0);
                minigunLoopSoundInstance.start();
                on = true;
            }
            
        }
        else
        {
            VFXBulletGo.SetActive(false);
            minigunLoopSoundInstance.setParameterByName("on", 1);
        }
    }

    private void FollowMouse()
    {


        xAngle += Input.GetAxis("Horizontal") * 2 * horizontalRotationSpeed * Time.deltaTime;
        //xAngle = Mathf.Clamp(xAngle, 0, 180); //use this if you want to clamp the rotation. second var = min angle, third var = max angle

        xAngle += Input.GetAxis("HorizontalB") * 2 * horizontalRotationSpeed * Time.deltaTime;
        //xAngle = Mathf.Clamp(xAngle, 0, 180); //use this if you want to clamp the rotation. second var = min angle, third var = max angle

        xAngle += Input.GetAxis("Mouse X") * horizontalRotationSpeed * Time.deltaTime;
        //xAngle = Mathf.Clamp(xAngle, 0, 180); //use this if you want to clamp the rotation. second var = min angle, third var = max angle

        yAngle += Input.GetAxis("Vertical") * 2 * verticalRotationSpeed * -Time.deltaTime;

        yAngle += Input.GetAxis("VerticalB") * 2 * verticalRotationSpeed * -Time.deltaTime;

        yAngle += Input.GetAxis("Mouse Y") * verticalRotationSpeed * -Time.deltaTime;
        

        yAngle = Mathf.Clamp(yAngle, minRotationHeight, maxRotationHeight); //use this if you want to clamp the rotation. second var = min angle, third var = max angle

        gunBarrel.localRotation = Quaternion.Euler(yAngle, xAngle, 0);
    }

    #region UI

    void CrossHair()
    {
        if (pauseMenu.paused || (!MasterClientRaceStart.Instance.weaponsFree && IsThisMultiplayer.Instance.multiplayer))
        {
            CrossHairGameobject.SetActive(false);
        }
        else
        {
            CrossHairGameobject.SetActive(true);
        }

        if (currentlyShooting )
        {
            spreadSize = currentCrosshairSpread * 10 + restingSize;
            if (spreadSize > maxCrosshairDeviation)
            {
                spreadSize = maxCrosshairDeviation;
            }
            currentSize = Mathf.Lerp(currentSize, spreadSize, Time.deltaTime * increaseSpeed);
        }
        else if (Time.time >= fireCooldown + crosshairWaitTime)
        {
            currentSize = Mathf.Lerp(currentSize, restingSize, Time.deltaTime * resetSpeed);
        }

        reticle.sizeDelta = new Vector2(currentSize, currentSize);

    }

    void Hitmarker()
    {
        hitmarker.SubtractAlpha(0.1f);
    }

    private void CoolDownBar(float sizeNormalized)
    {
        coolDownBarUi.localScale = new Vector3(sizeNormalized, 1f); //scale the ui cooldown bar to match the ammo count
    }

    void CooldownBarValues()
    {
        //if you are not shooting and the ammo isnt full
        if (amountOfAmmoForCooldownBar < startAmmo && !isShooting && Time.time >= fireCooldown + crosshairWaitTime)
        {
            amountOfAmmoForCooldownBar++;
        }
    }

    #endregion

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

    #region Shooting Functions

    Vector3 Spread(float maxDeviation)
    {
        Vector3 forwardVector = cineCamera.transform.forward;
        float deviation = Random.Range(0f, maxDeviation);
        float angle = Random.Range(0f, 360f);
        forwardVector = Quaternion.AngleAxis(deviation, cineCamera.transform.up) * forwardVector;
        forwardVector = Quaternion.AngleAxis(angle, cineCamera.transform.forward) * forwardVector;
        return forwardVector;
    }

    void Shoot()
    {
        //muzzleFlash.Play();
        //muzzleFlash.SetActive(true);

        //FMODUnity.RuntimeManager.PlayOneShotAttached(sound, gameObject);

        Vector3 direction = Spread(currentBulletSpread);

        /*
        GameObject bulletCasingGO;

        if (IsThisMultiplayer.Instance.multiplayer)
        {
            bulletCasingGO = PhotonNetwork.Instantiate("BulletCasing", CasingSpawn.transform.position, CasingSpawn.transform.rotation);
        }
        else
        {
            bulletCasingGO = Instantiate(Casing, CasingSpawn.transform.position, CasingSpawn.transform.rotation);
        }
        bulletCasingGO.GetComponent<Rigidbody>().AddForce((bulletCasingGO.transform.right + (bulletCasingGO.transform.up * 2)) * 0.3f, ForceMode.Impulse);

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
        */

        RaycastHit[] hits;

        hits = Physics.RaycastAll(cineCamera.transform.position, direction, weaponRange, everythingButIgnoreBullets);

        foreach (RaycastHit hit in hits)
        {
            if(hit.transform.gameObject.tag == "Explosive Barrel")
            {
                Debug.Log("You Shot an explosive barrel");
                hit.transform.gameObject.GetComponent<ExplosiveBarrel>().TakeDamage();

                //hitmarker
                hitmarker.ChangeAlpha(1);
                FMODUnity.RuntimeManager.PlayOneShot(hitmarkerSound);
            }



            if (hit.transform.gameObject != car && (hit.transform.gameObject.tag == "Player" || hit.transform.gameObject.tag == "car"))
            {
                Target target = hit.transform.GetComponent<Target>();
                if (target != null && target.gameObject != car)
                {
                    //target.TakeDamage(weaponDamage);
                    pv.RPC("RPC_DealDamage", RpcTarget.All, target, weaponDamage);
                }

                GameObject impactGo;
                if (IsThisMultiplayer.Instance.multiplayer)
                {
                    impactGo = PhotonNetwork.Instantiate("Impact Particle Effect", hit.point, Quaternion.LookRotation(hit.normal), 0);
                }
                else
                {
                    impactGo = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
                }
                //impactGo.transform.parent = impactEffectHolder;

                hitmarker.ChangeAlpha(1);
                FMODUnity.RuntimeManager.PlayOneShot(hitmarkerSound);

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
        }
    }

    [PunRPC]
    void RPC_DealDamage(Target target, float amount)
    {
        target.TakeDamage(amount);
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
    #endregion

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "car")
        {
            //car = other.gameObject;
        }
    }
}