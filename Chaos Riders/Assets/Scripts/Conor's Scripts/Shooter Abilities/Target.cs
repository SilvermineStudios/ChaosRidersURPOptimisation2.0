﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Target : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private bool ai = false;
    public bool invincible = false;
    public PhotonView pv;


    [Header("Health Stuff")]
    public float health;
    public Transform healthBarUi;
    public GameObject myHealthBar;
    [HideInInspector] public float startHealth;
    [HideInInspector] public float healthNormalized;
    private Slider healthbarSlider;
    private GameObject deathinstance;

    
    [Header("Death Stuff")]
    public bool dead;
    public bool respawning;
    [SerializeField] private GameObject deathExplosionVFX;
    [SerializeField] private float deathExplosionHeight = 3f;
    [SerializeField] private float deathTimer = 3;
    [SerializeField] private float timeSinceDeath;


    //used for stopping the target from being hit multiple times by the same thing
    [Header ("Stop Target Being Hit Multiple Times")]
    [HideInInspector] public bool hitByMine = false;
    private bool resettingHitByMine = false;
    [HideInInspector] public bool hitByRPG = false;
    private bool resettingHitByRPG = false;

    public bool isDead { get { return dead; } private set { isDead = dead; } }

    private void Awake()
    {
        pv = GetComponent<PhotonView>();
        startHealth = health;
        healthbarSlider = myHealthBar.GetComponentInChildren<Slider>();
        deathExplosionVFX.SetActive(false);
    }

    void Start()
    {
        if (pv.IsMine && IsThisMultiplayer.Instance.multiplayer && !ai || !IsThisMultiplayer.Instance.multiplayer)
            myHealthBar.SetActive(false);
    }

    void Update()
    {
        pv.RPC("SetHealth", RpcTarget.All);

        if (health < 0)
            health = 0;

        healthNormalized = (health / startHealth);

        DeathStuff();

        if (!ai)
            SetHealthBarUiSize(healthNormalized);

        if (hitByMine && !resettingHitByMine)
        {
            StartCoroutine(ResetMineBool());
            resettingHitByMine = true;
        }

        if(hitByRPG && !resettingHitByRPG)
        {
            StartCoroutine(ResetRPGBool());
            resettingHitByRPG = true;
        }
    }

    private IEnumerator ResetMineBool()
    {
        yield return new WaitForSeconds(1);
        resettingHitByMine = false;
        hitByMine = false;
    }

    private IEnumerator ResetRPGBool()
    {
        yield return new WaitForSeconds(1);
        resettingHitByRPG = false;
        hitByRPG = false;
    }

    private void SetHealthBarUiSize(float sizeNormalized)
    {
        //Debug.Log("Moving Player Health to: " + sizeNormalized);
        healthBarUi.localScale = new Vector3(1f, sizeNormalized);
    }

    private void DeathStuff()
    {
        if (health <= 0 && !dead)
            dead = true;

        if (dead && !respawning)
        {
            //pv.RPC("Die", RpcTarget.All);
            Die();
            respawning = true;
        }

        if (deathinstance != null)
        {
            float y = this.transform.position.y + deathExplosionHeight;
            Vector3 pos = new Vector3(this.transform.position.x, y, this.transform.position.z);
            deathinstance.transform.position = pos;
            deathinstance.transform.rotation = this.transform.rotation;
        }

        if (respawning)
        {
            if (timeSinceDeath > deathTimer)
            {
                dead = false;
                health = startHealth;
                Respawn();
                timeSinceDeath = 0;
            }
            else
            {
                timeSinceDeath += Time.deltaTime;
            }
        }
    }

    void Respawn()
    {
        Debug.Log("TURNED OFF REPSPAWN WHEN DEAD");
        //GetComponent<Checkpoint>().ResetPos();
    }

    public void TakeDamage(float amount)
    {
        if (!invincible)
        {
            //Debug.Log("Taking " + amount + " Damage");
            //if the amount of damage being dealt is more than the health set the amount of damage = to the health
            if (amount > health)
                amount = health;

            health -= amount;
        }
    }


    [PunRPC]
    void SetHealth()
    {
        healthbarSlider.value = healthNormalized;
    }


    void Die()
    {
        //deathParticles.SetActive(true);
        //Debug.Log("YOU FUCKING DIED!!!!!");
        StartCoroutine(DeathCourotine(deathTimer));
        //deathinstance = PhotonNetwork.Instantiate("DeathExplosion", this.transform.position, this.transform.rotation, 0);
    }


    private void OnCollisionEnter(Collision collision)
    {
        //Exlosive Barrel damage
        if(collision.gameObject.tag == "Explosive Barrel")
        {
            TakeDamage(TrapManager.ExplosiveBarrelDamage);
            //pv.RPC("RPC_TakeDamage", RpcTarget.All, this.gameObject, TrapManager.ExplosiveBarrelDamage);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //blade traps
        if(other.gameObject.tag == "Blade")
        {
            TakeDamage(TrapManager.BladeTrapDamage);
        }

        if(other.gameObject.tag == "Axe")
        {
            TakeDamage(TrapManager.AxeTrapDamage);
        }
    }

    private IEnumerator DeathCourotine(float time)
    {
        deathExplosionVFX.SetActive(true);
        FMODUnity.RuntimeManager.PlayOneShotAttached("event:/GunFX/RPG/Explosion", gameObject);

        yield return new WaitForSeconds(time);

        deathExplosionVFX.SetActive(false);

        respawning = false;
    }

    /*
    [PunRPC]
    void RPC_TakeDamage(float amountOfDamage)
    {
        TakeDamage(amountOfDamage);
    }
    */
}
