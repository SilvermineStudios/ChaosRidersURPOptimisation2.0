using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dumbCamera : MonoBehaviour
{

    public GameObject pointer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        transform.LookAt(pointer.transform.position);
    }
}
