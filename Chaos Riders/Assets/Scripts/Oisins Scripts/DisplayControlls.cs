using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayControlls : MonoBehaviour
{
    [SerializeField] GameObject[] Keyboard;
    [SerializeField] GameObject[] Controller;
    string[] Controllers;
   
    
    void Update()
    {
        Controllers = Input.GetJoystickNames();

        if(Controllers == null)
        {
            foreach (GameObject g in Keyboard)
            {
                g.SetActive(true);
            }
            foreach (GameObject g in Controller)
            {
                g.SetActive(false);
            }
        }
        else
        {
            foreach (GameObject g in Controller)
            {
                g.SetActive(true);
            }
            foreach (GameObject g in Keyboard)
            {
                g.SetActive(false);
            }
        }
    }
}
