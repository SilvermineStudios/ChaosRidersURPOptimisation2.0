using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class RaycastMinigun : MonoBehaviour
{
    public GameObject spawnpoint,barrel;
    public GameObject pointer;
    public float timeSinceLastBullet, fireRate;
    public PhotonView pv;
    private AudioSource speaker;
    public AudioClip gunShot;

    public float minigunDamage;
    [SerializeField] float playerNumber = 1;

    public LayerMask layerMask;

    private void Start()
    {
        pv = transform.parent.parent.parent.GetComponent<PhotonView>();
        timeSinceLastBullet = fireRate;
        speaker = GetComponent<AudioSource>();
    }

    void Update()
    {
        
        if (!pv.IsMine && IsThisMultiplayer.Instance.multiplayer) { return; }

        if ((Input.GetAxis("RT") > 0.01f || Input.GetButton("Fire1")) && (pv.IsMine || !IsThisMultiplayer.Instance.multiplayer))
        {
            if (timeSinceLastBullet > fireRate)
            {
                fireBullet();
                timeSinceLastBullet = 0;
            }
            else
            {
                timeSinceLastBullet += Time.deltaTime;
            }
        }
        else
        {
            timeSinceLastBullet = fireRate;
        }
    }

    Vector3 raycastDir;
    void fireBullet()
    {
        RaycastHit hit;
        raycastDir = pointer.transform.position - transform.position;


        if (Physics.Raycast(spawnpoint.transform.position, raycastDir, out hit, Mathf.Infinity, layerMask))
        {
            float[] DamagetoTake = new float[2];
            DamagetoTake[0] = minigunDamage;
            DamagetoTake[1] = playerNumber;
            hit.transform.gameObject.SendMessage("TakeDamage", DamagetoTake);
            Debug.Log("Did Hit");
        }
        else
        {
            Debug.DrawRay(transform.position, raycastDir * 1000, Color.white);
            Debug.Log("Did not Hit");
        }

        speaker.PlayOneShot(gunShot);
    }


}