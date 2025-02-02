﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class RaycastMinigun : MonoBehaviour
{
    private bool carConnected = false;

    public GameObject spawnpoint,barrel;

    GameObject car, carCollision;

    public GameObject pointer;
    public float timeSinceLastBullet, fireRate;
    public PhotonView pv;
    private AudioSource speaker;
    public AudioClip gunShot;
    [SerializeField] private float volume = 0.3f;
    //public Health healthScript;
    public Target healthScript;
    public float minigunDamage;
    [SerializeField] float playerNumber = 1;

    public LayerMask layerMask;
    public LineRenderer lr;


    private void Start()
    {
        pv = GetComponent<PhotonView>();
        timeSinceLastBullet = fireRate;
        speaker = GetComponent<AudioSource>();
        lr = GetComponentInChildren<LineRenderer>();
    }

    void Update()
    {       
        if(car == null && !carConnected)
        {
            car = GetComponentInParent<MoveTurretPosition>().car;
            carCollision = GetComponentInParent<MoveTurretPosition>().car.transform.GetChild(0).gameObject; 
            //carCollision = car.transform.GetChild(0).gameObject;
            //carCollision = car;
        }

        if(car != null) //<--------------------------------------------------------------------------------------NEW STOPS ERRORS IF CAR DISCONNECTS    
        {
            carConnected = true;
        }

        //if (!pv.IsMine && IsThisMultiplayer.Instance.multiplayer) { return; }

        //if (pv.IsMine && healthScript.isDead)
        //{
            //return;
        //}

        //new
        if(pv.IsMine && IsThisMultiplayer.Instance.multiplayer)
        {
            if (Input.GetAxis("RT") > 0.01f || Input.GetButton("Fire1"))
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
                lr.enabled = false;
            }
        }
        else if (!IsThisMultiplayer.Instance.multiplayer)
        {
            if (Input.GetAxis("RT") > 0.01f || Input.GetButton("Fire1"))
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
                lr.enabled = false;
            }
        }

        /* old
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
            lr.enabled = false;
        }
        */
    }

    Vector3 raycastDir;
    void fireBullet()
    {
        RaycastHit hit;
        raycastDir = pointer.transform.position - transform.position;


        if (Physics.Raycast(spawnpoint.transform.position, raycastDir, out hit, Mathf.Infinity, layerMask))
        {
            if (hit.transform.gameObject.layer == 10 && hit.transform.gameObject != car && hit.transform.gameObject != carCollision)
            {
                float[] DamagetoTake = new float[2];
                DamagetoTake[0] = minigunDamage;
                DamagetoTake[1] = playerNumber;
                hit.transform.gameObject.SendMessage("TakeDamage", DamagetoTake);
                Debug.Log("Did Hit");
                Debug.Log(hit.transform.gameObject);
                Debug.Log(car);

            }
            else
            {
                Debug.Log("Did not Hit");
            }
        }


        lr.enabled = true;
        lr.SetPosition(0, spawnpoint.transform.position);
        lr.SetPosition(1, hit.point);

        speaker.PlayOneShot(gunShot, volume);
    }





}