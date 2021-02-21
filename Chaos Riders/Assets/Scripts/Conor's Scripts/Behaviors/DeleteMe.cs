using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class DeleteMe : MonoBehaviour
{
    [SerializeField] private float timeToDelete = 1f;
    private PhotonView pv;


    void Awake()
    {
        pv = GetComponent<PhotonView>();
        //if multiplayer
        if (IsThisMultiplayer.Instance.multiplayer && pv.IsMine )
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
        PhotonNetwork.Destroy(this.gameObject); //delete over photon network
    }
}
