using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class MoveTurretPosition : MonoBehaviour
{
    [SerializeField] private Transform gunstand;

    public GameObject car;
    private Transform carGunPos, carGunStandPosition;
    [SerializeField] private bool isAiGun = false;

    public Transform FakeParent;

    private Vector3 _positionOffset;
    private Quaternion _rotationOffset;

    private PlayerSpawner ps;
    private bool canConnect = true;

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

        if (IsThisMultiplayer.Instance.multiplayer && car != null && !isAiGun)
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
        if (car == null)
            return;

        carGunPos = car.GetComponent<MultiplayerCarPrefabs>().gunSpawnPoint;
        carGunStandPosition = car.GetComponent<MultiplayerCarPrefabs>().gunstand;

        transform.position = carGunPos.transform.position;
        transform.rotation = Quaternion.Euler(0, -90, 0);

        //var targetPos = carGunPos.position;
        var targetRot = car.transform.rotation;

        //transform.rotation = targetRot;
        //gunstand.localRotation = targetRot;
        gunstand.localRotation = Quaternion.Euler(0, targetRot.eulerAngles.y - targetRot.eulerAngles.y, 0);
        gunstand.transform.position = carGunStandPosition.transform.position;
        //Debug.Log("Attached to fake parent");

        if (car != null && car.tag == "car")
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
        car.GetComponent<ReplaceDisconnectedShooters>().shooter = this.gameObject;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!canConnect) return;

        /*
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
        */
        
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
        
    }
}
