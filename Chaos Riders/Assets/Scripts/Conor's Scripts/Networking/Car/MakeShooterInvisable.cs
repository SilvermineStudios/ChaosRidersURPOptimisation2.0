using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MakeShooterInvisable : MonoBehaviour
{
    //makes the turret invisable to all other players
    private PhotonView pv;
    [SerializeField] private MeshRenderer ammoMesh, barrelMesh, standMesh, platformMesh;


    void Start()
    {
        pv = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!pv.IsMine && IsThisMultiplayer.Instance.multiplayer)
        {
            ammoMesh.enabled = false;
            barrelMesh.enabled = false;
            standMesh.enabled = false;
            platformMesh.enabled = false;
        }
    }
}
