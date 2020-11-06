using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.IO;
using UnityEngine.UI;

public class playerFinder : MonoBehaviour
{
    public bool dewit;

    private void Update()
    {
        if (!dewit) { return; }
        var photonViews = UnityEngine.Object.FindObjectsOfType<PhotonView>();
        foreach (var view in photonViews)
        {
            var player = view.Owner;
            //Objects in the scene don't have an owner, its means view.owner will be null
            if (player != null)
            {
                var playerPrefabObject = view.gameObject;
                gameObject.SetActive(false);
            }
        }
    }
}
