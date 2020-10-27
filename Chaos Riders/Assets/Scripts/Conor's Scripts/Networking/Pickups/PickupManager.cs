using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupManager : MonoBehaviour
{
    //linked scripts: SpeedPickup, CarPickup

    [SerializeField] private float pickupRespawn = 5f;
    public static float pickupRespawnTime;

    [SerializeField] private float invincibleTimer = 5f;
    public static float InvincibleTime;

    [SerializeField] private float speedBoostTimer = 5f;
    public static float speedBoostTime;

    [SerializeField] private GameObject invinciblePic;
    public static GameObject invincibleUI;

    [SerializeField] private GameObject speedPic;
    public static GameObject speedUI;

    void Start()
    {
        invinciblePic.SetActive(false);
        speedPic.SetActive(false);

        pickupRespawnTime = pickupRespawn;
        InvincibleTime = invincibleTimer;
        speedBoostTime = speedBoostTimer;

        invincibleUI = invinciblePic;
        speedUI = speedPic;
    }

}
