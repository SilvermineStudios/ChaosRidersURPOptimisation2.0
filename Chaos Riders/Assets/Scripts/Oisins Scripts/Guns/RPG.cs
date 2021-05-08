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
    [SerializeField] bool isFake;
    private Rigidbody rb;
    private PhotonView pv;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    private void Awake()
    {
        pv = this.GetComponent<PhotonView>();
        rb = this.GetComponent<Rigidbody>();

        trailSmokeVFX.SetActive(true);
        explosionVFX.SetActive(false);

        StartCoroutine(DestroyIfHitsNothing(20f));
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

        ExplosiveDamage();
        StartCoroutine(ExplosionCoroutine(timeUntilDeleted));
        

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
        //Debug.Log("RPG COUROUTINE STARTED");

        trailSmokeVFX.SetActive(false);
        explosionVFX.SetActive(true);

        rb.velocity = Vector3.zero;
        rb.isKinematic = true;

        Collider[] colliders = this.GetComponentsInChildren<Collider>();
        foreach (Collider col in colliders)
        {
            col.enabled = false;
        }

        yield return new WaitForSeconds(time);

        //Debug.Log("RPG COUROUTINE FINISHED");


        if (IsThisMultiplayer.Instance.multiplayer && pv.IsMine)
            PhotonNetwork.Destroy(this.gameObject);
        else
            Destroy(this.gameObject);
    }

    private IEnumerator DestroyIfHitsNothing(float time)
    {
        yield return new WaitForSeconds(time);

        if (IsThisMultiplayer.Instance.multiplayer && pv.IsMine)
            PhotonNetwork.Destroy(this.gameObject);
        else
            Destroy(this.gameObject);
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
