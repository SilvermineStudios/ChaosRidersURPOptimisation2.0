using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayControlls : MonoBehaviour
{
    [SerializeField] GameObject[] Keyboard;
    [SerializeField] GameObject[] Controller;
    string[] Controllers;
    bool controller;

    private void Start()
    {
        Controllers = Input.GetJoystickNames();
        InvokeRepeating("CheckForControllers", 0, 1);
    }

    void Update()
    {
        Controllers = Input.GetJoystickNames();
        //Debug.Log(Controllers.Length);

        if(!controller)
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

    void CheckForControllers()
    {
        if (Controllers.Length > 0)
        {
            //Iterate over every element
            for (int i = 0; i < Controllers.Length; ++i)
            {
                //Check if the string is empty or not
                if (!string.IsNullOrEmpty(Controllers[i]))
                {
                    //Not empty, controller temp[i] is connected
                    Debug.Log("Controller " + i + " is connected using: " + Controllers[i]);
                    controller = true;
                    break;
                }
                else
                {
                    //If it is empty, controller i is disconnected
                    //where i indicates the controller number
                    Debug.Log("Controller: " + i + " is disconnected.");
                    controller = false;
                }
            }
        }
    }


}
