using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ReplaceDisconnectedShooters : MonoBehaviour
{
    private PhotonView pv;

    public GameObject shooter; //ref to the connected car
    [SerializeField] private bool canReplaceShooter = false;
    [SerializeField] private GameObject AIShooter;


    private void Awake()
    {
        pv = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        if (pv.IsMine && IsThisMultiplayer.Instance.multiplayer || !IsThisMultiplayer.Instance.multiplayer)
        { 
            if(shooter != null && !canReplaceShooter)
            {
                canReplaceShooter = true;
            }

            //replace the disconnected driver
            if (canReplaceShooter && shooter == null)
            {
                //Debug.Log("DRIVER DISCONNECTED");

                GameObject AiReplacementGun = Instantiate(AIShooter, this.transform.position, this.transform.rotation);
                shooter = AiReplacementGun;

                canReplaceShooter = false;
                //StartCoroutine(SpawnAIDelay(spawnAIDelay));
            }
        }
    }
}
