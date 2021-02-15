using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionAudio : MonoBehaviour
{
    void Awake()
    {
        FMODUnity.RuntimeManager.PlayOneShotAttached("event:/GunFX/RPG/Explosion", gameObject);
    }


}
