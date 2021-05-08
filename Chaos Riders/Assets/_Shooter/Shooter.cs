using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Cinemachine;
using TMPro;
using System.IO;
using UnityEngine.UI;

public class Shooter : MonoBehaviourPun
{
    public GameObject car { get; private set; }
    #region General GameObjects
    [Header("General GameObjects")]
    GameObject trailGO;
    private Transform barrelToRotate;
    [SerializeField] private Transform minigunBarrel;
    [SerializeField] private Transform rifleBarrel;
    private GameObject carCollision;
    [SerializeField] GameObject rpgGo;
    [SerializeField] GameObject rocketspawn;
    [SerializeField] GameObject rocket;
    [SerializeField] Animator anim;
    [SerializeField] TMP_Text rpgcount;
    Rigidbody rb;
    [SerializeField] private Transform stand; //stand that is going to rotate to face the correct direction
    [SerializeField] private Transform gunBarrel; //barrel that is going to rotate to face the correct direction
    [SerializeField] private GameObject bulletSpawnPoint;
    [SerializeField] GameObject MinigunHolder;
    [SerializeField] GameObject RifleHolder;
    [SerializeField] GameObject[] TurnOffForRifle;
    [SerializeField] GameObject MinigunIcon, RifleIcon;
    [SerializeField] LayerMask everythingButIgnoreBullets;
    //[SerializeField] public ParticleSystem muzzleFlash;
    

    #endregion

    #region Camera  
    [SerializeField] CinemachineVirtualCamera CinemachineVirtualCamera;
    public CinemachineVirtualCamera cineCamera { get; private set; }
    #endregion

    #region Bools
    [Header("Bools")]
    public bool connectCar = false;
    public bool RPG;
    [SerializeField] private bool useImpactParticleEffect = false;
    [SerializeField] private bool pickedUpRPG = false;
    private bool currentlyShooting;
    private bool shootButtonHeld;
    public bool isShooting { get { return currentlyShooting; } private set { isShooting = currentlyShooting; } }
    public bool isPressingShootbutton { get { return shootButtonHeld; } private set { isPressingShootbutton = shootButtonHeld; } }
    [SerializeField] bool noCarNeeded;
    bool usingAmmo;
    bool rotate;
    bool shootingDecorationStuffOn = false;

    bool RPGEquipped;
    bool RPGReadytoFire;
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
    [SerializeField] private GameObject muzzleFlash;
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
        cineCamera = CinemachineVirtualCamera;
        pv = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody>();
        VFXBulletGo.SetActive(false);
        muzzleFlash.SetActive(false);
        minigunLoopSoundInstance = FMODUnity.RuntimeManager.CreateInstance("event:/GunFX/Minigun/MinigunLoop");
        rifleSoundInstance = FMODUnity.RuntimeManager.CreateInstance(rifleSoundString);
        //FMODUnity.RuntimeManager.AttachInstanceToGameObject(minigunLoopSoundInstance, transform, rb);
        trailGO = PhotonNetwork.Instantiate("Trail", Vector3.zero, Quaternion.identity);
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
                //mr.material = gold;
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

            //Wait for weapons Free
            if (IsThisMultiplayer.Instance.multiplayer)
            {
                if (!MasterClientRaceStart.Instance.weaponsFree) { return; }
            }
            //Check to see if shootButton is held down for crosshair
            if ((Input.GetAxis("RT") > 0 || Input.GetKey(shootButton)) && usingAmmo && (pv.IsMine || !IsThisMultiplayer.Instance.multiplayer))
            {
                shootButtonHeld = true;
            }
            else
            {
                shootButtonHeld = false;
            }



            //Wait for weapons Free
            if (IsThisMultiplayer.Instance.multiplayer)
            {
                if (!MasterClientRaceStart.Instance.weaponsFree) { return; }
            }

            if (shooterClass == ShooterClass.rifle)
            {
                //shooting Rifle
                if(Input.GetKey(shootButton) && amountOfAmmoForCooldownBar > weaponAmmoUsage && !RPGEquipped && Time.time >= fireCooldown + fireRate)
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
                    TurnOffForRifle[0].SetActive(false);
                    TurnOffForRifle[1].SetActive(false);
                    UI.SetActive(false);
                    isScoped = true;
                    cineCamera.m_Lens.FieldOfView = 20;
                }
                else
                {
                    scopeOverlay.SetActive(false);
                    RifleHolder.SetActive(true);
                    TurnOffForRifle[0].SetActive(true);
                    TurnOffForRifle[1].SetActive(true);
                    UI.SetActive(true);
                    isScoped = false;
                    cineCamera.m_Lens.FieldOfView = 60;
                }
            }

            //if you are shooting and have ammo 
            if (shootButtonHeld && amountOfAmmoForCooldownBar > weaponAmmoUsage && !RPGEquipped && Time.time >= fireCooldown + fireRate)
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


            if (!RPG)
            {
                if (IsThisMultiplayer.Instance.multiplayer)
                {
                    pv.RPC("HideRPG", RpcTarget.All);
                }
                else
                {
                    HideRPG();
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
                    ShowRPG();
                }

                if(Time.time >= fireCooldown + 1.5f)
                {
                    RPGReadytoFire = true;
                }

                if (shootButtonHeld && RPGEquipped)
                {
                    if (RPGReadytoFire && amountOfAmmoForRPG > 0)// && Time.time >= fireCooldown + 1.5f)
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
                        RPGEquipped = false;
                        RPGReadytoFire = false;
                        anim.SetBool("Gun Anim", false);
                        this.GetComponent<ShooterPickup>().hasRPG = false;
                        amountOfAmmoForRPG = startAmountOfAmmoForRPG;
                    }
                }
            }

            // Weapon Specific functions <-------------------------------------------------------------- MINIGUN DECORATIONS / AUDIO
            if (shooterClass == ShooterClass.minigun)
            {
                //if you are shooting the minigun
                if (shootButtonHeld && !RPGEquipped)
                {
                    if (!shootingDecorationStuffOn)
                    {
                        /*
                        barrelRotationSpeed = barrelRotationMaxSpeed;
                        VFXBulletGo.SetActive(true);
                        muzzleFlash.SetActive(true);

                        FMODUnity.RuntimeManager.AttachInstanceToGameObject(minigunLoopSoundInstance, transform, rb);
                        minigunLoopSoundInstance.setParameterByName("on", 0);
                        minigunLoopSoundInstance.start();

                        shootingDecorationStuffOn = true;
                        */
                        if (IsThisMultiplayer.Instance.multiplayer)
                            pv.RPC("RPC_EnableBulletEffects", RpcTarget.All);
                        else
                            RPC_EnableBulletEffects();
                    }
                }
                else
                {
                    if (shootingDecorationStuffOn)
                    {
                        /*
                        barrelRotationSpeed = barrelRotationStartSpeed;
                        VFXBulletGo.SetActive(false);
                        muzzleFlash.SetActive(false);
                        minigunLoopSoundInstance.setParameterByName("on", 1);

                        shootingDecorationStuffOn = false;
                        */
                        if (IsThisMultiplayer.Instance.multiplayer)
                            pv.RPC("RPC_DisableBulletEffects", RpcTarget.All);
                        else
                            RPC_DisableBulletEffects();
                    }
                }
            }
            //---------------------------------------------------------------------------------------------------------------------
        }

        if (Input.GetButtonDown("RPGButton") && RPG && !RPGEquipped)
        {
            anim.SetBool("Gun Anim", true);
            RPGEquipped = true;
            fireCooldown = Time.time;
        }
        else if (Input.GetButtonDown("RPGButton") && RPG && RPGEquipped)
        {
            anim.SetBool("Gun Anim", false);
            RPGEquipped = false;
            RPGReadytoFire = false;
        }
    }

    [PunRPC]
    void RPC_EnableBulletEffects()
    {
        barrelRotationSpeed = barrelRotationMaxSpeed;
        VFXBulletGo.SetActive(true);
        muzzleFlash.SetActive(true);

        FMODUnity.RuntimeManager.AttachInstanceToGameObject(minigunLoopSoundInstance, transform, rb);
        minigunLoopSoundInstance.setParameterByName("on", 0);
        minigunLoopSoundInstance.start();

        shootingDecorationStuffOn = true;
    }

    [PunRPC]
    void RPC_DisableBulletEffects()
    {
        barrelRotationSpeed = barrelRotationStartSpeed;
        VFXBulletGo.SetActive(false);
        muzzleFlash.SetActive(false);
        minigunLoopSoundInstance.setParameterByName("on", 1);

        shootingDecorationStuffOn = false;
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

        stand.localRotation = Quaternion.Euler(0, xAngle, 0);
        gunBarrel.localRotation = Quaternion.Euler(yAngle, -90, 0);
    }

    #region UI

    void CrossHair()
    {
        if (!IsThisMultiplayer.Instance.multiplayer) { return; }
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
        GameObject grenade = Instantiate(rocket, cineCamera.transform.position, cineCamera.transform.rotation);
        Vector3 direction = Spread(0);
        grenade.GetComponent<Rigidbody>().AddForce(direction * 100, ForceMode.Impulse);
    }

    [PunRPC]
    void ShootRPG()
    {
        if (pv.IsMine)
        {
            FMODUnity.RuntimeManager.PlayOneShotAttached("event:/GunFX/RPG/RPGFire", gameObject);
            GameObject grenade = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Grenade"), cineCamera.transform.position, cineCamera.transform.rotation, 0);
            Vector3 direction = Spread(0);
            grenade.GetComponent<Rigidbody>().AddForce(direction * 100, ForceMode.Impulse);
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

        {
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
        }
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
                    //pv.RPC("RPC_DealDamage", RpcTarget.All, target, weaponDamage);

                    foreach(Player p in PhotonNetwork.PlayerList)
                    {
                        if(target.pv.Owner == p && target.gameObject != this.GetComponent<MoveTurretPosition>().car) // <---------- New car check
                        {
                            pv.RPC("RPC_DealDamage", RpcTarget.All, weaponDamage, target.gameObject.GetPhotonView().ViewID);
                        }
                    }
                }

                if(useImpactParticleEffect)
                {
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
                }



                //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// HIT MARKERS
                //only show a hitmarker if the car doesnt belong to you
                if (this.GetComponent<MoveTurretPosition>().car != null)
                {
                    if(hit.transform.gameObject != this.GetComponent<MoveTurretPosition>().car)
                    {
                        hitmarker.ChangeAlpha(1);
                        FMODUnity.RuntimeManager.PlayOneShot(hitmarkerSound);
                    }
                }
                else //if you are not connected to a car you can get hitmarkers on every car
                {
                    hitmarker.ChangeAlpha(1);
                    FMODUnity.RuntimeManager.PlayOneShot(hitmarkerSound);
                }

                /*
                float chance = Random.Range(0, 100);
                if (chance <= trailPercentage)
                {
                    
                    if (IsThisMultiplayer.Instance.multiplayer)
                    {
                        
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

                }*/
                
            }
        }
    }

    [PunRPC]
    void RPC_DealDamage(float amountOfDamage, int targetViewID)
    {
        PhotonView.Find(targetViewID).gameObject.GetComponent<Target>().TakeDamage(amountOfDamage);
        //TakeDamage(amountOfDamage);
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