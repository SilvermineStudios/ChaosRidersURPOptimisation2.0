using System.Collections;
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

    [SerializeField] private float health, startHealth, healthNormalized;
    [SerializeField] private bool setStartHealth = false;

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
                {
                    //healthBarAboveCar = car.GetComponent<Health>().myHealthBar;
                    //carHealthBarUI = car.GetComponent<Health>().healthBarUi;
                    healthBarAboveCar = car.GetComponent<Target>().myHealthBar;
                    carHealthBarUI = car.GetComponent<Target>().healthBarUi;
                }
                if(car.tag != "car")
                {
                    //healthBarAboveCar = car.GetComponent<AIHealth>().myHealthBar;
                    //health = car.GetComponent<AIHealth>().health;
                    //startHealth = car.GetComponent<AIHealth>().startHealth;
                    //healthNormalized = car.GetComponent<AIHealth>().healthNormalized;

                    healthBarAboveCar = car.GetComponent<Target>().myHealthBar;
                    health = car.GetComponent<Target>().health;
                    startHealth = car.GetComponent<Target>().startHealth;
                    healthNormalized = car.GetComponent<Target>().healthNormalized;

                    SetHealthBarUiSize(healthNormalized);
                }

                if (healthBarAboveCar != null)
                {
                    healthBarAboveCar.SetActive(false); //make the cars healthbar invisable to the gunner


                    //myHealthBarUi.localScale = carHealthBarUI.localScale;
                    //myHealthBarUi.localScale = new Vector3(1f, sizeNormalized);
                }

            }
        }
    }

    private void SetHealthBarUiSize(float sizeNormalized)
    {
        myHealthBarUi.localScale = new Vector3(1f, sizeNormalized);
    }
}
