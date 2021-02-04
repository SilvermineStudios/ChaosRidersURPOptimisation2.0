using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.IO;

public class PhotonPlayer : MonoBehaviour
{
    public PhotonView pv;
    public GameObject myAvatar;
    public int myTeam; //driver = 1, shooter = 2

    private bool canSpawnShooters = false;
    [SerializeField] private float shooterSpawnDelay = 0.5f;

    private void Start()
    {
        pv = GetComponent<PhotonView>();
        if(pv.IsMine)
        {
            pv.RPC("RPC_GetTeam", RpcTarget.MasterClient); //call the get team rpc for the host
        }
    }

    private void Update()
    {
        if(myAvatar == null && myTeam != 0)
        {
            if (myTeam == 1)
            {
                if (pv.IsMine)
                {
                    myAvatar = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Braker"),
                        this.transform.position, this.transform.rotation, 0);
                }
            }
            else
            {
                if (pv.IsMine)
                {
                    myAvatar = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Shooter2"),
                        this.transform.position, this.transform.rotation, 0);
                }
            }
        }
    }



    [PunRPC]
    void RPC_GetTeam() //only executed on the master client
    {
        myTeam = GameSetup.GS.nextPlayersTeam;
        GameSetup.GS.UpdateTeam();
        pv.RPC("RPC_SentTeam", RpcTarget.OthersBuffered, myTeam);
    }

    [PunRPC]
    void RPC_SentTeam(int whichTeam)
    {
        myTeam = whichTeam;
    }

    private IEnumerator SpawnShooterDelay(float time)
    {
        yield return new WaitForSeconds(time);
        canSpawnShooters = true;
    }
}
