using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OfflineHealth : MonoBehaviour
{
    private GameObject deathinstance;
    [SerializeField] private float deathExplosionHeight = 3f;

    public Transform healthBarUi;
    public GameObject myHealthBar;

    public float health;
    private float healthNormalized;
    private float startHealth;
    [SerializeField] float lastHit;

    public bool isProtected;

    Slider healthbar;
    [SerializeField] GameObject deathExplosion;


    public bool isDead { get { return dead; } private set { isDead = dead; } }

    public bool dead, respawning;


    void Start()
    {
        startHealth = health;
        healthbar = GetComponentInChildren<Slider>();

        myHealthBar.SetActive(false);
    }



    void Update()
    {

        //if (Input.GetKey(KeyCode.Space)) //<----------------------KILL YOURSELF!!!!
        //health -= 4f;

        if (health < 0)
            health = 0;

        healthNormalized = (health / startHealth);
        SetHealthBarUiSize(healthNormalized);

        SetHealth();

        if (health <= 0 && !dead)
        {
            dead = true;
        }
        if (dead && !respawning)
        {
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

    float timeSinceDeath, deathTimer = 6;

    void Die()
    {
        //deathParticles.SetActive(true);
        StartCoroutine(DeathCourotine(deathTimer));
        Instantiate(deathExplosion, this.transform.position, this.transform.rotation);
    }

    void Respawn()
    {
        //deathParticles.SetActive(false);
    }

    void SetHealth()
    {
        healthbar.value = healthNormalized;
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


    private IEnumerator DeathCourotine(float time)
    {

        yield return new WaitForSeconds(time);

        respawning = false;
    }
}
