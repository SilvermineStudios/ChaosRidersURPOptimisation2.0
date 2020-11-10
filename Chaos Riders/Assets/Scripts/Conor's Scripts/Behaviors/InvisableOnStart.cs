using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisableOnStart : MonoBehaviour
{
    private MeshRenderer myRenderer;
    void Awake()
    {
        myRenderer = GetComponent<MeshRenderer>();
        myRenderer.enabled = false;
    }
}
