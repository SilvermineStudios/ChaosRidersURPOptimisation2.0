﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ReplaceDisconnectedDriver : MonoBehaviour
{
    private GameObject car; //ref to the connected car
    private Shooter ShooterScript;
    [SerializeField] private bool canReplaceDriver = false;
    [SerializeField] private int driverModelIndex = -1; //this is the same index from the Driver Title script; 0 = braker, 1 = shredder, 2 = colt
    [SerializeField] private GameObject AIBraker, AIColt;

    private PhotonView pv;

    private void Awake()
    {
        pv = GetComponent<PhotonView>();
        ShooterScript = GetComponent<Shooter>();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (pv.IsMine && IsThisMultiplayer.Instance.multiplayer || !IsThisMultiplayer.Instance.multiplayer)
        {
            //if there is a car connected to the shooter
            if (ShooterScript.car != null)
            {
                car = ShooterScript.car;
                canReplaceDriver = true;
                driverModelIndex = car.GetComponent<DriverTitle>().carIndex;
            }

            //replace the disconnected driver
            if(canReplaceDriver && ShooterScript.car == null)
            {
                //Debug.Log("DRIVER DISCONNECTED");
                
                //braker
                if(driverModelIndex == 0)
                {
                    GameObject AIReplacementBraker = Instantiate(AIBraker, this.transform.position, this.transform.rotation);
                }
                //shredder
                if(driverModelIndex == 1)
                {

                }

                canReplaceDriver = false;
            }
        }
    }
}
