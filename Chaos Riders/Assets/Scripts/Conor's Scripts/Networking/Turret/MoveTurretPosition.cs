using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class MoveTurretPosition : MonoBehaviour
{
    [SerializeField] private Transform gunstand;

    public GameObject smoothCar;
    public GameObject driverCar;
    private Transform carGunPos, carGunStandPosition;
    [SerializeField] private bool isAiGun = false;

    public Transform FakeParent;

    private Vector3 _positionOffset;
    private Quaternion _rotationOffset;

    private PlayerSpawner ps;
    private bool canConnectShooter = true;
    private bool canGetRefToDriverCar = true;

    [SerializeField] private TurretTester turretTester;

    private Shooter shooterScript;

    PhotonView pv;

    private void OnEnable()
    {
        turretTester = GetComponent<TurretTester>();
        //ps = FindObjectOfType<PlayerSpawner>();           //////////////////////////////////////////////////////////////////////////////////////
        //ps.gunners.Add(this.gameObject);                  //////////////////////////////////////////////////////////////////////////////////////
    }

    private void Awake()
    {
        //pv.GetComponent<PhotonView>();
        if(this.GetComponent<Shooter>())
            shooterScript = GetComponent<Shooter>();
    }

    private void Start()
    {
        pv = GetComponent<PhotonView>();
        if (FakeParent != null)
        {
            SetFakeParent(FakeParent);
        }
    }

    private void Update()
    {
        if (FakeParent == null)
            return;

        if (IsThisMultiplayer.Instance.multiplayer && smoothCar != null && !isAiGun)
        {
            pv.RPC("AttachToFakeParent", RpcTarget.All);
        }
        else
        {
            AttachToFakeParent();
        }
        
    }

    [PunRPC]
    void AttachToFakeParent()
    {
        carGunPos = smoothCar.GetComponent<MultiplayerCarPrefabs>().gunSpawnPoint;
        carGunStandPosition = smoothCar.GetComponent<MultiplayerCarPrefabs>().gunstand;

        transform.position = carGunPos.transform.position;


        //var targetPos = carGunPos.position;
        var targetRot = smoothCar.transform.rotation;

        //targetRot.x = 0;
        //targetRot.z = 0;

        //this.transform.position = RotatePointAroundPivot(targetPos, targetPos, targetRot);
        //this.transform.localRotation = targetRot;

        gunstand.localRotation = targetRot;
        gunstand.transform.position = carGunStandPosition.transform.position;
        //Debug.Log("Attached to fake parent");

        if (smoothCar != null && smoothCar.tag == "car")
            GiveGunRefrenceToCar();
    }


    public void SetFakeParent(Transform parent)
    {
        //Offset vector
        _positionOffset = parent.position - transform.position;
        //Offset rotation
        _rotationOffset = Quaternion.Inverse(parent.localRotation * transform.localRotation);
        //Our fake parent
        FakeParent = parent;
    }

    public Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Quaternion rotation)
    {
        //Get a direction from the pivot to the point
        Vector3 dir = point - pivot;
        //Rotate vector around pivot
        dir = rotation * dir;
        //Calc the rotated vector
        point = dir + pivot;
        //Return calculated vector
        return point;
    }

    void GiveGunRefrenceToCar()
    {
        smoothCar.GetComponent<ReplaceDisconnectedShooters>().shooter = this.gameObject;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!canConnectShooter) return;

        if (other.gameObject.tag == "Smooth Car" && canConnectShooter)
        {
            Debug.Log("Connect to smooth car here");

            canConnectShooter = false;

            smoothCar = other.gameObject;
            FakeParent = other.gameObject.transform;

            if (shooterScript != null)
                shooterScript.connectCar = true;
        }


        if(other.gameObject.layer == LayerMask.NameToLayer("Cars") && canGetRefToDriverCar)
        {
            canGetRefToDriverCar = false;

            if (other.gameObject.tag == "car")
            {
                driverCar = other.gameObject.transform.root.gameObject;
            }
            else
            {
                driverCar = other.gameObject;
            }
        }

        /*
        if (other.gameObject.layer == LayerMask.NameToLayer("Cars") && canConnect)
        {
            //Debug.Log("Connect to not smooth car here");

            canConnect = false;
            
            
            if (other.gameObject.tag == "car")
            {
                car = other.gameObject.transform.root.gameObject;
                //Debug.Log(car);
                FakeParent = other.gameObject.transform.root;
            }
            else
            {
                car = other.gameObject;
                FakeParent = other.gameObject.transform;
            }


            if (shooterScript != null)
                shooterScript.connectCar = true;
        }
        */
    }
}
