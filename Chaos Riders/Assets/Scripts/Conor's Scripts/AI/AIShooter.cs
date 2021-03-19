using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIShooter : MonoBehaviour
{
    #region Variables
    [Header("General GameObjects")]
    public GameObject car;
    [SerializeField] private Transform minigunBarrel;
    [SerializeField] private Transform rifleBarrel;
    [SerializeField] GameObject rpgGo;
    [SerializeField] GameObject rocketspawn;
    [SerializeField] GameObject rocket;
    [SerializeField] private Transform gunBarrel; //barrel that is going to rotate to face the correct direction
    [SerializeField] private GameObject bulletSpawnPoint;
    [SerializeField] GameObject MinigunHolder;
    [SerializeField] GameObject RifleHolder;
    LayerMask everythingButIgnoreBullets;
    [SerializeField] public ParticleSystem muzzleFlash;

    [Header("Bullet Decoration")]
    public GameObject impactEffect;
    [SerializeField] private Transform impactEffectHolder;
    [SerializeField] GameObject CasingSpawn;
    [SerializeField] GameObject Casing;

    /// <summary>
    /// /////////////////////////////////////////////////////////////////MAKE SURE TO NOT HAVE YOUR OWN CAR BE IN THE TARGETS ARRAY///////////////////////////////////////////////////////
    ///                                                                      foreach(Gameobkect go in targets)
    ///                                                                      {
    ///                                                                         if(go.isMyCar)
    ///                                                                         {
    ///                                                                             targets.Remove(go);
    ///                                                                         }
    ///                                                                      }
    /// </summary>
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
    private Transform target;
    #endregion

    void Start()
    {
        StartCoroutine(GetTargets(delayBeforeStartWorking));
    }

    void Update()
    {
        //dont do anything if there is no target
        if (target == null)
            return;

        TargetLockOn();
    }

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
        partToRotate.rotation = Quaternion.Euler(rotation.x, rotation.y, 0f);
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
            targets.Add(go);
        }

        //add all of the player cars to the targets array
        foreach(GameObject go in cars)
        {
            targets.Add(go);
        }

        //Starts lookinf for a target and then repeats it over and over again
        InvokeRepeating("UpdateTarget", 0f, timeBeforeLookingForANewTarget); //MOVE TO WHEN WEAPONS ARE ENABLED
    }
}
