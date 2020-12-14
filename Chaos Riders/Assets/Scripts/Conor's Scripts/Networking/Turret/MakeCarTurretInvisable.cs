using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MakeCarTurretInvisable : MonoBehaviour
{
    //each car has a turret on the back
    //this script makes the cars default turret invisable to the shooter
    //this is done so the there doesnt appear to be as much lag
    //the cars turret mimics the rotation of the shooter 

    private MoveTurretPosition mtp;
    private GameObject car;
    [SerializeField] private Transform carTurret;

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
        if(mtp.car != null && canAssign && IsThisMultiplayer.Instance.multiplayer)
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
