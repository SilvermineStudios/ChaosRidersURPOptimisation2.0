using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
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
        //if the barrel is shot and out of health
        if (readyToExplode && barrelHealth <= 0)
        {
            StartCoroutine(ExplodeCoroutine(TrapManager.ExplodedForTime));
        }
    }
    
    public void TakeDamage()
    {
        barrelHealth -= 1;
    }

    #region Enable / Disable
    [PunRPC]
    void ExplodeBarrel()
    {
        FMODUnity.RuntimeManager.PlayOneShotAttached("event:/GunFX/RPG/Explosion", gameObject);

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
        if(IsThisMultiplayer.Instance.multiplayer)
        {
            foreach (Player p in PhotonNetwork.PlayerList)
            {
                if (p.IsLocal)
                {
                    pv.RPC("ExplodeBarrel", RpcTarget.All);
                    //Debug.Log("RPC EXPLODE");
                }
            }
        }
        else
        {
            ExplodeBarrel();
            //Debug.Log("OFFLINE EXPLODE");
        }
        

        yield return new WaitForSeconds(time);

        if (IsThisMultiplayer.Instance.multiplayer)
        {
            foreach (Player p in PhotonNetwork.PlayerList)
            {
                if (p.IsLocal)
                {
                    pv.RPC("ResetBarrel", RpcTarget.All);
                    //Debug.Log("RPC RESET");
                }
            }
        }
        else
        {
            ResetBarrel();
            //Debug.Log("OFFLINE RESET");
        }
    }
}
