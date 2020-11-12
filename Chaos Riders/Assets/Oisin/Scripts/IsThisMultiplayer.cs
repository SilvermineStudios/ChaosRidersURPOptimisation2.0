using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsThisMultiplayer : MonoBehaviour
{

    public bool ThisIsMultiplayer;

    public static bool multiplayer;// { get { return multiplayer;} private set {} }

    private void Awake()
    {
        multiplayer = ThisIsMultiplayer;
    }

}
