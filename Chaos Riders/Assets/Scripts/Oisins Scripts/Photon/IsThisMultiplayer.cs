using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsThisMultiplayer : MonoBehaviour
{
    private static IsThisMultiplayer _instance;

    public static IsThisMultiplayer Instance { get { return _instance; } }

    public bool multiplayer { get { return ThisIsMultiplayer; } private set { } }
    public bool ThisIsMultiplayer;


    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        multiplayer = ThisIsMultiplayer;

    }
}


