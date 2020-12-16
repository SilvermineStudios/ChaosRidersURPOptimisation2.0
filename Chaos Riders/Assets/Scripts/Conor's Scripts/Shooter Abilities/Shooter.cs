using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Cinemachine;
using TMPro;
using System.IO;

public class Shooter : MonoBehaviour
{
    //z rotation positive ++
    [SerializeField] private Transform barrelToRotate;
    private float barrelRotationSpeed;
    [SerializeField] private float barrelRotationStartSpeed = 100f, barrelRotationMaxSpeed = 800f;
    [SerializeField] private float maxDeviation;

    [SerializeField] private GameObject bulletSpawnPoint;
    public GameObject car;
    private GameObject carCollision;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float minigunDamage;
    [SerializeField] float playerNumber = 1;
    public bool connectCar = false;
    [SerializeField] GameObject rpgGo;
    [SerializeField] GameObject rocketspawn;
    [SerializeField] GameObject rocket;
    [SerializeField] TMP_Text rpgcount;
    public bool RPG;


    //shooting
    [SerializeField] private float damage = 10f;
    [SerializeField] private float range = 100f;

    [SerializeField] private KeyCode shootButton = KeyCode.Mouse0;
    [SerializeField] private KeyCode RPGButton = KeyCode.Tab;
    [SerializeField] private float amountOfAmmoForCooldownBar = 1000;
    [SerializeField] private float amountOfAmmoForRPG = 10;
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
        
        rpgcount.text = amountOfAmmoForRPG + " / 10";
        if (connectCar)
        {
            connectCar = false;
            car = GetComponentInParent<MoveTurretPosition>().car;
            carCollision = GetComponentInParent<MoveTurretPosition>().car.transform.GetChild(0).gameObject;
        }

        if (pv.IsMine && IsThisMultiplayer.Instance.multiplayer || !IsThisMultiplayer.Instance.multiplayer)
        {
            FollowMouse();
            
            if(car.GetComponent<CarPickup>().hasRPG)
                RPG = true;

            if(amountOfAmmoForRPG <= 0)
            {
                RPG = false;
                car.GetComponent<CarPickup>().hasRPG = false;
            }
            
        }

        //online shooting
        if (pv.IsMine && IsThisMultiplayer.Instance.multiplayer)
        {

            if (!RPG)
            {
                pv.RPC("HideRPG", RpcTarget.All);
                

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
                if (Input.GetKeyDown(RPGButton))
                {
                    RPG = true;
                }

            }
            else
            {
                pv.RPC("ShowRPG", RpcTarget.All);
                if (Input.GetKeyDown(shootButton) && amountOfAmmoForRPG > 0)
                {
                    amountOfAmmoForRPG--;
                    ShootRPG();
                }
                if (Input.GetKeyDown(RPGButton))
                {
                    RPG = false;
                }
            }
        }

        //offline Shooting
        if(!IsThisMultiplayer.Instance.multiplayer)
        {
            if (!RPG)
            {
                rpgGo.SetActive(false);
                

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
                if (Input.GetKeyDown(RPGButton))
                {
                    RPG = true;
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
                    RPG = false;
                }
            }
        }
    }

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
        GameObject grenade = Instantiate(rocket, rocketspawn.transform.position, rpgGo.transform.rotation);
        grenade.GetComponent<Rigidbody>().AddForce(rpgGo.transform.transform.forward * 100, ForceMode.Impulse);
    }

    [PunRPC]
    void ShootRPG()
    {
        GameObject grenade = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Grenade"), rocketspawn.transform.position, rpgGo.transform.rotation, 0);
        grenade.GetComponent<Rigidbody>().AddForce(rpgGo.transform.transform.forward * 100, ForceMode.Impulse);
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
        Vector3 direction = Spread(maxDeviation);
        RaycastHit hit; //gets the information on whats hit
        if (Physics.Raycast(bulletSpawnPoint.transform.position, direction, out hit, range))
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

    Vector3 Spread(float maxDeviation)
    {
        Vector3 forwardVector = Vector3.forward;
        float deviation = Random.Range(0f, maxDeviation);
        float angle = Random.Range(0f, 360f);
        forwardVector = Quaternion.AngleAxis(deviation, Vector3.up) * forwardVector;
        forwardVector = Quaternion.AngleAxis(angle, Vector3.forward) * forwardVector;
        forwardVector = bulletSpawnPoint.transform.rotation * forwardVector;
        return forwardVector;
    }


    void OfflineShoot()
    {
        muzzleFlash.Play();
        Vector3 direction = Spread(maxDeviation);

        RaycastHit hit; //gets the information on whats hit
        if (Physics.Raycast(bulletSpawnPoint.transform.position, direction, out hit, range))
        {
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
