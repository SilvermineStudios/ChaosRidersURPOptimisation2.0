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
        //Player 3
        if (PhotonNetwork.LocalPlayer.ActorNumber == 3)
        {
            shooterCharacterScreen.SetActive(false);
            driverCharacterScreen.SetActive(true);
        }
        //Player 4
        if (PhotonNetwork.LocalPlayer.ActorNumber == 4)
        {
            shooterCharacterScreen.SetActive(true);
            driverCharacterScreen.SetActive(false);
        }
        //Player 5
        if (PhotonNetwork.LocalPlayer.ActorNumber == 5)
        {
            shooterCharacterScreen.SetActive(false);
            driverCharacterScreen.SetActive(true);
        }
        //Player 6
        if (PhotonNetwork.LocalPlayer.ActorNumber == 6)
        {
            shooterCharacterScreen.SetActive(true);
            driverCharacterScreen.SetActive(false);
        }
        //Player 7
        if (PhotonNetwork.LocalPlayer.ActorNumber == 7)
        {
            shooterCharacterScreen.SetActive(false);
            driverCharacterScreen.SetActive(true);
        }
        //Player 8
        if (PhotonNetwork.LocalPlayer.ActorNumber == 8)
        {
            shooterCharacterScreen.SetActive(true);
            driverCharacterScreen.SetActive(false);
        }
        //Player 9
        if (PhotonNetwork.LocalPlayer.ActorNumber == 9)
        {
            shooterCharacterScreen.SetActive(false);
            driverCharacterScreen.SetActive(true);
        }
        //Player 10
        if (PhotonNetwork.LocalPlayer.ActorNumber == 10)
        {
            shooterCharacterScreen.SetActive(true);
            driverCharacterScreen.SetActive(false);
        }
        //Player 11
        if (PhotonNetwork.LocalPlayer.ActorNumber == 11)
        {
            shooterCharacterScreen.SetActive(false);
            driverCharacterScreen.SetActive(true);
        }
        //Player 12
        if (PhotonNetwork.LocalPlayer.ActorNumber == 12)
        {
            shooterCharacterScreen.SetActive(true);
            driverCharacterScreen.SetActive(false);
        }
        //Player 13
        if (PhotonNetwork.LocalPlayer.ActorNumber == 13)
        {
            shooterCharacterScreen.SetActive(false);
            driverCharacterScreen.SetActive(true);
        }
        //Player 14
        if (PhotonNetwork.LocalPlayer.ActorNumber == 14)
        {
            shooterCharacterScreen.SetActive(true);
            driverCharacterScreen.SetActive(false);
        }
        //Player 15
        if (PhotonNetwork.LocalPlayer.ActorNumber == 15)
        {
            shooterCharacterScreen.SetActive(false);
            driverCharacterScreen.SetActive(true);
        }
        //Player 16
        if (PhotonNetwork.LocalPlayer.ActorNumber == 16)
        {
            shooterCharacterScreen.SetActive(true);
            driverCharacterScreen.SetActive(false);
        }
        //Player 17
        if (PhotonNetwork.LocalPlayer.ActorNumber == 17)
        {
            shooterCharacterScreen.SetActive(false);
            driverCharacterScreen.SetActive(true);
        }
        //Player 18
        if (PhotonNetwork.LocalPlayer.ActorNumber == 18)
        {
            shooterCharacterScreen.SetActive(true);
            driverCharacterScreen.SetActive(false);
        }
        //Player 19
        if (PhotonNetwork.LocalPlayer.ActorNumber == 19)
        {
            shooterCharacterScreen.SetActive(false);
            driverCharacterScreen.SetActive(true);
        }
        //Player 20
        if (PhotonNetwork.LocalPlayer.ActorNumber == 20)
        {
            shooterCharacterScreen.SetActive(true);
            driverCharacterScreen.SetActive(false);
        }
    }
}
