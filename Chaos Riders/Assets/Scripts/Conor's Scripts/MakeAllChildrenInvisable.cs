using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeAllChildrenInvisable : MonoBehaviour
{
    //[SerializeField] private MeshRenderer ammoMesh, barrelMesh, standMesh, platformMesh;
    [SerializeField] private MeshRenderer[] meshRenderers;

    // Update is called once per frame
    void Update()
    {
        meshRenderers = this.GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer mr in meshRenderers)
            {
                mr.enabled = false;
            }
    }
}
