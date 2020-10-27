using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dumb : MonoBehaviour
{
    public GameObject target;
    public float Hspeed,Vspeed;
    float angle;
    void FixedUpdate()
    {

        target.transform.RotateAround(transform.position, Vector3.up, Input.GetAxis("Horizontal")* Hspeed * Time.deltaTime);
        target.transform.Translate(new Vector3(0, Input.GetAxis("Vertical") * Vspeed, 0));
        if(target.transform.position.y > 15)
        {
            target.transform.position = new Vector3(target.transform.position.x, 15, target.transform.position.z);
        }
        else if (target.transform.position.y < -1)
        {
            target.transform.position = new Vector3(target.transform.position.x, -1, target.transform.position.z);
        }
    }
}
