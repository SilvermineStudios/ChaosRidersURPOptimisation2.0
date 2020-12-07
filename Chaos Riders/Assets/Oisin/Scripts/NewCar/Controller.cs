using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


public class Controller : MonoBehaviour
{
    [SerializeField] private WheelCollider[] wheelColliders = new WheelCollider[4];
    [SerializeField] private Transform[] wheelMeshes = new Transform[4];
    [SerializeField] private Vector3 centerOfMass;
    [Range(0, 1)] [SerializeField] private float steerHelper;
    [SerializeField] private float tractionControl = 0;
    [SerializeField] private float fullTorqueOverAllWheels;
    [SerializeField] private float brakeTorque;
    [SerializeField] private float reverseTorque;
    [SerializeField] private float topspeed;
    [SerializeField] private float downforce;

    private CinemachineVirtualCamera cineCamera;
    public Skidmarks skidmarksController;
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
    Skidmarks skidFL, skidFR, skidBL, skidBR;
    float slipLimit = 0.3f;
    float skidLimit = 0.5f;

    private void Awake()
    {
        skidmarksController = FindObjectOfType<Skidmarks>();
    }

    private void Start()
    {

        skidFL = FindObjectOfType(typeof(Skidmarks)) as Skidmarks;
        skidFR = FindObjectOfType(typeof(Skidmarks)) as Skidmarks;
        skidBL = FindObjectOfType(typeof(Skidmarks)) as Skidmarks;
        skidBR = FindObjectOfType(typeof(Skidmarks)) as Skidmarks;
        

        
        cineCamera = gameObject.transform.GetChild(1).gameObject.GetComponent<CinemachineVirtualCamera>();
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

        
        float thrustTorque = -verticalInput * (currentTorque / 4f);

        if(boost)
        {
            thrustTorque = -verticalInput * 5000 * (currentTorque / 4f);
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
        
    }

    private void TractionControl()
    {
        WheelHit wheelHit;
        for (int i = 0; i < 4; i++)
        {
            wheelColliders[i].GetGroundHit(out wheelHit);

            AdjustTorque(wheelHit.forwardSlip);
        }
        
    }

    

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
        {
            rb.velocity = topspeed / 2.23693629f * rb.velocity.normalized;
        }
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
        Skid();
        TractionControl();
        CapSpeed();
        UpdateWheelPoses();
        ChangeFOV();
    }

    public Skidmarks[] skidmarks;
    public int[] lastSkid;

    private void Skid()
    {
        WheelHit wheelHit;
        for(int i = 0; i<4;i++)
        {
            wheelColliders[i].GetGroundHit(out wheelHit);
            Debug.Log(wheelHit.sidewaysSlip);

            if (((wheelHit.forwardSlip > skidLimit || wheelHit.forwardSlip < -skidLimit) || (wheelHit.sidewaysSlip > skidLimit || wheelHit.sidewaysSlip < -skidLimit)) && skidmarks[i] != null)
            {
                

                //lastSkid = skid.AddSkidMark(hit.point, hit.normal, Mathf.Abs(slipRatio) - 0.2f, lastSkid);
                //Debug.Log(2222);
                Vector3 skidPoint = new Vector3(wheelColliders[i].transform.position.x, wheelHit.point.y, wheelColliders[i].transform.position.z) + (rb.velocity * Time.deltaTime);
                lastSkid[i] = skidmarksController.AddSkidMark(skidPoint, wheelHit.normal, 0.5f, lastSkid[i]);
            }
            else
            {
                lastSkid[i] = -1;
            }
            
        }
    }
    //int lastSkid = -1;

    private void ChangeFOV()
    {
        //Debug.Log(currentSpeed);
        if(currentSpeed > cineCamera.m_Lens.FieldOfView)
        {
            cineCamera.m_Lens.FieldOfView = Mathf.Lerp(cineCamera.m_Lens.FieldOfView, currentSpeed, Time.deltaTime);
        }

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
