using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Cinemachine;
using TMPro;
using System.IO;
using UnityEngine.UI;

public class Shooter : MonoBehaviour
{

    //Variables
    #region General GameObjects
    public GameObject car;
    private GameObject carCollision;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] GameObject rpgGo;
    [SerializeField] GameObject rocketspawn;
    [SerializeField] GameObject rocket;
    [SerializeField] TMP_Text rpgcount;
    Rigidbody rb;
    [SerializeField] private Transform gunBarrel; //barrel that is going to rotate to face the correct direction
    [SerializeField] private GameObject bulletSpawnPoint;
    ParticleSystem muzzleFlash;
    [SerializeField] GameObject MinigunHolder;
    [SerializeField] GameObject RifleHolder;
    [SerializeField] GameObject MinigunIcon, RifleIcon;

    #endregion

    #region Camera
    [SerializeField] private CinemachineVirtualCamera cineCamera;
    #endregion

    #region Bools
    public bool connectCar = false;
    public bool RPG;
    [SerializeField] private bool pickedUpRPG = false;
    private bool currentlyShooting;
    private bool shootButtonHeld;
    public bool isShooting { get { return currentlyShooting; } private set { isShooting = currentlyShooting; } }
    public bool isPressingShootbutton { get { return shootButtonHeld; } private set { isPressingShootbutton = shootButtonHeld; } }
    [SerializeField] bool noCarNeeded;
    bool usingAmmo;
    #endregion

    #region Floats
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
    private float maxBulletDeviation;
    private float maxCrosshairDeviation;
    private float bulletDeviationIncrease;
    private float crosshairDeviationIncrease;
    [SerializeField] float currentBulletSpread = 0;
    [SerializeField] float currentCrosshairSpread = 0;

    #endregion

    #region Input
    [SerializeField] private KeyCode shootButton = KeyCode.Mouse0;
    [SerializeField] private KeyCode RPGButton = KeyCode.Tab;
    [SerializeField] private KeyCode changeWeapon = KeyCode.Mouse1;
    #endregion

    #region RPG
    [SerializeField] private float amountOfAmmoForRPG = 10;
    private float startAmountOfAmmoForRPG;
    #endregion

    #region BulletTrails
    [SerializeField] GameObject trail;
    [SerializeField] float trailPercentage;
    #endregion

    #region Photon
    private PhotonView pv;
    #endregion

    #region Scripts
    Pause pauseMenu;
    Controller carController;
    AICarController aiCarController;
    Minigun minigunScript;
    Rifle rifleScript;
    #endregion

    #region Bullet Decoration
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
    [SerializeField] RectTransform reticle; // The RecTransform of reticle UI element.
    [SerializeField] GameObject CrossHairGameobject;
    public float restingSize;
    public float maxSize;
    public float speed;
    public float currentSize;
    private float spreadSize;
    #endregion

    void Start()
    {
        hitmarker.ChangeAlpha(0);
        pauseMenu = GetComponent<Pause>();
        pv = GetComponent<PhotonView>();
        startAmountOfAmmoForRPG = amountOfAmmoForRPG;
        rb = GetComponent<Rigidbody>();
        previousShooterClass = shooterClass;
        if(shooterClass == ShooterClass.minigun)
        {
            SetupMinigun();
        }
        else if (shooterClass == ShooterClass.rifle)
        {
            SetupRifle();
        }
        fireCooldown = fireCooldown - fireRate;
    }


    void SetupMinigun()
    {
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
    }

    void Update()
    {
        if (pauseMenu.paused) { return; }
        if (pv.IsMine && IsThisMultiplayer.Instance.multiplayer || !IsThisMultiplayer.Instance.multiplayer)
        {
            FollowMouse();
            if (connectCar)
            {
                connectCar = false;
                car = GetComponentInParent<MoveTurretPosition>().car;
                carCollision = GetComponentInParent<MoveTurretPosition>().car.transform.GetChild(0).gameObject;
            }

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
                            Debug.Log("Put AI car RPG STUFF HERE");///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


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

        //online shooting
        if (pv.IsMine && IsThisMultiplayer.Instance.multiplayer)
        {
            if (Input.GetKeyDown(changeWeapon) && pv.IsMine)
            {
                
                if (shooterClass == ShooterClass.minigun)
                {
                    SetupRifle();
                    shooterClass = ShooterClass.rifle;
                }
                else if (shooterClass == ShooterClass.rifle)
                {
                    SetupMinigun();
                    shooterClass = ShooterClass.minigun;
                }
                previousShooterClass = shooterClass;
                currentCrosshairSpread = 0;
            }
            if (Input.GetKey(shootButton) && pv.IsMine && usingAmmo)
            {
                shootButtonHeld = true;
            }
            else
            {
                shootButtonHeld = false;
            }

            if (!RPG)
            {
                pv.RPC("HideRPG", RpcTarget.All);
            }

            //Wait for weapons Free
            if (!MasterClientRaceStart.Instance.weaponsFree) { return; }
            if (shooterClass == ShooterClass.minigun)
            {
                //if you are shooting and have ammo (MINIGUN)
                if (Input.GetKey(shootButton) && minigunScript.amountOfAmmoForCooldownBar > 0 && !RPG)
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
                pv.RPC("ShowRPG", RpcTarget.All);
                if (Input.GetKeyDown(shootButton) && amountOfAmmoForRPG > 0)
                {
                    amountOfAmmoForRPG--;
                    pv.RPC("ShootRPG", RpcTarget.All);
                }
                if (amountOfAmmoForRPG <= 0)
                {
                    RPG = false;
                    this.GetComponent<ShooterPickup>().hasRPG = false;
                    amountOfAmmoForRPG = startAmountOfAmmoForRPG;
                }
            }
            
        }
        
        
        //offline Shooting
        if (!IsThisMultiplayer.Instance.multiplayer)
        {
            if (Input.GetKeyDown(changeWeapon))
            {
                if (shooterClass == ShooterClass.minigun)
                {
                    SetupRifle();
                    shooterClass = ShooterClass.rifle;
                }
                else if (shooterClass == ShooterClass.rifle)
                {
                    SetupMinigun();
                    shooterClass = ShooterClass.minigun;
                }
                previousShooterClass = shooterClass;
                currentCrosshairSpread = 0;
            }

            if (Input.GetKey(shootButton) && usingAmmo)
            {
                shootButtonHeld = true;
            }
            else
            {
                shootButtonHeld = false;
            }

            if (shooterClass == ShooterClass.minigun)
            {
                //if you are shooting and have ammo
                if (Input.GetKey(shootButton) && minigunScript.amountOfAmmoForCooldownBar > 0 && !RPG)
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
                        OfflineShoot();
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
                //if you are shooting
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
                        OfflineShoot();
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

            if (!RPG)
            {
                rpgGo.SetActive(false);
                
                if (Input.GetKeyDown(RPGButton))
                {
                    //RPG = true;
                }
            }
            else
            {
                rpgGo.SetActive(true);
                if (Input.GetKeyDown(shootButton) && amountOfAmmoForRPG > 0)
                {
                    amountOfAmmoForRPG--;
                    OfflineShootRPG();
                }
                if (Input.GetKeyDown(RPGButton))
                {
                    //RPG = false;
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
        if (pauseMenu.paused)
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

        GameObject a = PhotonNetwork.Instantiate("BulletCasing", CasingSpawn.transform.position, CasingSpawn.transform.rotation);

        if (!noCarNeeded)
        {
            if (car.GetComponent<CarController>())
            {
                a.GetComponent<Rigidbody>().velocity = carController.rb.velocity;
            }
            else
            {
                a.GetComponent<Rigidbody>().velocity = aiCarController.rb.velocity;
            }

            a.GetComponent<Rigidbody>().AddForce((a.transform.right + (a.transform.up * 2)) * 0.3f, ForceMode.Impulse);
        }
        RaycastHit hit; 
        if (Physics.Raycast(cineCamera.transform.position, direction, out hit, weaponRange))
        {
            Target target = hit.transform.GetComponent<Target>();
            if (target != null && target.gameObject != car)
            {
                target.TakeDamage(weaponDamage);
            }

            GameObject impactGo = PhotonNetwork.Instantiate("Impact Particle Effect", hit.point, Quaternion.LookRotation(hit.normal), 0);
        }

        float chance = Random.Range(0, 100);
        if (chance <= trailPercentage)
        {
            GameObject b = PhotonNetwork.Instantiate("Trail", bulletSpawnPoint.transform.position, bulletSpawnPoint.transform.rotation);
            FMODUnity.RuntimeManager.PlayOneShotAttached(bulletWhistle, b);
            if (!noCarNeeded)
            {
                if (car.GetComponent<CarController>() && pv.IsMine)
                    b.GetComponent<Rigidbody>().velocity = carController.rb.velocity;
                else
                    b.GetComponent<Rigidbody>().velocity = aiCarController.rb.velocity;
            }

            b.GetComponent<Rigidbody>().AddForce(b.transform.forward * 100, ForceMode.Impulse);
            b.GetComponent<DeleteMe>().enabled = true;
        }

    }

    void OfflineShoot()
    {
        muzzleFlash.Play();
        FMODUnity.RuntimeManager.PlayOneShotAttached( sound, gameObject);
        Vector3 direction = Spread(currentBulletSpread);

        GameObject a = Instantiate(Casing, CasingSpawn.transform.position, CasingSpawn.transform.rotation);

        a.GetComponent<Rigidbody>().AddForce((a.transform.right + (a.transform.up * 2)) * 0.3f, ForceMode.Impulse);


        if (!noCarNeeded)
        {
            if (car.transform.tag == "car")
                a.GetComponent<Rigidbody>().velocity = carController.rb.velocity;
            else
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
            }

            GameObject impactGo = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            impactGo.transform.parent = impactEffectHolder;
            //Destroy(impactGo, 1);
        }

        float chance = Random.Range(0, 100);
        if (chance <= trailPercentage)
        {
            GameObject b = Instantiate(trail, bulletSpawnPoint.transform.position, bulletSpawnPoint.transform.rotation);
            FMODUnity.RuntimeManager.PlayOneShotAttached(bulletWhistle, b);
            if (!noCarNeeded)
            {
                if (car.transform.tag == "car")
                    b.GetComponent<Rigidbody>().velocity = carController.rb.velocity;
                else
                    b.GetComponent<Rigidbody>().velocity = aiCarController.rb.velocity;
            }

            b.GetComponent<Rigidbody>().AddForce(b.transform.forward * 300, ForceMode.Impulse);
            b.GetComponent<DeleteMe>().enabled = true;
        }

    }
    #endregion


    #region BulletImpacts
    private IEnumerator DeleteImpactEffect(float time)
    {
        yield return new WaitForSeconds(time);

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
