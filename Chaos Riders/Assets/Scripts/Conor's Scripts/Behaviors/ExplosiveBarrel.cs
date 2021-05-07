using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class ExplosiveBarrel : MonoBehaviour
{
    private PhotonView pv;

    private MeshRenderer[] meshRenderers; //used for making the object invisible
    private Collider[] colliders; //used for disabling the colliders

    [SerializeField] private int barrelHealth, startHealth;
    private bool readyToExplode = true;

    //[SerializeField] private GameObject explosionEffect;
    [SerializeField] private GameObject explosionVFX;


    void Start()
    {
        pv = this.GetComponent<PhotonView>();
        meshRenderers = this.GetComponentsInChildren<MeshRenderer>();
        colliders = this.GetComponentsInChildren<Collider>();

        barrelHealth = TrapManager.BarrelHealth;
        startHealth = barrelHealth;

        explosionVFX.SetActive(false);
    }

    void Update()
    {
        //if the barrel is shot
        if (readyToExplode && barrelHealth <= 0)
        {
            StartCoroutine(ExplodeCoroutine(TrapManager.ExplodedForTime));
        }
    }

    public void TakeDamage()
    {
        //Debug.Log("Barrel Took Damage");
        barrelHealth -= 1;
    }

    #region Enable / Disable
    [PunRPC]
    void ExplodeBarrel()
    {
        readyToExplode = false;

        //make invisible
        foreach (MeshRenderer mr in meshRenderers)
            mr.enabled = false;

        //disable the colliders
        foreach (Collider col in colliders)
            col.enabled = false;

        //Instantiate(explosionEffect, transform.position, transform.rotation);
        explosionVFX.SetActive(true);
    }

    [PunRPC]
    void ResetBarrel()
    {
        readyToExplode = true;
        barrelHealth = startHealth;

        //make visable
        foreach (MeshRenderer mr in meshRenderers)
            mr.enabled = true;

        //enable the colliders
        foreach (Collider col in colliders)
            col.enabled = true;

        explosionVFX.SetActive(false);
    }
    #endregion

    private void OnCollisionEnter(Collision collision)
    {
        StartCoroutine(ExplodeCoroutine(TrapManager.ExplodedForTime));
    }

    public IEnumerator ExplodeCoroutine(float time)
    {
        if (IsThisMultiplayer.Instance.multiplayer)
            pv.RPC("ExplodeBarrel", RpcTarget.AllBuffered);
        else
            ExplodeBarrel();


        yield return new WaitForSeconds(time);


        if (IsThisMultiplayer.Instance.multiplayer)
            pv.RPC("ResetBarrel", RpcTarget.AllBuffered);
        else
            ResetBarrel();
    }
}
