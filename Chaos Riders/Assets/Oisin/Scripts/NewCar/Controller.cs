using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    [SerializeField] private WheelCollider[] wheelColliders = new WheelCollider[4];
    [SerializeField] private GameObject[] m_WheelMeshes = new GameObject[4];
    [Range(0, 1)] [SerializeField] private float steerHelper;
    [Range(0, 1)] [SerializeField] private float tractionControl;
    [SerializeField] private float fullTorqueOverAllWheels;
    [SerializeField] private float brakeTorque;
    [SerializeField] private float reverseTorque;


    public float maxSteerAngle = 30;
    public float motorForce = 50;
    private float horizontalInput;
    private float verticalInput;
    private float steeringAngle;
    public bool boost;
    Rigidbody rb;
    float oldRot;
    float steerAngle;
    private float currentTorque;
    public float currentSpeed { get { return rb.velocity.magnitude * 2.23693629f; } }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentTorque = fullTorqueOverAllWheels - (tractionControl * fullTorqueOverAllWheels);
    }


    public void GetInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        boost = Input.GetKey(KeyCode.LeftShift);
    }

    private void Steer()
    {
        if(horizontalInput != 0)
        {
            steeringAngle = maxSteerAngle * horizontalInput;
        }
        else
        {
            steeringAngle = 0;
        }
        wheelColliders[0].steerAngle = steeringAngle;
        wheelColliders[1].steerAngle = steeringAngle;
    }
    
    private void Accelerate()
    {
        float thrustTorque = verticalInput * (currentTorque / 4f);

        for (int i = 0; i < 4; i++)
        {
            wheelColliders[i].motorTorque = -thrustTorque;
        }
        

        for (int i = 0; i < 4; i++)
        {
            if (currentSpeed > 5 && Vector3.Angle(transform.forward, rb.velocity) < 50f)
            {
                wheelColliders[i].brakeTorque = brakeTorque * verticalInput;
            }
            else if (verticalInput > 0)
            {
                wheelColliders[i].brakeTorque = 0f;
                wheelColliders[i].motorTorque = reverseTorque * verticalInput;
            }
        }
    }

    private void UpdateWheelPoses()
    {

    }

    private void UpdateWheelPose(WheelCollider _collider, Transform _transform)
    {
        Vector3 _pos = _transform.position;
        Quaternion _quat = _transform.rotation;

        _collider.GetWorldPose(out _pos, out _quat);

        _transform.position = _pos;
        _transform.rotation = _quat;
    }

    private void FixedUpdate()
    {
        GetInput();
        Steer();
        HelpSteer();
        Accelerate();
        //UpdateWheelPoses();
    }


    private void HelpSteer()
    {

        for (int i = 0; i < 4; i++)
        {
            WheelHit wheelhit;
            wheelColliders[i].GetGroundHit(out wheelhit);


            // wheels arent on the ground so dont realign the rigidbody velocity
            if (wheelhit.normal == Vector3.zero)
            {
                return;
            }
        }

        // this if is needed to avoid gimbal lock problems that will make the car suddenly shift direction
        if (Mathf.Abs(oldRot - transform.eulerAngles.y) < 10f)
        {
            var turnadjust = (transform.eulerAngles.y - oldRot) * steerHelper;
            Quaternion velRotation = Quaternion.AngleAxis(turnadjust, Vector3.up);
            rb.velocity = velRotation * rb.velocity;
        }
        oldRot = transform.eulerAngles.y;
    }

    
}
