using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBarrel : MonoBehaviour
{
    private MeshRenderer[] meshRenderers;
    private Collider[] colliders;
    [SerializeField] private float explodedForTime = 7f;

    private float explosiveDamage = 60f;
    public static float ExplosiveDamage; //used in the Target script to take health off
    

    void Awake()
    {
        meshRenderers = this.GetComponentsInChildren<MeshRenderer>();
        colliders = this.GetComponentsInChildren<Collider>();
        ExplosiveDamage = explosiveDamage;
    }

    void DisableBarrel()
    {
        //make invisible
        foreach (MeshRenderer mr in meshRenderers)
            mr.enabled = false;

        //disable the colliders
        foreach (Collider col in colliders)
            col.enabled = false;
    }

    void EnableBarrel()
    {
        //make visable
        foreach (MeshRenderer mr in meshRenderers)
            mr.enabled = true;

        //enable the colliders
        foreach (Collider col in colliders)
            col.enabled = true;
    }



    private void OnCollisionEnter(Collision collision)
    {
        StartCoroutine(ExplodeCoroutine(explodedForTime));

        collision.gameObject.GetComponent<Target>().health -= explosiveDamage;
    }

    private IEnumerator ExplodeCoroutine(float time)
    {
        DisableBarrel();

        yield return new WaitForSeconds(time);

        EnableBarrel();
    }
}
