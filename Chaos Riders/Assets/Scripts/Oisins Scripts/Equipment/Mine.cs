using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class Mine : MonoBehaviour
{
    //[SerializeField] GameObject explosionEffect;
    [SerializeField] GameObject explosionEffectVFX;
    [SerializeField] float damage;
    [SerializeField] float radius;
    [SerializeField] PhotonView pv;
    [SerializeField] float waitTime;
    [SerializeField] float timeUntilDeleted = 5f;

    private void Awake()
    {
        explosionEffectVFX.SetActive(false);
    }

    private void FixedUpdate()
    {
        if(waitTime > 0)
        {
            waitTime -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (waitTime <= 0)
        {
            StartCoroutine(ExplodeCoroutine(timeUntilDeleted));


            /*
            if (!IsThisMultiplayer.Instance.multiplayer)
            {
                //OfflineExplode();
            }
            else
            {
                //pv.RPC("Explode", RpcTarget.All);
            }
            */
        }
    }

    private IEnumerator ExplodeCoroutine(float time)
    {
        explosionEffectVFX.SetActive(true);

        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

        foreach (Collider nearby in colliders)
        {
            GameObject objectHit = nearby.transform.root.gameObject;
            Target target = objectHit.GetComponent<Target>();

            if (target != null && !target.hitByMine)
            {
                target.hitByMine = true;
                target.TakeDamage(damage);
                Debug.Log("You hit " + objectHit.name + " which has a target script attached");
            }
        }

        yield return new WaitForSeconds(time);

        if (IsThisMultiplayer.Instance.multiplayer)
            PhotonNetwork.Destroy(this.gameObject);
        else
            Destroy(this.gameObject);
    }

    /*
    void OfflineExplode()
    {
        Instantiate(explosionEffect, transform.position, transform.rotation);

        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

        foreach (Collider nearby in colliders)
        {
            GameObject objectHit = nearby.transform.root.gameObject;
            Target target = objectHit.GetComponent<Target>();

            if (target != null && !target.hitByMine)
            {
                target.hitByMine = true;
                target.TakeDamage(damage);
                Debug.Log("You hit " + objectHit.name + " which has a target script attached");
            }
        }
        Destroy(gameObject);
    }

    [PunRPC]
    void Explode()
    {
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "explosionEffect"), transform.position, transform.rotation, 0);

        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

        foreach (Collider nearby in colliders)
        {
            GameObject objectHit = nearby.transform.root.gameObject;
            Target target = objectHit.GetComponent<Target>();

            if (target != null && !target.hitByMine)
            {
                target.hitByMine = true;
                target.TakeDamage(damage);
                Debug.Log("You hit " + objectHit.name + " which has a target script attached");
            }
        }
        Destroy(gameObject);
    }
    */
}
