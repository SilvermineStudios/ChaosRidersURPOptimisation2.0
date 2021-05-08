using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class LinkToCM : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera inspecCam;
    public CinemachineVirtualCamera CMcamera { get; private set; }

    private void OnEnable()
    {
        CMcamera = inspecCam;
    }
}
