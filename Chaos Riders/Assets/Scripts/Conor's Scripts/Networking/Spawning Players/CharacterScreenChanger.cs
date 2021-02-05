using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;

public class CharacterScreenChanger : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject shooterCharacterScreen, driverCharacterScreen;
    private PhotonView pv;

    private void Start()
    {
        pv = GetComponent<PhotonView>();
    }

    void Update()
    {
        /*
        if (PhotonNetwork.PlayerList != null)
        {


            if (pv.IsMine && PhotonNetwork.PlayerList[1].ActorNumber == 1)
            {
                Debug.Log("Photon Value: " + pv.ViewID);
            }

            //drivers
            for (int i = 0; i > PhotonNetwork.PlayerList.Length; i += 2)
            {
                if (pv.IsMine)
                {

                }
                //shooterCharacterScreen.SetActive(false);
                //driverCharacterScreen.SetActive(true);
            }

            //shooters
            for (int i = 1; i > PhotonNetwork.PlayerList.Length; i += 2)
            {
                //shooterCharacterScreen.SetActive(true);
                //driverCharacterScreen.SetActive(false);
            }
        }
        */

        PlayerType();

    }

    private void PlayerType()
    {
        //Player 1
        if (PhotonNetwork.LocalPlayer.ActorNumber == 1)
        {
            shooterCharacterScreen.SetActive(false);
            driverCharacterScreen.SetActive(true);
        }
        //Player 2
        if (PhotonNetwork.LocalPlayer.ActorNumber == 2)
        {
            shooterCharacterScreen.SetActive(true);
            driverCharacterScreen.SetActive(false);
        }
    }
}
