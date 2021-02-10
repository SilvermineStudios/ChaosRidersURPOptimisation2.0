using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionAudio : MonoBehaviour
{
    void Awake()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/GunFX/RPG/Explosion", transform.position);
    }


}
