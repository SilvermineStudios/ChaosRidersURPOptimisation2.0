using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Health : MonoBehaviour
{
    public Transform healthBarUi;
    public GameObject myHealthBar;

    public float health;
    private float healthNormalized;
    private float startHealth;
    [SerializeField] float lastHit;

    public bool isProtected;

    Slider healthbar;
    [SerializeField] GameObject deathParticles;

    
    public bool isDead { get { return dead; } private set { isDead = dead; } }

    bool dead, respawning;
    PhotonView pv;

    void Awake()
    {
        pv = GetComponent<PhotonView>();
    }

    void Start()
    {
        startHealth = health;
        healthbar = GetComponentInChildren<Slider>();

        if (pv.IsMine && IsThisMultiplayer.Instance.multiplayer || !IsThisMultiplayer.Instance.multiplayer)
        {
            myHealthBar.SetActive(false);
        }
    }



    void Update()
    {
        if (health < 0)
            health = 0;

        healthNormalized = (health / startHealth);
        SetHealthBarUiSize(healthNormalized);

        healthbar.value = health;

        if(health <= 0 && !dead)
        {
            dead = true;
        }
        if(dead && !respawning)
        {
            pv.RPC("Die", RpcTarget.All);
            respawning = true;
        }
        if(respawning)
        {
            if (timeSinceDeath > deathTimer)
            {
                dead = false;
                health = startHealth;
                pv.RPC("Respawn", RpcTarget.All);
                timeSinceDeath = 0;
            }
            else
            {
                timeSinceDeath += Time.deltaTime;
            }
        }
    }

    float timeSinceDeath, deathTimer = 5;

    [PunRPC]
    void Die()
    {
        deathParticles.SetActive(true);
    }

    [PunRPC]
    void Respawn()
    {
        deathParticles.SetActive(false);
    }


    public void TakeDamage(float[] DamagetoTake)
    {
        if (!isProtected)
        {
            health -= DamagetoTake[0];
            lastHit = DamagetoTake[1];
        }
    }

    private void SetHealthBarUiSize(float sizeNormalized)
    {
        healthBarUi.localScale = new Vector3(1f, sizeNormalized);
    }



}
