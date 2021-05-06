using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class CarShooterMirror : MonoBehaviour
{
    private PhotonView pv;
    public bool on = true;

    [Header("Local")]
    public Transform localStand;
    public Transform localGunBarrel;
    public GameObject localMuzzleFlash;
    public GameObject localBulletCasings;

    [Header("Connected Shooter")]
    public GameObject shooter;
    public Transform connectedStand;
    public Transform connectedGunBarrel;
    public GameObject connectedMuzzleFlash;
    public GameObject connectedBulletCasings;

    void Awake()
    {
        pv = this.GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!on)
            return;

        if(shooter != null && shooter.GetComponent<PhotonView>() && !shooter.GetComponent<PhotonView>().IsMine)
        {
            DoTheStuff();
        }

        foreach(Player p in PhotonNetwork.PlayerList)
        {
            if(p.IsMasterClient && shooter != null && shooter.GetComponent<MoveTurretPosition>().isAiGun)
            {
                DoTheStuff();
            }
        }
    }

    void DoTheStuff()
    {
        MeshRenderer[] meshes = shooter.GetComponentsInChildren<MeshRenderer>();

        foreach (MeshRenderer mr in meshes)
        {
            mr.enabled = false;
        }



        if (connectedStand != null && localStand != null)
        {
            localStand.rotation = Quaternion.Euler(0f, connectedStand.rotation.y, 0f);
        }
        else
            Debug.Log("You Need To Connect the gunstand through the move turret position script");



        if (connectedGunBarrel != null && localGunBarrel != null)
        {
            localGunBarrel.rotation = Quaternion.Euler(connectedGunBarrel.rotation.x, connectedGunBarrel.rotation.y, 0f);
        }
        else
            Debug.Log("You Need To Connect the gun barrel through the move turret position script");



        if (connectedMuzzleFlash != null && localMuzzleFlash != null)
        {
            if (connectedMuzzleFlash.activeSelf)
                localMuzzleFlash.SetActive(true);
            else
                localMuzzleFlash.SetActive(false);

        }
        else
            Debug.Log("You Need To Connect the muzzleflash through the move turret position script");



        if (connectedBulletCasings != null && localBulletCasings != null)
        {
            if (connectedBulletCasings.activeSelf)
                localBulletCasings.SetActive(true);
            else
                localBulletCasings.SetActive(false);
        }
        else
            Debug.Log("You Need To Connect the bulletcasings through the move turret position script");
    }
}
