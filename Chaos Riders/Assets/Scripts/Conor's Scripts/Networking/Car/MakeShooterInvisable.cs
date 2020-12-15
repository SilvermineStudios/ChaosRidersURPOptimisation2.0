using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MakeShooterInvisable : MonoBehaviour
{
    //makes the turret invisable to all other players
    private PhotonView pv;
    //[SerializeField] private MeshRenderer ammoMesh, barrelMesh, standMesh, platformMesh;
    [SerializeField] private MeshRenderer[] meshRenderers;


    void Start()
    {
        pv = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        meshRenderers = this.GetComponentsInChildren<MeshRenderer>();
        if (!pv.IsMine && IsThisMultiplayer.Instance.multiplayer)
        {
            /*
            ammoMesh.enabled = false;
            barrelMesh.enabled = false;
            standMesh.enabled = false;
            platformMesh.enabled = false;
            */
            foreach(MeshRenderer mr in meshRenderers)
            {
                mr.enabled = false;
            }
        }
    }
}
