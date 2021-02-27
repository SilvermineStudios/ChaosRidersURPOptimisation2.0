using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Cinemachine;
using TMPro;
using System.IO;
using UnityEngine.UI;

public class Rifle : MonoBehaviour
{
    [SerializeField] public ParticleSystem muzzleFlash;
    [SerializeField] public float rifleDamage;
    [SerializeField] public float range = 100f;
    [SerializeField] public float rifleFireRate;

    public float maxBulletDeviation;
    public float maxCrosshairDeviation;
    public float bulletDeviationIncrease;
    public float crosshairDeviationIncrease;


    public string sound = "event:/GunFX/Rifle/RifleShot";
    public string bulletWhistle = "event:/GunFX/Minigun/BulletWhistle";

    Shooter shooterScript;

    private void Start()
    {
        shooterScript = GetComponent<Shooter>();
    }


    private void Update()
    {

    }






}
