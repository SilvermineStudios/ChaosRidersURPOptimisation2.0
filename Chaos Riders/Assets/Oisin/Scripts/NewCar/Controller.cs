using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    [SerializeField] private WheelCollider[] wheelColliders = new WheelCollider[4];
    [SerializeField] private Transform[] wheelMeshes = new Transform[4];
    [SerializeField] private Vector3 centerOfMass;
    [Range(0, 1)] [SerializeField] private float steerHelper;
    [SerializeField] private float tractionControl = 1;
    [SerializeField] private float fullTorqueOverAllWheels;
    [SerializeField] private float brakeTorque;
    [SerializeField] private float reverseTorque;
    [SerializeField] private float topspeed;
    [SerializeField] private float downforce;


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
        rb.centerOfMass = centerOfMass;
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

        if(boost)
        {
            thrustTorque = verticalInput * 5000 * (currentTorque / 4f);
            topspeed = 10000f;
        }
        else
        {
            topspeed = 120f;
        }

        for (int i = 0; i < 4; i++)
        {
            wheelColliders[i].motorTorque = thrustTorque;
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
                wheelColliders[i].motorTorque = -reverseTorque * verticalInput;
            }
        }
    }

    private void TractionControl()
    {
        WheelHit wheelHit;
        // loop through all wheels
        for (int i = 0; i < 4; i++)
        {
            wheelColliders[i].GetGroundHit(out wheelHit);

            AdjustTorque(wheelHit.forwardSlip);
        }
        
    }

    float slipLimit = 0.3f;

    private void AdjustTorque(float forwardSlip)
    {
        if (forwardSlip >= slipLimit && currentTorque >= 0)
        {
            currentTorque -= 10 * tractionControl;
        }
        else
        {
            currentTorque += 10 * tractionControl;
            if (currentTorque > fullTorqueOverAllWheels)
            {
                currentTorque = fullTorqueOverAllWheels;
            }
        }
    }


    private void CapSpeed()
    {
        float speed = rb.velocity.magnitude;

        speed *= 2.23693629f;
        if (speed > topspeed)
            rb.velocity = (topspeed / 2.23693629f) * rb.velocity.normalized;

    }

    private void UpdateWheelPoses()
    {
        for(int i = 0; i< 4; i++)
        {
            UpdateWheelPose(wheelColliders[i], wheelMeshes[i]);
        }
    }

    private void UpdateWheelPose(WheelCollider collider, Transform transform)
    {
        Vector3 pos = transform.position;
        Quaternion quat = transform.rotation;

        collider.GetWorldPose(out pos, out quat);

        transform.position = pos;
        transform.rotation = quat;
    }

    private void FixedUpdate()
    {
        GetInput();
        Steer();
        HelpSteer();
        Accelerate();
        AddDownForce();
        TractionControl();
        CapSpeed();
        UpdateWheelPoses();
    }

    private void AddDownForce()
    {
       rb.AddForce(-transform.up * downforce * rb.velocity.magnitude);
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
