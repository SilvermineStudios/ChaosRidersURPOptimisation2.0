﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MatchDriversHealth : MonoBehaviour
{
    private Shooter ShooterScript; //ref to the shooter script so you can access the car gameobject
    private GameObject car; //ref to the connected car
    [SerializeField] private GameObject healthBarAboveCar;
    [SerializeField] private Transform myHealthBarUi;
    private Transform carHealthBarUI;

    private PhotonView pv;



    void Awake()
    {
        pv = GetComponent<PhotonView>();
        ShooterScript = GetComponent<Shooter>();
    }

    void Start()
    {
        
    }


    void Update()
    {
        if (pv.IsMine && IsThisMultiplayer.Instance.multiplayer || !IsThisMultiplayer.Instance.multiplayer)
        {
            //if there is a car connected to the shooter
            if (ShooterScript.car != null)
            {
                car = ShooterScript.car;

                if(car.tag == "car")
                    healthBarAboveCar = car.GetComponent<Health>().myHealthBar;
                else
                    healthBarAboveCar = car.GetComponent<AIHealth>().myHealthBar;

                //healthBarAboveCar.SetActive(false); //make the cars healthbar invisable to the gunner

                //carHealthBarUI = car.GetComponent<Health>().healthBarUi;
                //myHealthBarUi.localScale = carHealthBarUI.localScale;
            }
        }
    }
}
