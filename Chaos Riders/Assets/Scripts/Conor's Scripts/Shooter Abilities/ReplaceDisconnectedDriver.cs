using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ReplaceDisconnectedDriver : MonoBehaviour
{
    private GameObject car; //ref to the connected car
    private MoveTurretPosition mtp;

    [SerializeField] private float spawnAIDelay = 3f;

    private Shooter ShooterScript;
    [SerializeField] private bool canReplaceDriver = false;
    [SerializeField] private int driverModelIndex = -1; //this is the same index from the Driver Title script; 0 = braker, 1 = shredder, 2 = colt
    [SerializeField] private GameObject AIBraker, AIColt, AIShredder;

    private PhotonView pv;

    private void Awake()
    {
        pv = GetComponent<PhotonView>();
        ShooterScript = GetComponent<Shooter>();
        mtp = GetComponent<MoveTurretPosition>();
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
                if(car.tag == "car")
                {
                    driverModelIndex = car.GetComponent<DriverTitle>().carIndex;
                }
            }

            //replace the disconnected driver
            if(canReplaceDriver && ShooterScript.car == null)
            {
                //Debug.Log("DRIVER DISCONNECTED");

                //braker
                if (driverModelIndex == 0)
                {
                    GameObject AIReplacementBraker = Instantiate(AIBraker, this.transform.position, this.transform.rotation); //spawn the ai breaker

                    mtp.FakeParent = AIReplacementBraker.transform; //attach the ai braker to the shooter in the moveTurretPosition Script
                    mtp.smoothCar = AIReplacementBraker;
                }
                //shredder
                if (driverModelIndex == 1)
                {
                    GameObject AIReplacementShredder = Instantiate(AIShredder, this.transform.position, this.transform.rotation); //spawn the ai shredder 

                    mtp.FakeParent = AIReplacementShredder.transform;
                    mtp.smoothCar = AIReplacementShredder;
                }
                //colt
                if (driverModelIndex == 2)
                {
                    GameObject AIReplacementColt = Instantiate(AIColt, this.transform.position, this.transform.rotation); //spawn the ai colt

                    mtp.FakeParent = AIReplacementColt.transform;
                    mtp.smoothCar = AIReplacementColt;
                }

                canReplaceDriver = false;
                //StartCoroutine(SpawnAIDelay(spawnAIDelay));
            }
        }
    }

    private IEnumerator SpawnAIDelay(float time)
    {
        yield return new WaitForSeconds(time);

        //braker
        if (driverModelIndex == 0)
        {
            GameObject AIReplacementBraker = Instantiate(AIBraker, this.transform.position, this.transform.rotation);
        }
        //shredder
        if (driverModelIndex == 1)
        {
            //GameObject AIReplacementBraker = Instantiate(x, this.transform.position, this.transform.rotation);
        }
        //colt
        if (driverModelIndex == 2)
        {
            GameObject AIReplacementBraker = Instantiate(AIColt, this.transform.position, this.transform.rotation);
        }

        canReplaceDriver = false;
    }
}
