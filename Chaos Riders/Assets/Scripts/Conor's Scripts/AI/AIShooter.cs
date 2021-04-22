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
    private bool shooting = false;
    private Rigidbody rb;

    [Header("Gun Variables")]
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

        //Getting car info
        if (mtp.smoothCar != null && car == null)
        {
            car = mtp.smoothCar;
        }

        if (car != null && carRigidBody == null)
            carRigidBody = car.GetComponent<Rigidbody>();

        if(car != null)
        {
            carHealth = car.GetComponent<Target>().health;
        }

        if (carHealth <= 0 && !dead)
        {
            dead = true;
        }
        else
            dead = false;

        //////////////////////////////////////////////////////////// Put all shooting stuff below here
        if (!MasterClientRaceStart.Instance.weaponsFree) { return; }

        //dont do anything if there is no target
        if (target == null)
            return;
        else
            TargetLockOn();

        ShootSound();
    }


    private void FixedUpdate()
    {
        RotateGunBarrel();

        if (!MasterClientRaceStart.Instance.weaponsFree) { return; }

        if (target != null && !shooting)
        {
            InvokeRepeating("Shoot", 0, 1);
            shooting = true;
        }
        else
        {
            CancelInvoke("Shoot");
            //CancelInvoke("ShootSound");
            shooting = false;
        }

        

        if(target == null)
        {
            VFXBulletGo.SetActive(false);
            muzzleFlash.SetActive(false);
            barrelRotationSpeed = barrelRotationStartSpeed;
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
        foreach(GameObject enemy in targets)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);

            if(distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        //if a nearest enemy is found and within range then it becomes the target
        if(nearestEnemy != null && shortestDistance <= range)
        {
            if(nearestEnemy.transform.root.gameObject.GetComponent<Target>().health > 0)
                target = nearestEnemy.transform;
            else
                target = null;
        }
        else
        {
            target = null; //if the enemy goes out of range remove them from being the target
        }
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

        GameObject[] aicars = GameObject.FindGameObjectsWithTag(aiCarTag); //get an array of all of the ai cars 
        GameObject[] cars = GameObject.FindGameObjectsWithTag(carTag); //get an array of all of the player cars
        
        //add all of the ai cars to the targets array
        foreach(GameObject go in aicars)
        {
            if(go.gameObject != car.gameObject)
                targets.Add(go);
        }

        //add all of the player cars to the targets array
        foreach(GameObject go in cars)
        {
            if (go.transform.root.gameObject != car.gameObject)
                targets.Add(go);
        }

        //Starts lookinf for a target and then repeats it over and over again
        InvokeRepeating("UpdateTarget", 0f, timeBeforeLookingForANewTarget); //MOVE TO WHEN WEAPONS ARE ENABLED
    }
    #endregion


    #region Shooting

    void ShootSound()
    {
        if (!UseShootingSound)
            return;

        //if there is a target
        if (target != null)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, target.transform.position); //check the distance of the target from the player

            //shooting
            if (distanceToEnemy <= range) //if the target is within shooting range
            {
                //play the FMOD shooting sound
                if (!shootingSoundOn)
                {
                    FMODUnity.RuntimeManager.AttachInstanceToGameObject(minigunLoopSoundInstance, this.transform, rb);
                    minigunLoopSoundInstance.setParameterByName("on", 0);
                    minigunLoopSoundInstance.start();

                    shootingSoundOn = true;
                }
            }
            else //not shooting
            {
                //turn off the FMOD shooting sound
                if (shootingSoundOn)
                {
                    minigunLoopSoundInstance.setParameterByName("on", 1);

                    shootingSoundOn = false;
                }

            }
        }
    }

    void Shoot()
    {
        //Debug.Log("Shooting");

        //decoration
        if (UseMuzzleFlash)
            muzzleFlash.SetActive(true);
        if (UseBulletCasings)
            VFXBulletGo.SetActive(true); //have bullet casings flying out
        barrelRotationSpeed = barrelRotationMaxSpeed;


        RaycastHit[] hits;
        Vector3 direction = bulletSpawnPoint.transform.forward;
        //Vector3 direction = Vector3.zero;

        hits = Physics.RaycastAll(bulletSpawnPoint.transform.position, direction, range, everythingButIgnoreBullets);
        Debug.DrawRay(bulletSpawnPoint.transform.position, direction * range, Color.red);

        foreach (RaycastHit hit in hits)
        {
            //Debug.Log("You Hit: " + hit.transform.name);

            if (hit.transform.gameObject != car && (hit.transform.gameObject.tag == "Player" || hit.transform.gameObject.tag == "car"))
            {
                Target target = hit.transform.GetComponent<Target>();
                if (target != null && target.gameObject != car)
                {
                    target.TakeDamage(weaponDamage);
                }

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
    }
        
    #endregion
}
