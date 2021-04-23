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

    public float explodedForTime = 7f;
    public int barrelHealth = 5; //amount of bullets it takes to destroy
    private int startHealth;
    private bool readyToExplode = true;

    [SerializeField] private float explosiveDamage = 60f;
    public static float ExplosiveDamage; //used in the Target script to take health off

    [SerializeField] private GameObject explosionEffect;
    

    void Awake()
    {
        pv = GetComponent<PhotonView>();
        meshRenderers = this.GetComponentsInChildren<MeshRenderer>();
        colliders = this.GetComponentsInChildren<Collider>();
        ExplosiveDamage = explosiveDamage;
        startHealth = barrelHealth;
    }

    void Update()
    {
        //if the barrel is shot
        if(readyToExplode && barrelHealth <= 0)
        {
            StartCoroutine(ExplodeCoroutine(explodedForTime));
        }  
    }

    public void TakeDamage()
    {
        //Debug.Log("Barrel Took Damage");
        barrelHealth -= 1;
    }

    #region Enable / Disable
    [PunRPC]
    void RPC_ExplodeBarrel()
    {
        readyToExplode = false;

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

    [PunRPC]
    void RPC_ResetBarrel()
    {
        readyToExplode = true;
        barrelHealth = startHealth;

        //make visable
        foreach (MeshRenderer mr in meshRenderers)
            mr.enabled = true;

        //enable the colliders
        foreach (Collider col in colliders)
            col.enabled = true;
    }
    #endregion

    #region explosion effect
    [PunRPC]
    void Explode()
    {
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", explosionEffect.gameObject.name), transform.position, transform.rotation, 0);
    }

    void OfflineExplode()
    {
        Instantiate(explosionEffect, transform.position, transform.rotation);
    }
    #endregion


    private void OnCollisionEnter(Collision collision)
    {
        StartCoroutine(ExplodeCoroutine(explodedForTime));
    }

    public IEnumerator ExplodeCoroutine(float time)
    {
        if (IsThisMultiplayer.Instance.multiplayer)
            pv.RPC("RPC_ExplodeBarrel", RpcTarget.All);
        else
            RPC_ExplodeBarrel();
        
        yield return new WaitForSeconds(time);

        if (IsThisMultiplayer.Instance.multiplayer)
            pv.RPC("RPC_ResetBarrel", RpcTarget.All);
        else
            RPC_ResetBarrel();
    }
}
