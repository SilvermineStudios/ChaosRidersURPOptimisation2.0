using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIShooter : MonoBehaviour
{
    #region Variables
    [Header("General GameObjects")]
    public GameObject car;
    [SerializeField] private Rigidbody carRigidBody;
    public bool noCarNeeded = false;
    [SerializeField] private bool useTestingCamera = false;
    [SerializeField] private GameObject Camera;
    [SerializeField] private float weaponDamage = 3f;
    [SerializeField] private Transform minigunBarrel;
    [SerializeField] private Transform rifleBarrel;
    [SerializeField] GameObject rpgGo;
    [SerializeField] GameObject rocketspawn;
    [SerializeField] GameObject rocket;
    [SerializeField] private Transform gunBarrel; //barrel that is going to rotate to face the correct direction
    [SerializeField] private GameObject bulletSpawnPoint;
    [SerializeField] GameObject MinigunHolder;
    [SerializeField] GameObject RifleHolder;
    [SerializeField] public GameObject muzzleFlash;
    public bool removedMyCarFromTargets = false;
    [SerializeField] LayerMask everythingButIgnoreBullets;
    private MoveTurretPosition mtp;
    private bool readyToDestroy = false;
    private bool shooting = false;



    [Header("Decoration")]
    public GameObject impactEffect;
    [SerializeField] private Transform impactEffectHolder;
    [SerializeField] GameObject VFXBulletGo;
    private float barrelRotationSpeed;
    private float barrelRotationStartSpeed = 100f;
    private float barrelRotationMaxSpeed = 800f;

    //FMOD
    [SerializeField] private GameObject soundSourceLocation;
    [SerializeField] private float gunSoundCoolDownTime = 2f;
    private float startGunSoundCoolDownTime;
    
    


    // https://www.youtube.com/watch?v=QKhn2kl9_8I
    [Header("Brackeys Stuff")]
    public Transform partToRotate;
    public string aiCarTag = "Player";
    public string carTag = "car";
    public float range = 30f;
    [SerializeField] private float timeBeforeLookingForANewTarget = 2f;
    [SerializeField] private float delayBeforeStartWorking = 3f;
    public List<GameObject> targets = new List<GameObject>();
    public float turnSpeed = 5f;
    [SerializeField] private float xOffest = 0.2f;
    private Transform target;
    #endregion

    private void Awake()
    {
        startGunSoundCoolDownTime = gunSoundCoolDownTime;

        if(mtp == null)
            mtp = this.gameObject.GetComponent<MoveTurretPosition>();

        //decoration
        VFXBulletGo.SetActive(false);
        muzzleFlash.SetActive(false);
        barrelRotationSpeed = barrelRotationStartSpeed;

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
    
        if (mtp.car != null && car == null)
            car = mtp.car;

        if (car != null && carRigidBody == null)
            carRigidBody = car.GetComponent<Rigidbody>();


        if (!MasterClientRaceStart.Instance.weaponsFree) { return; }

        //dont do anything if there is no target
        if (target == null)
            return;
        else
            TargetLockOn();

        if (shooting)
        {
            gunSoundCoolDownTime -= 1 * Time.deltaTime;

            if(gunSoundCoolDownTime <= 0)
            {
                InvokeRepeating("ShootSound", 0, 100);
                gunSoundCoolDownTime = startGunSoundCoolDownTime;
            }
            
        }
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
            CancelInvoke("ShootSound");
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
            target = nearestEnemy.transform;
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
        Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        partToRotate.rotation = Quaternion.Euler(rotation.x - xOffest, rotation.y, 0f);
    }

    //draws a sphere to show the range of the gun
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
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

        foreach(GameObject go in targets)
        {
            if(car != null && go.gameObject == car.gameObject)
            {
                //targets.Remove(go);
                //removedMyCarFromTargets = true;
            }
        }

        //Starts lookinf for a target and then repeats it over and over again
        InvokeRepeating("UpdateTarget", 0f, timeBeforeLookingForANewTarget); //MOVE TO WHEN WEAPONS ARE ENABLED
    }
    #endregion





    #region Shooting

    void ShootSound()
    {
        FMODUnity.RuntimeManager.PlayOneShotAttached("event:/GunFX/Minigun/MinigunShot 2", soundSourceLocation);
    }

    void Shoot()
    {
        //Debug.Log("Shooting");
        

        muzzleFlash.SetActive(true);
        //muzzleFlash.GetComponent<ParticleSystem>().Play();
        
        VFXBulletGo.SetActive(true); //have bullet casings flying out
        barrelRotationSpeed = barrelRotationMaxSpeed;

        //FMODUnity.RuntimeManager.PlayOneShotAttached(sound, gameObject);

        /*
        Vector3 direction = Vector3.zero;
        RaycastHit hit;
        if (Physics.Raycast(bulletSpawnPoint.transform.position, direction, out hit, range))
        {
            Target target = hit.transform.GetComponent<Target>();

            if (target != null && target.gameObject != car)
            {
                //Debug.Log("Hit: " + target);
                target.TakeDamage(WeaponDamage);
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
        }
        */

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

                //GameObject impactGo;
                if (IsThisMultiplayer.Instance.multiplayer)
                {
                    //impactGo = PhotonNetwork.Instantiate("Impact Particle Effect", hit.point, Quaternion.LookRotation(hit.normal), 0);
                }
                else
                {
                    //impactGo = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
                }
                //impactGo.transform.parent = impactEffectHolder;
            }
        }
    }
    #endregion
}
