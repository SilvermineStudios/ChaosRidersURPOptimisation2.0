using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Shooter : MonoBehaviour
{
    [SerializeField] private Transform gunBarrel; //barrel that is going to rotate to face the correct direction
    [SerializeField] private float horizontalRotationSpeed = 5f, verticalRotationSpeed = 3f; //rotation speeds for the gun
    private float xAngle, yAngle; //angle of rotation for the gun axis

    private PhotonView pv;

    private void Awake()
    {
        pv = GetComponent<PhotonView>();
    }

    void Start()
    {
        if (pv.IsMine && IsThisMultiplayer.Instance.multiplayer || !IsThisMultiplayer.Instance.multiplayer)
        {

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (pv.IsMine && IsThisMultiplayer.Instance.multiplayer || !IsThisMultiplayer.Instance.multiplayer)
        {
            RotateGunBarrel();
        }
    }

    private void RotateGunBarrel()
    {
        xAngle += Input.GetAxis("Mouse X") * horizontalRotationSpeed * Time.deltaTime;
        //xAngle = Mathf.Clamp(xAngle, 0, 180); //use this if you want to clamp the rotation. second var = min angle, third var = max angle

        yAngle += Input.GetAxis("Mouse Y") * verticalRotationSpeed * -Time.deltaTime;
        //yAngle = Mathf.Clamp(yAngle, 0, 180); //use this if you want to clamp the rotation. second var = min angle, third var = max angle

        //gunBarrel.localRotation = Quaternion.AngleAxis(xAngle, Vector3.up); //only rotates on y
        //gunBarrel.localRotation = Quaternion.AngleAxis(yAngle, Vector3.right); //only rotates on x
        gunBarrel.localRotation = Quaternion.Euler(yAngle, xAngle, 0);
    }
}
