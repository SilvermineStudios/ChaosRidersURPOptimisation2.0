using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class MoveTurretPosition : MonoBehaviour
{
    [SerializeField] private bool multiplayer = false;
    //private PhotonView pv;

    public GameObject car;
    private Transform carGunPos; 

    private Transform FakeParent;

    private Vector3 _positionOffset;
    private Quaternion _rotationOffset;

    private PlayerSpawner ps;
    private bool canConnect = true;

    private TurretTester turretTester;

    private Shooter shooterScript;

    private void OnEnable()
    {
        turretTester = GetComponent<TurretTester>();
        ps = FindObjectOfType<PlayerSpawner>();
        ps.gunners.Add(this.gameObject);
    }

    private void Awake()
    {
        //pv.GetComponent<PhotonView>();
        shooterScript = GetComponent<Shooter>();
    }

    private void Start()
    {
        if (FakeParent != null)
        {
            SetFakeParent(FakeParent);
        }
    }

    private void Update()
    {
        if (FakeParent == null)
            return;

        carGunPos = car.GetComponent<MultiplayerCarPrefabs>().gunSpawnPoint;

        transform.position = carGunPos.transform.position;

        /*
        var targetPos = carGunPos.position;
        var targetRot = car.transform.rotation;

        this.transform.position = RotatePointAroundPivot(targetPos, targetPos, targetRot);
        this.transform.localRotation = targetRot;
        */
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

    private void OnTriggerEnter(Collider other)
    {
        if (!canConnect) return;

        if (other.gameObject.tag == "car" && canConnect)
        {
            canConnect = false;
            car = other.gameObject;
            car.GetComponent<Controller>().ShooterAttached = turretTester;
            FakeParent = other.gameObject.transform;
            shooterScript.connectCar = true;
        }
    }
}
