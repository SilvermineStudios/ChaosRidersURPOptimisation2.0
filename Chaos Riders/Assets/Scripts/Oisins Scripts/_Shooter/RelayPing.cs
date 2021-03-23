using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Knife.HDRPOutline.Core;
using Photon.Pun;

public class RelayPing : MonoBehaviour
{
    [SerializeField] OutlineObject outlineObject;

    IEnumerator Countdown()
    {
        yield return new WaitForSeconds(5);

        outlineObject.enabled = false;
    }

    public void RelayPingToOutline(PhotonView[] ourViews)
    {
        if (ourViews[0].IsMine || ourViews[1].IsMine)
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/Ping/Ping");
            outlineObject.enabled = true;
            StartCoroutine(Countdown());
        }
    }
}
