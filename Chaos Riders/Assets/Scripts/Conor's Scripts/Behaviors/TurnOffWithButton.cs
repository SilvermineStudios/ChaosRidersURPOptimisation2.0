using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOffWithButton : MonoBehaviour
{
    bool isOn = true;
    GameObject[] children;

    void Update()
    {
        if (Input.GetButtonDown("LB"))
        {
            if(isOn)
            {
                foreach(Transform child in transform)
                {
                    child.gameObject.SetActive(false);
                }
                isOn = false;
            }
            else
            {
                foreach (Transform child in transform)
                {
                    child.gameObject.SetActive(true);
                }
                isOn = true;
            }
        }
    }
}
