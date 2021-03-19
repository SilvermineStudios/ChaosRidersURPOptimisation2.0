using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class findcenter : MonoBehaviour
{

    public GameObject o;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       //Debug.Log( GetComponent<Renderer>().bounds.center);
        o.transform.position = GetComponent<Renderer>().bounds.center;
    }
}
