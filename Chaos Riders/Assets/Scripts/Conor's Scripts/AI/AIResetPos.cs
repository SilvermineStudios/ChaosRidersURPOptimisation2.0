using System.Collections;
using UnityEngine;

public class AIResetPos : MonoBehaviour
{
    private Rigidbody rb;
    private AICarController controller;
    
    private float tooSlowAmount = 0.25f; //if the car is at this velocity or lower then it gets reset
    [SerializeField] private float maxTimeToBeStuck = 8f; //the amount of time the car has to be stuck before being reset

    public float xVelocity, yVelocity, zVelocity;
    private bool checkingIfTooSlow = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        controller = GetComponent<AICarController>();
    }

    private void Update()
    {
        if (!MasterClientRaceStart.Instance.countdownTimerStart) { return; }

        //Check if Stuck
        xVelocity = rb.velocity.x;
        yVelocity = -zVelocity;
        zVelocity = rb.velocity.z;
        if (zVelocity <= tooSlowAmount && zVelocity >= -tooSlowAmount && !checkingIfTooSlow)
        {
            //Debug.Log("Not Moving");
            StartCoroutine(CarNotMovingCheck(maxTimeToBeStuck));
            checkingIfTooSlow = true;
        }
    }


    private IEnumerator CarNotMovingCheck(float time)
    {
        //Debug.Log("Starting...");

        yield return new WaitForSeconds(time);

        checkingIfTooSlow = false;

        //stuck
        if (zVelocity <= tooSlowAmount && zVelocity >= -tooSlowAmount)
        {
            ResetPos();
        }
    }


    private void ResetPos()
    {
        rb.velocity = Vector3.zero;
        controller.ResetPosition();
    }
}
