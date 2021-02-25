using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MiniMapPlayerColour : MonoBehaviour
{
    private PhotonView pv;
    private SpriteRenderer sr;

    private void Awake()
    {
        pv = this.transform.parent.gameObject.GetComponent<PhotonView>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        if (pv.IsMine)
        {
            sr.color = Color.green;
        }
        else
        {
            sr.color = Color.red;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
