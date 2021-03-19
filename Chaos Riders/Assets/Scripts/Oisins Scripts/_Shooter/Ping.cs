using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Ping : MonoBehaviour
{
    [SerializeField] CanPing canPing;
    [SerializeField] float pingRadius = 1;
    RaycastHit hit;
    CinemachineVirtualCamera cineCamera;

    void Start()
    {
        cineCamera = GetComponent<Shooter>().cineCamera;
        
    }

    void Update()
    {
        if (Physics.SphereCast(cineCamera.transform.position, pingRadius, cineCamera.transform.forward, out hit, 999))
        {
            if (canPing.tags.Contains(hit.transform.gameObject.tag))
            {
                Debug.Log(hit.transform.gameObject.tag);
            }
        }
        Debug.DrawRay(cineCamera.transform.position, cineCamera.transform.forward * 100, Color.red);
    }
}
