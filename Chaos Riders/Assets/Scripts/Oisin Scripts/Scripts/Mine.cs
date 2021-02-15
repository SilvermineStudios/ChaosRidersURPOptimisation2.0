using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class Mine : MonoBehaviour
{
    [SerializeField] GameObject explosionEffect;
    [SerializeField] float damage;
    [SerializeField] float radius;
    [SerializeField] PhotonView pv;
    [SerializeField] float waitTime;

    private void FixedUpdate()
    {
        if(waitTime > 0)
        {
            waitTime -= Time.deltaTime;
        }
    }

    void OfflineExplode()
    {
        Instantiate(explosionEffect, transform.position, transform.rotation);

        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

        foreach (Collider nearby in colliders)
        {
            Target health = nearby.GetComponent<Target>();
            if (health != null)
            {
                health.TakeDamage(damage);
            }

            AIHealth aiHealth = GetComponent<AIHealth>();
            if (aiHealth != null)
            {
                aiHealth.TakeDamage(damage);
            }
        }
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (waitTime <= 0)
        {
            if (IsThisMultiplayer.Instance.multiplayer)
            {
                pv.RPC("Explode", RpcTarget.All);
            }
            else
            {
                OfflineExplode();
            }
        }
    }

    [PunRPC]
    void Explode()
    {
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "explosionEffect"), transform.position, transform.rotation, 0);

        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

        foreach (Collider nearby in colliders)
        {
            Target health = nearby.GetComponent<Target>();
            if (health != null)
            {
                health.TakeDamage(damage);
            }

            AIHealth aiHealth = GetComponent<AIHealth>();
            if (aiHealth != null)
            {
                aiHealth.TakeDamage(damage);
            }
        }

        Destroy(gameObject);
    }

}
