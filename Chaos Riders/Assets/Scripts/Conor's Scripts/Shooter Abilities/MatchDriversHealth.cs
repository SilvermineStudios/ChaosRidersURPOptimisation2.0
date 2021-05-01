using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MatchDriversHealth : MonoBehaviour
{
    private Shooter ShooterScript; //ref to the shooter script so you can access the car gameobject
    public Target CarHealth; //ref to the shooter script so you can access the car gameobject
    private GameObject car; //ref to the connected car
    [SerializeField] private Transform myHealthBarUi;

    private PhotonView pv;


    void Awake()
    {
        pv = GetComponent<PhotonView>();
        ShooterScript = GetComponent<Shooter>();
    }

    void Update()
    {
        if (pv.IsMine && IsThisMultiplayer.Instance.multiplayer || !IsThisMultiplayer.Instance.multiplayer)
        {
            /*
            //if there is a car connected to the shooter
            if (ShooterScript.car != null)
            {
                car = ShooterScript.car;

                if (CarHealth == null)
                    CarHealth = car.GetComponent<Target>();

                CarHealth.myHealthBar.SetActive(false); //make the cars healthbar invisable to the gunner
                SetHealthBarUiSize(CarHealth.healthNormalized);
            }
            */

            //assign carhealth if it hasnt been assigned or if the car disconnects
            if(CarHealth == null && ShooterScript.car != null)
            {
                CarHealth = ShooterScript.car.GetComponent<Target>();
                car = ShooterScript.car;
            }

            //match the health bar with the connected drivers health
            if(CarHealth != null)
            {
                CarHealth.myHealthBar.SetActive(false); //make the cars healthbar invisable to the gunner
                SetHealthBarUiSize(CarHealth.healthNormalized);
            }
        }
    }

    private void SetHealthBarUiSize(float sizeNormalized)
    {
        //Debug.Log("Health Normalized is: " + CarHealth.healthNormalized);
        myHealthBarUi.localScale = new Vector3(1f, sizeNormalized);
    }
}
