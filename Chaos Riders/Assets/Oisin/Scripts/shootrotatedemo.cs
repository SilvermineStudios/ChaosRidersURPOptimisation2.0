using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shootrotatedemo : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public float speed;
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, Input.GetAxis("ControllerVert") * speed, 0);
    }
}
