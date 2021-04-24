using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOffWithButton : MonoBehaviour
{
    public KeyCode buttonToPress = KeyCode.X;

    void Update()
    {
        if (Input.GetKeyDown(buttonToPress))
        {
            this.gameObject.SetActive(false);
        }
    }
}
