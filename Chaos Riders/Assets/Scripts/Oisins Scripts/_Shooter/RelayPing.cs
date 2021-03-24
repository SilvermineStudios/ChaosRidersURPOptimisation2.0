using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Knife.HDRPOutline.Core;
using Photon.Pun;

public class RelayPing : MonoBehaviour
{
    [SerializeField] OutlineObject outlineObject;
    PhotonView pv;

    private void Awake()
    {
        pv = GetComponent<PhotonView>();
    }

    IEnumerator Countdown()
    {
        yield return new WaitForSeconds(5);

        outlineObject.enabled = false;
    }

    public void RelayPingToOutline(PhotonView driverPV)
    {
        pv.RPC("SendAcrossNetwork", driverPV.Owner);
        SendAcrossNetwork();
    }


    [PunRPC]
    void SendAcrossNetwork()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Ping/Ping");
        outlineObject.enabled = true;
        StartCoroutine(Countdown());
        
    }
}
