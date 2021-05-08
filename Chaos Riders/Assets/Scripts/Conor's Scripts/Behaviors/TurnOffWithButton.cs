using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOffWithButton : MonoBehaviour
{
    bool isOn = true;
    GameObject[] children;

    private void Start()
    {
        if(PlayerPrefs.GetInt("Controls") < 0)
        {
            isOn = false;
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }
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
                PlayerPrefs.SetInt("Controls", -1);
            }
            else
            {
                foreach (Transform child in transform)
                {
                    child.gameObject.SetActive(true);
                }
                isOn = true;
                PlayerPrefs.SetInt("Controls", 1);
            }
        }
    }
}
