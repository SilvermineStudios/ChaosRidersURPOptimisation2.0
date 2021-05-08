using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class DeleteMe : MonoBehaviourPun
{
    [SerializeField] private float timeToDelete = 1f;
    private PhotonView pv;


    void Awake()
    {
        pv = GetComponent<PhotonView>();
        //if multiplayer
        if (IsThisMultiplayer.Instance.multiplayer)
        {
            StartCoroutine(DeleteTimer(timeToDelete));
        }
        
    }

    void Update()
    {
        //Way to delete if not multiplayer
        if (!IsThisMultiplayer.Instance.multiplayer)
            Destroy(this.gameObject, timeToDelete);
    }

    private IEnumerator DeleteTimer(float time)
    {
        yield return new WaitForSeconds(time);

        foreach (Player p in PhotonNetwork.PlayerList)
        {
            if (p.IsLocal)
            {
                PhotonNetwork.Destroy(this.gameObject); //delete over photon network
                //Debug.Log("RPC RESET");
            }
        }
        

        
    }
}
