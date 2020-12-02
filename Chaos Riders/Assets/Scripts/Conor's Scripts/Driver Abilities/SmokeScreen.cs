using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeScreen : MonoBehaviour
{
    [SerializeField] private ParticleSystem smokeParticleSystem;


    void Start()
    {
        smokeParticleSystem.Stop();
    }

    
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && AbiliyCooldowns.canUseSmoke)
        {
            smokeParticleSystem.Play();
        }
    }
}
