using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerNameBox : MonoBehaviour
{
    private PhotonView pv;

    [SerializeField] private GameObject none, braker, shredder, standardGun, goldenGun;

    // Start is called before the first frame update
    void Awake()
    {
        pv = GetComponent<PhotonView>();
        NoneSelected();
    }

    
    public void NoneSelected()
    {
        pv.RPC("RPC_None", RpcTarget.AllBuffered);
    }
    [PunRPC]
    void RPC_None()
    {
        none.SetActive(true);
        braker.SetActive(false);
        shredder.SetActive(false);
        standardGun.SetActive(false);
        goldenGun.SetActive(false);
    }

    #region Cars
    public void BrakerSelected()
    {
        pv.RPC("RPC_Braker", RpcTarget.AllBuffered);
    }
    [PunRPC]
    void RPC_Braker()
    {
        none.SetActive(false);
        braker.SetActive(true);
        shredder.SetActive(false);
        standardGun.SetActive(false);
        goldenGun.SetActive(false);
    }

    public void ShredderSelected()
    {
        pv.RPC("RPC_Shredder", RpcTarget.AllBuffered);
    }
    [PunRPC]
    void RPC_Shredder()
    {
        none.SetActive(false);
        braker.SetActive(false);
        shredder.SetActive(true);
        standardGun.SetActive(false);
        goldenGun.SetActive(false);
    }
    #endregion

    #region Guns
    public void StandardGunSelected()
    {
        pv.RPC("RPC_StandardGun", RpcTarget.AllBuffered);
    }
    [PunRPC]
    void RPC_StandardGun()
    {
        none.SetActive(false);
        braker.SetActive(false);
        shredder.SetActive(false);
        standardGun.SetActive(true);
        goldenGun.SetActive(false);
    }

    public void GoldenGunSelected()
    {
        pv.RPC("RPC_GoldenGun", RpcTarget.AllBuffered);
    }
    [PunRPC]
    void RPC_GoldenGun()
    {
        none.SetActive(false);
        braker.SetActive(false);
        shredder.SetActive(false);
        standardGun.SetActive(false);
        goldenGun.SetActive(true);
    }
    #endregion
}
