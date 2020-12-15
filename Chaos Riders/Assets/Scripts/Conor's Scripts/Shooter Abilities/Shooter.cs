using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Cinemachine;

public class Shooter : MonoBehaviour
{
    //z rotation positive ++
    [SerializeField] private Transform barrelToRotate;
    private float barrelRotationSpeed;
    [SerializeField] private float barrelRotationStartSpeed = 100f, barrelRotationMaxSpeed = 800f;

    [SerializeField] private GameObject bulletSpawnPoint;
    public GameObject car;
    private GameObject carCollision;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float minigunDamage;
    [SerializeField] float playerNumber = 1;
    public bool connectCar = false;


    //shooting
    [SerializeField] private float damage = 10f;
    [SerializeField] private float range = 100f;

    [SerializeField] private KeyCode shootButton = KeyCode.Mouse0; 
    [SerializeField] private float amountOfAmmoForCooldownBar = 1000;
    private float startAmmo; //the amount of ammo for the cooldown bar at the start of the game
    private float ammoNormalized; //normalized the ammo value to be between 0 and 1 for the cooldown bar scale
    [SerializeField] private Transform coolDownBarUi; //ui bar that shows the cooldown of the minigun

    [SerializeField] private Transform gunBarrel; //barrel that is going to rotate to face the correct direction
    [SerializeField] private float horizontalRotationSpeed = 5f, verticalRotationSpeed = 3f; //rotation speeds for the gun
    private float xAngle, yAngle; //angle of rotation for the gun axis

    [SerializeField] private ParticleSystem muzzleFlash;
    public GameObject impactEffect;

    [SerializeField] private float minRotationHeight = -20f, maxRotationHeight = 20f;

    [SerializeField] private Transform impactEffectHolder;

    private PhotonView pv;

    private void Awake()
    {
        pv = GetComponent<PhotonView>();
        startAmmo = amountOfAmmoForCooldownBar;

        barrelRotationSpeed = barrelRotationStartSpeed;
    }

    void Start()
    {
        if (pv.IsMine && IsThisMultiplayer.Instance.multiplayer || !IsThisMultiplayer.Instance.multiplayer)
        {

        }
    }

    // Update is called once per frame
    void Update()
    {
        RotateGunBarrel();

        if(connectCar)
        {
            connectCar = false;
            car = GetComponentInParent<MoveTurretPosition>().car;
            carCollision = GetComponentInParent<MoveTurretPosition>().car.transform.GetChild(0).gameObject;
        }


        //online shooting
        if (pv.IsMine && IsThisMultiplayer.Instance.multiplayer)
        {
            FollowMouse();

            ammoNormalized = amountOfAmmoForCooldownBar / startAmmo; //normalized the ammo value to be between 0 and 1 for the cooldown bar scale
            CoolDownBar(ammoNormalized); //scale the size of the cooldown bar to match the ammo count

            //if you are shooting and have ammo
            if (Input.GetKey(shootButton) && amountOfAmmoForCooldownBar > 0)
            {
                amountOfAmmoForCooldownBar--;
                pv.RPC("Shoot", RpcTarget.All);
                barrelRotationSpeed = barrelRotationMaxSpeed;
            }
            else
                barrelRotationSpeed = barrelRotationStartSpeed;

            //if you are not shooting and the ammo isnt full
            if (amountOfAmmoForCooldownBar < startAmmo && !Input.GetKey(shootButton))
            {
                amountOfAmmoForCooldownBar++;
            }
        }

        //offline Shooting
        if(!IsThisMultiplayer.Instance.multiplayer)
        {
            FollowMouse();

            ammoNormalized = amountOfAmmoForCooldownBar / startAmmo; //normalized the ammo value to be between 0 and 1 for the cooldown bar scale
            CoolDownBar(ammoNormalized); //scale the size of the cooldown bar to match the ammo count

            //if you are shooting and have ammo
            if (Input.GetKey(shootButton) && amountOfAmmoForCooldownBar > 0)
            {
                amountOfAmmoForCooldownBar--;
                OfflineShoot();
                barrelRotationSpeed = barrelRotationMaxSpeed;
            }
            else
                barrelRotationSpeed = barrelRotationStartSpeed;

            //if you are not shooting and the ammo isnt full
            if (amountOfAmmoForCooldownBar < startAmmo && !Input.GetKey(shootButton))
            {
                amountOfAmmoForCooldownBar++;
            }
        }
    }

    private void RotateGunBarrel()
    {
        barrelToRotate.Rotate(0, 0, barrelRotationSpeed * Time.deltaTime);
    }

    private void FollowMouse()
    {
        xAngle += Input.GetAxis("Mouse X") * horizontalRotationSpeed * Time.deltaTime;
        //xAngle = Mathf.Clamp(xAngle, 0, 180); //use this if you want to clamp the rotation. second var = min angle, third var = max angle

        yAngle += Input.GetAxis("Mouse Y") * verticalRotationSpeed * -Time.deltaTime;
        yAngle = Mathf.Clamp(yAngle, minRotationHeight, maxRotationHeight); //use this if you want to clamp the rotation. second var = min angle, third var = max angle

        gunBarrel.localRotation = Quaternion.Euler(yAngle, xAngle, 0);
    }

    private void CoolDownBar(float sizeNormalized)
    {
       coolDownBarUi.localScale = new Vector3(sizeNormalized, 1f); //scale the ui cooldown bar to match the ammo count
    }

    [PunRPC]
    void Shoot()
    {
        muzzleFlash.Play();

        RaycastHit hit; //gets the information on whats hit
        if (Physics.Raycast(bulletSpawnPoint.transform.position, bulletSpawnPoint.transform.forward, out hit, range))
        {
            //Debug.Log("You Hit The: " + hit.transform.name);

            Target target = hit.transform.GetComponent<Target>();
            if(target != null && target.gameObject != car)
            {
                target.TakeDamage(damage);
            }

            GameObject impactGo = PhotonNetwork.Instantiate("Impact Particle Effect", hit.point, Quaternion.LookRotation(hit.normal), 0);
            //impactGo.transform.parent = impactEffectHolder;
            //PhotonNetwork.Destroy(impactGo);
        }
    }

    void OfflineShoot()
    {
        muzzleFlash.Play();

        RaycastHit hit; //gets the information on whats hit
        if (Physics.Raycast(bulletSpawnPoint.transform.position, bulletSpawnPoint.transform.forward, out hit, range))
        {
            //Debug.Log("You Hit The: " + hit.transform.name);

            Target target = hit.transform.GetComponent<Target>();
            if (target != null && target.gameObject != car)
            {
                target.TakeDamage(damage);
            }

            GameObject impactGo = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            impactGo.transform.parent = impactEffectHolder;
            //Destroy(impactGo, 1);
        }
    }

    private IEnumerator DeleteImpactEffect(float time)
    {
        yield return new WaitForSeconds(time);

    }
}
