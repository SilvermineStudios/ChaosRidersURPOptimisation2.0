using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsThisMultiplayer : MonoBehaviour
{
    /*

    private static IsThisMultiplayer instance;


    public bool ThisIsMultiplayer;

    
    public static bool multiplayer

    {
        get
        {
            return ;
        }
        private set
        {

        }
    }

    private void Awake()
    {
        multiplayer = ThisIsMultiplayer;
    }


    public GameObject playerInstance;
    public static GameObject player { get { return instance.playerInstance; } }
    void Awake()
    {
        instance = this;
    }
    */

    


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


