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

    

    void Start()
    {

        pickupRespawnTime = pickupRespawn;
        InvincibleTime = invincibleTimer;
        speedBoostTime = speedBoostTimer;
    }

}
