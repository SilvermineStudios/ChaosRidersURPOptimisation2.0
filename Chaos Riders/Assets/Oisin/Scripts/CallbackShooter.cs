using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallbackShooter : MonoBehaviour
{


    void Awake()
    {
        PlayerSpawner.shooter = gameObject;
    }


}
