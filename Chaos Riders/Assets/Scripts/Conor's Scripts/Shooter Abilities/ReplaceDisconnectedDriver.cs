using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class ReplaceDisconnectedDriver : MonoBehaviour
{
    private GameObject car; //ref to the connected car
    private MoveTurretPosition mtp;

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
                    //GameObject AIReplacementBraker = Instantiate(AIBraker, this.transform.position, this.transform.rotation); //spawn the ai breaker
                    GameObject AIReplacementBraker = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "AI", AIBraker.name), this.transform.position, this.transform.rotation, 0);

                    mtp.FakeParent = AIReplacementBraker.transform; //attach the ai braker to the shooter in the moveTurretPosition Script
                    mtp.car = AIReplacementBraker;

                    AIReplacementBraker.GetComponent<AICarController>().healthBar.SetActive(false);
                    this.GetComponent<MatchDriversHealth>().CarHealth = AIReplacementBraker.GetComponent<Target>();
                }
                //shredder
                if (driverModelIndex == 1)
                {
                    //GameObject AIReplacementShredder = Instantiate(AIShredder, this.transform.position, this.transform.rotation); //spawn the ai shredder 
                    GameObject AIReplacementShredder = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "AI", AIShredder.name), this.transform.position, this.transform.rotation, 0);

                    mtp.FakeParent = AIReplacementShredder.transform;
                    mtp.car = AIReplacementShredder;

                    AIReplacementShredder.GetComponent<AICarController>().healthBar.SetActive(false);
                    this.GetComponent<MatchDriversHealth>().CarHealth = AIReplacementShredder.GetComponent<Target>();
                    this.GetComponent<Shooter>().car = AIReplacementShredder;
                }
                //colt
                if (driverModelIndex == 2)
                {
                    //GameObject AIReplacementColt = Instantiate(AIColt, this.transform.position, this.transform.rotation); //spawn the ai colt
                    GameObject AIReplacementColt = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "AI", AIColt.name), this.transform.position, this.transform.rotation, 0);

                    mtp.FakeParent = AIReplacementColt.transform;
                    mtp.car = AIReplacementColt;

                    AIReplacementColt.GetComponent<AICarController>().healthBar.SetActive(false);
                    this.GetComponent<MatchDriversHealth>().CarHealth = AIReplacementColt.GetComponent<Target>();
                }

                canReplaceDriver = false;
                //StartCoroutine(SpawnAIDelay(spawnAIDelay));
            }
        }
    }
}
