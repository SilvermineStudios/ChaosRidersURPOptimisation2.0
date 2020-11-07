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
    private Vector3 carpos;



    public Transform FakeParent;

    private Vector3 _positionOffset;
    private Quaternion _rotationOffset;

    private void Awake()
    {
        //pv.GetComponent<PhotonView>();
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
        //if(pv.IsMine)
        //{
            carGunPos = car.GetComponent<MultiplayerCarPrefabs>().gunSpawnPoint;

            if (FakeParent == null)
                return;

        var targetPos = carGunPos.position;
        var targetRot = car.transform.rotation;

        this.transform.position = RotatePointAroundPivot(targetPos, targetPos, targetRot);
        this.transform.localRotation = targetRot;
        //}
        //else if(!multiplayer)
    }

    private void FixedUpdate()
    {
        
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
        if (other.gameObject.tag == "car")
        {
            car = other.gameObject;
            FakeParent = other.gameObject.transform;
        }
    }
}
