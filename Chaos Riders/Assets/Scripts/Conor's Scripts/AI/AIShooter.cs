using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIShooter : MonoBehaviour
{
    // https://www.youtube.com/watch?v=QKhn2kl9_8I // usefull youtube video 

    #region Variables
    [Header("General GameObjects")]
    public GameObject car;
    public float carHealth;
    public bool dead = false;
    private Rigidbody carRigidBody;
    private GameObject Camera;
    [SerializeField] private bool shooting = false;
    [SerializeField] private bool gunShooting = false;
    [SerializeField] private Transform gunBarrel; //barrel that is going to rotate to face the correct direction
    [SerializeField] private Transform minigunBarrel;
    [SerializeField] private Transform rifleBarrel;
    [SerializeField] GameObject rpgGo;
    [SerializeField] GameObject rocketspawn;
    [SerializeField] GameObject rocket;
    [SerializeField] private GameObject bulletSpawnPoint;
    [SerializeField] GameObject MinigunHolder;
    [SerializeField] GameObject RifleHolder;
    private MoveTurretPosition mtp;
    private bool readyToDestroy = false;
    private Rigidbody rb;

    [Header("Gun Variables")]
    [SerializeField] private float shotsPerSecond = 10f; //the amount of bullets the ai gun can shoot per second
    private float shootingRepeatSpeed;
    [SerializeField] LayerMask everythingButIgnoreBullets;
    [SerializeField] private float weaponDamage = 0.2f;
    public float range = 30f;
    public float turnSpeed = 5f;
    [SerializeField] private float barrelRotationStartSpeed = 100f;
    [SerializeField] private float barrelRotationMaxSpeed = 800f;
    private float timeBeforeLookingForANewTarget = 1f;
    private float delayBeforeStartWorking = 2f;


    [Header("Decoration")]
    [SerializeField] private GameObject muzzleFlash;
    public GameObject impactEffect;
    [SerializeField] private Transform impactEffectHolder;
    [SerializeField] GameObject VFXBulletGo;
    private float barrelRotationSpeed;
    

    [Header("FMOD")]
    FMOD.Studio.EventInstance minigunLoopSoundInstance;
    private bool shootingSoundOn = false;

    
    [Header("Targeting")]
    public string aiCarTag = "Player";
    public string carTag = "car";
    public List<GameObject> targets = new List<GameObject>();
    private float xOffest = 0.2f;
    private Transform target;


    [Header("Toggles")]
    [SerializeField] private bool useTestingCamera = false;
    public bool noCarNeeded = false;
    [SerializeField] private bool UseShootingSound = true;
    [SerializeField] private bool UseImpactEffect = true;
    [SerializeField] private bool UseMuzzleFlash = true;
    [SerializeField] private bool UseBulletCasings = true;
    #endregion


    //draws a sphere to show the range of the gun
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }

    private void Awake()
    {
        shootingRepeatSpeed = (60 / shotsPerSecond) / 60;
        //Debug.Log("Shooting speed = " + shootingRepeatSpeed);

        rb = GetComponent<Rigidbody>();
        Camera = this.gameObject.GetComponentInChildren<AudioListener>().gameObject;

        if(mtp == null)
            mtp = this.gameObject.GetComponent<MoveTurretPosition>();

        //decoration
        VFXBulletGo.SetActive(false);
        muzzleFlash.SetActive(false);
        barrelRotationSpeed = barrelRotationStartSpeed;
        minigunLoopSoundInstance = FMODUnity.RuntimeManager.CreateInstance("event:/GunFX/Minigun/MinigunLoop");

        if (!useTestingCamera)
            Camera.SetActive(false);
        else
            Camera.SetActive(true);
    }

    void Start()
    {
        StartCoroutine(GetTargets(delayBeforeStartWorking));
    }

    void Update()
    {
        DestroyMeIfDriverDisconnects();

        targets.RemoveAll(x => x == null); //remove any targets that are null (Disconnected drivers)

        //Getting car info
        if (mtp.car != null && car == null)
        {
            car = mtp.car;
        }

        if (car != null && carRigidBody == null)
            carRigidBody = car.GetComponent<Rigidbody>();

        if(car != null)
            carHealth = car.GetComponent<Target>().health;

        if (carHealth <= 0 && !dead)
            dead = true;
        else
            dead = false;

        //////////////////////////////////////////////////////////// Put all shooting stuff below here
        if (!MasterClientRaceStart.Instance.weaponsFree) { return; }

        //dont do anything if there is no target
        if (target == null)
            return;
        else
            TargetLockOn();
    }


    private void FixedUpdate()
    {
        RotateGunBarrel();

        if (!MasterClientRaceStart.Instance.weaponsFree) { return; }

        //Check If you are shooting or not
        if (target != null)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, target.transform.position); //check the distance of the target from the player
            if (distanceToEnemy <= range)
                shooting = true;
            else
                shooting = false;
        }
        else
            shooting = false;


        //Shooting
        if (shooting)
        {
            ShootingEffectsOn();

            if(!gunShooting)
            {
                //InvokeRepeating("Shoot", 0, 1);
                InvokeRepeating("ShootBullets", 0, shootingRepeatSpeed);
                gunShooting = true;
            }
        }
        else
        {
            ShootingEffectsOff();

            if(gunShooting)
            {
                //CancelInvoke("Shoot");
                CancelInvoke("ShootBullets");
                gunShooting = false;
            }
        }
    }

    private void RotateGunBarrel()
    {
        minigunBarrel.Rotate(0, 0, barrelRotationSpeed * Time.deltaTime);
    }

    private void DestroyMeIfDriverDisconnects()
    {
        if (!readyToDestroy && car != null)
            readyToDestroy = true;

        if (readyToDestroy && car == null)
            Destroy(this.gameObject);
    }


    #region Targeting

    //gets turned on in the GetTargets Courotine and repeats by what the "TimeBeforeLookingForANewTarget" is set too
    void UpdateTarget()
    {
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        //cycle through all of the targets in the target array list to find the nearest one
        foreach (GameObject enemy in targets)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);

            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        //if a nearest enemy is found and within range then it becomes the target
        if (nearestEnemy != null && shortestDistance <= range)
        {
            if (nearestEnemy.transform.root.gameObject.GetComponent<Target>().health > 0)
                target = nearestEnemy.transform;
            else
                target = null;
        }
        else
            target = null; //if the enemy goes out of range remove them from being the target
    }

    void TargetLockOn()
    {
        Vector3 dir = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(gunBarrel.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        gunBarrel.rotation = Quaternion.Euler(rotation.x - xOffest, rotation.y, 0f);
    }

    private IEnumerator GetTargets(float time)
    {
        yield return new WaitForSeconds(time);

        foreach (GameObject go in GameObject.FindGameObjectsWithTag(aiCarTag))
        {
            if (go.transform.root.gameObject != car.gameObject)
                targets.Add(go);
        }
        foreach (GameObject go in GameObject.FindGameObjectsWithTag(carTag))
        {
            if (go.transform.root.gameObject != car.gameObject)
                targets.Add(go);
        }

        InvokeRepeating("UpdateTarget", 0f, timeBeforeLookingForANewTarget); //MOVE TO WHEN WEAPONS ARE ENABLED
    }
    #endregion


    #region Shooting
    void ShootingEffectsOn()
    {
        //turn on muzzleflash
        if (UseMuzzleFlash)
            muzzleFlash.SetActive(true);

        //turn on bullet casings
        if (UseBulletCasings)
            VFXBulletGo.SetActive(true); //have bullet casings flying out

        //turn on shooting sound
        if(UseShootingSound && !shootingSoundOn)
        {
            FMODUnity.RuntimeManager.AttachInstanceToGameObject(minigunLoopSoundInstance, this.transform, rb);
            minigunLoopSoundInstance.setParameterByName("on", 0);
            minigunLoopSoundInstance.start();
            shootingSoundOn = true;
        }

        //make barrel move faster
        barrelRotationSpeed = barrelRotationMaxSpeed;
    }

    void ShootingEffectsOff()
    {
        //turn off bullet casings
        if (UseBulletCasings)
            VFXBulletGo.SetActive(false);

        //turn off muzzleflash
        if (UseMuzzleFlash)
            muzzleFlash.SetActive(false);

        //turn off shooting sound
        if(UseShootingSound && shootingSoundOn)
        {
            minigunLoopSoundInstance.setParameterByName("on", 1);
            shootingSoundOn = false;
        }
            
        //make gun barrel rotation slow again
        barrelRotationSpeed = barrelRotationStartSpeed;
    }

    void ShootBullets()
    {
        RaycastHit[] hits;
        Vector3 direction = bulletSpawnPoint.transform.forward;

        hits = Physics.RaycastAll(bulletSpawnPoint.transform.position, direction, range, everythingButIgnoreBullets);
        Debug.DrawRay(bulletSpawnPoint.transform.position, direction * range, Color.red);

        foreach (RaycastHit hit in hits)
        {
            /*
            if(hit.transform.gameObject.GetComponent<Target>())
                Debug.Log("The AI Gun Hit: " + hit.transform.root.name);
            else
                Debug.Log("The AI Gun Missed");
            */

            //dont so anything if what you hit doesnt have a target script on it or if it is your own car
            if (!hit.transform.gameObject.GetComponent<Target>())
                return;

            Target target = hit.transform.GetComponent<Target>();

            if (target.gameObject != car)
                target.TakeDamage(weaponDamage);


            if (UseImpactEffect)
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
                impactGo.transform.parent = impactEffectHolder;
            }
        }
    }
    #endregion
}
