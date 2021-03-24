using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class rotateSlow : MonoBehaviour
{
    public float speed;

    public bool x, y, z;

    void Update()
    {
        if (z)
        {
            gameObject.transform.Rotate(0, 0, speed * Time.deltaTime);
        }
        if (x)
        {
            gameObject.transform.Rotate(speed * Time.deltaTime, 0, 0);
        }
        if (y)
        {
            gameObject.transform.Rotate(0, speed * Time.deltaTime, 0);
        }

    }
}
