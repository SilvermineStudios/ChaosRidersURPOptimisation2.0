﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MakeCarTurretInvisable : MonoBehaviour
{
    private MoveTurretPosition mtp;
    private GameObject car;
    private Transform carTurret;

    private MeshRenderer[] meshRenderers;
    private int gunMeshCount = 4; //ammo, barrel, stand, platform

    private PhotonView pv;
    private bool canAssign = true;

    void Start()
    {
        mtp = GetComponent<MoveTurretPosition>();
        meshRenderers = new MeshRenderer[gunMeshCount];
        pv = GetComponent<PhotonView>();
    }

    void Update()
    {
        if(mtp.car != null && canAssign && IsThisMultiplayer.multiplayer)
        {
            canAssign = false;
            car = mtp.car; ////////////////////////////////////////////////////////////STOP FROM LOOPING  <--- <--- <--- <--- <---
            carTurret = car.transform.Find("ShooterAttach");
            meshRenderers = carTurret.GetComponentsInChildren<MeshRenderer>();

            if (pv.IsMine)
            {
                foreach (MeshRenderer mr in meshRenderers)
                {
                    mr.enabled = false; //invisible: disable all the mesh renderers on the cars turret for the shooter
                }
            }
        }
    }
}
