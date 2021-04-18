using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class ExplosiveBarrel : MonoBehaviour
{
    private PhotonView pv;

    private MeshRenderer[] meshRenderers;
    private Collider[] colliders;
    [SerializeField] private float explodedForTime = 7f;

    [SerializeField] private float explosiveDamage = 60f;
    public static float ExplosiveDamage; //used in the Target script to take health off

    [SerializeField] private GameObject explosionEffect;
    

    void Awake()
    {
        pv = GetComponent<PhotonView>();
        meshRenderers = this.GetComponentsInChildren<MeshRenderer>();
        colliders = this.GetComponentsInChildren<Collider>();
        ExplosiveDamage = explosiveDamage;
    }

    void ExplodeBarrel()
    {
        //make invisible
        foreach (MeshRenderer mr in meshRenderers)
            mr.enabled = false;

        //disable the colliders
        foreach (Collider col in colliders)
            col.enabled = false;

        //spawn explosion
        if (IsThisMultiplayer.Instance.multiplayer)
        {
            pv.RPC("Explode", RpcTarget.All);
        }
        else
        {
            OfflineExplode();
        }
    }

    void ResetBarrel()
    {
        //make visable
        foreach (MeshRenderer mr in meshRenderers)
            mr.enabled = true;

        //enable the colliders
        foreach (Collider col in colliders)
            col.enabled = true;
    }

    [PunRPC]
    void Explode()
    {
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", explosionEffect.gameObject.name), transform.position, transform.rotation, 0);
    }

    void OfflineExplode()
    {
        Instantiate(explosionEffect, transform.position, transform.rotation);
    }




    private void OnCollisionEnter(Collision collision)
    {
        StartCoroutine(ExplodeCoroutine(explodedForTime));

        //collision.gameObject.GetComponent<Target>().health -= explosiveDamage;
    }

    private IEnumerator ExplodeCoroutine(float time)
    {
        ExplodeBarrel();

        yield return new WaitForSeconds(time);

        ResetBarrel();
    }
}
