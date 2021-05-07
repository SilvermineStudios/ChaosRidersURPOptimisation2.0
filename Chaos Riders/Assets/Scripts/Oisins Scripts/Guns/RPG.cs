using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class RPG : MonoBehaviour
{
    //[SerializeField] GameObject explosionEffect;
    [SerializeField] GameObject trailSmokeVFX;
    [SerializeField] GameObject explosionVFX;
    [SerializeField] float damage;
    [SerializeField] float radius;
    [SerializeField] float timeUntilDeleted = 3f;

    private Rigidbody rb;
    private PhotonView pv;

    private void Awake()
    {
        pv = this.GetComponent<PhotonView>();
        rb = this.GetComponent<Rigidbody>();

        trailSmokeVFX.SetActive(true);
        explosionVFX.SetActive(false);
    }

    //new
    void ExplosiveDamage()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

        foreach (Collider nearby in colliders)
        {
            GameObject objectHit = nearby.transform.root.gameObject;
            Target target = objectHit.GetComponent<Target>();

            if (target != null && !target.hitByRPG)
            {
                target.hitByRPG = true;
                target.TakeDamage(damage);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        rb.velocity = Vector3.zero;
        rb.isKinematic = true;
        ExplosiveDamage();
        explosionVFX.SetActive(true);

        ExplosionCoroutine(timeUntilDeleted);
        

        /*
        if(IsThisMultiplayer.Instance.multiplayer && pv.IsMine)
        {
            pv.RPC("Explode", RpcTarget.All);
        }
        else
        {
            OfflineExplode();
        }
        */
    }

    private IEnumerator ExplosionCoroutine(float time)
    {
        yield return new WaitForSeconds(time);

        explosionVFX.SetActive(false);
        PhotonNetwork.Destroy(this.gameObject);
    }







    //Old Shit
    void OfflineExplode()
    {
        //Instantiate(explosionEffect, transform.position, transform.rotation);

        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

        foreach (Collider nearby in colliders)
        {
            Target health = nearby.GetComponent<Target>();
            if (health != null)
            {
                health.TakeDamage(damage);
            }

            /*
            //AIHealth aiHealth = GetComponent<AIHealth>();
            Target aiHealth = GetComponent<Target>();
            if (aiHealth != null)
            {
                aiHealth.TakeDamage(damage);
            }
            */
        }
        Destroy(gameObject);
    }

    [PunRPC]
    void Explode()
    {
        //PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "explosionEffect"), transform.position, transform.rotation, 0);

        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

        foreach (Collider nearby in colliders)
        {
            Target health = nearby.GetComponent<Target>();
            if (health != null)
            {
                health.TakeDamage(damage);
            }

            /*
            AIHealth aiHealth = GetComponent<AIHealth>();
            if (aiHealth != null)
            {
                aiHealth.TakeDamage(damage);
            }
            */
        }
        Destroy(gameObject);
    }
}
