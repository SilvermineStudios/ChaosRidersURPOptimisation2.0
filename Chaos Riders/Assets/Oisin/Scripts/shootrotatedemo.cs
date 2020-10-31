using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shootrotatedemo : MonoBehaviour
{
    public GameObject target;
    public float speed;

    void Update()
    {

        transform.position = target.transform.position;

    }
}
