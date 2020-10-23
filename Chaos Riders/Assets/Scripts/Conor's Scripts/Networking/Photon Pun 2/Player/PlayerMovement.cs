using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private GameObject playerCam;

    [SerializeField] private float movementSpeed = 6f;
    [SerializeField] private float rotationSpeed = 300f;
    [SerializeField] private float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    public CharacterController controller;
    public Transform cam;


    void Awake()
    {
        if (GetComponent<PhotonView>().IsMine)
        {
            playerCam.SetActive(true);
        }
    }

    void Update()
    {
        if(GetComponent<PhotonView>().IsMine)
        {
            Movement();
        }
    }

    void Movement()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDirection.normalized * movementSpeed * Time.deltaTime);
        }
    }
}
