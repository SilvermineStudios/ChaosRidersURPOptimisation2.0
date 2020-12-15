using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class MakeAllChildrenInvisable : MonoBehaviour
{
    [SerializeField] private bool makeInvisable = false;

    [SerializeField] private MeshRenderer[] meshRenderers;

    // Update is called once per frame
    void Update()
    {
        meshRenderers = this.GetComponentsInChildren<MeshRenderer>();

        if(makeInvisable)
        {
            foreach (MeshRenderer mr in meshRenderers)
            {
                mr.enabled = false;
            }
        }
        else
        {
            foreach (MeshRenderer mr in meshRenderers)
            {
                mr.enabled = true;
            }
        }
    }
}
