using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class shootrotatedemo : MonoBehaviour
{
    public GameObject target;
    public float speed;
    private PhotonView pv;

    private void Start()
    {
        pv = GetComponent<PhotonView>();
    }

    void Update()
    {
        if(pv.IsMine)
            transform.position = target.transform.position;
    }
}
