using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;

public class CharacterScreenChanger : MonoBehaviourPunCallbacks
{
    private PhotonView pv;

    [SerializeField] private GameVariables gameVariables;
    private PlayerDataManager playerDataManager;
    [SerializeField] private PhotonMenuPlayer myPhotonMenuPlayer;

    public GameObject shooterCharacterScreen, driverCharacterScreen;
    public GameObject choosePlayerTypePanel; //panel for choosing whether to be a driver or shooter 
    public GameObject backButton;

    [SerializeField] private PhotonMenuPlayer[] photonMenuPlayers;

    private void Awake()
    {
        playerDataManager = FindObjectOfType<PlayerDataManager>();
        pv = GetComponent<PhotonView>();
        gameVariables = FindObjectOfType<GameVariables>();
    }

    private void Start()
    {
        choosePlayerTypePanel.SetActive(true);
        shooterCharacterScreen.SetActive(false);
        driverCharacterScreen.SetActive(false);
        backButton.SetActive(false);
    }

    void Update()
    {
        //PlayerType();
        photonMenuPlayers = FindObjectsOfType<PhotonMenuPlayer>();

        if(photonMenuPlayers.Length > 0 && myPhotonMenuPlayer == null)
        {
            foreach(PhotonMenuPlayer pmp in photonMenuPlayers)
            {
                if(pmp.gameObject.GetComponent<PhotonView>().IsMine)
                {
                    myPhotonMenuPlayer = pmp;
                }
            }
        }
    }
    #region Buttons
    public void BackButton()
    {
        foreach (Player p in PhotonNetwork.PlayerList)
        {
            if (p == PhotonNetwork.LocalPlayer)
            {
                choosePlayerTypePanel.SetActive(true);
                shooterCharacterScreen.SetActive(false);
                driverCharacterScreen.SetActive(false);
                backButton.SetActive(false);

                pv.RPC("RPC_BackButton", RpcTarget.AllBuffered, p);
            }
        }
    }
    [PunRPC]
    private void RPC_BackButton(Player p)
    {
        foreach (PhotonMenuPlayer pmp in photonMenuPlayers)
        {
            if (pmp.gameObject.GetComponent<PhotonView>().Owner == p)
            {
                pmp.carModel = PhotonMenuPlayer.carType.None;
                pmp.shooterModel = PhotonMenuPlayer.shooterType.None;
                pmp.driver = false;
                pmp.shooter = false;
            }
        }
    }
    #endregion

    #region Driver Buttons
    //button for choosing to be a driver
    public void DriverButton()
    {
        foreach (Player p in PhotonNetwork.PlayerList)
        {
            if (p == PhotonNetwork.LocalPlayer)
            {
                choosePlayerTypePanel.SetActive(false);
                shooterCharacterScreen.SetActive(false);
                driverCharacterScreen.SetActive(true);
                backButton.SetActive(true);

                pv.RPC("RPC_AddToDrivers", RpcTarget.AllBuffered, p);
            }
        }
    }
    [PunRPC]
    void RPC_AddToDrivers(Player p)
    {
        foreach (PhotonMenuPlayer pmp in photonMenuPlayers)
        {
            if (pmp.gameObject.GetComponent<PhotonView>().Owner == p)
            {
                gameVariables.amountOfDrivers++;
                playerDataManager.drivers.Add(pmp.gameObject);

                pmp.driver = true;
                pmp.shooter = false;
            }
        }
    }    

    //pick between driver characters (braker / shredder etc.)
    public void DriverTypeButton(int whichCharacter)
    {
        foreach (Player p in PhotonNetwork.PlayerList)
        {
            if (p == PhotonNetwork.LocalPlayer)
            {
                if (DriverPlayerInfo.pi != null) //check if a playerInfo singleton exists
                {
                    DriverPlayerInfo.pi.mySelectedCharacter = whichCharacter;
                    PlayerPrefs.SetInt("MyCharacter", whichCharacter);
                }

                pv.RPC("RPC_AssignDriverCharacter", RpcTarget.AllBuffered, whichCharacter, p);
            }
        }
    }
    [PunRPC]
    void RPC_AssignDriverCharacter(int whichCharacter, Player p)
    {
        foreach (PhotonMenuPlayer pmp in photonMenuPlayers)
        {
            if (pmp.gameObject.GetComponent<PhotonView>().Owner == p)
            {
                //braker
                if (whichCharacter == 0)
                {
                    Debug.Log("Braker");
                    pmp.carModel = PhotonMenuPlayer.carType.Braker;
                    pmp.currentCarClass = CarClass.Braker;
                }
                //shredder
                if (whichCharacter == 1)
                {
                    Debug.Log("Shredder");
                    pmp.carModel = PhotonMenuPlayer.carType.Shredder;
                    pmp.currentCarClass = CarClass.Shredder;
                }
            }
        }
        
    }
    #endregion

    #region Shooter Buttons
    //button for choosing to be a shooter
    public void ShooterButton()
    {
        foreach (Player p in PhotonNetwork.PlayerList)
        {
            if (p == PhotonNetwork.LocalPlayer)
            {
                choosePlayerTypePanel.SetActive(false);
                shooterCharacterScreen.SetActive(true);
                driverCharacterScreen.SetActive(false);
                backButton.SetActive(true);

                pv.RPC("RPC_AddToShooters", RpcTarget.AllBuffered, p);
            }
        }
    }
    [PunRPC]
    void RPC_AddToShooters(Player p)
    {
        foreach (PhotonMenuPlayer pmp in photonMenuPlayers)
        {
            if (pmp.gameObject.GetComponent<PhotonView>().Owner == p)
            {
                gameVariables.amountOfShooters++;
                playerDataManager.shooters.Add(pmp.gameObject);
                pmp.shooter = true;
                pmp.driver = false;
            }
        }
    }

    //pick between shooter characters (Dreagen Max / Celia Lock etc.)
    public void ShooterTypeButton(int whichCharacter)
    {
        foreach (Player p in PhotonNetwork.PlayerList)
        {
            if (p == PhotonNetwork.LocalPlayer)
            {
                if (ShooterPlayerInfo.pi != null) //check if a playerInfo singleton exists
                {
                    ShooterPlayerInfo.pi.mySelectedCharacter = whichCharacter;
                    PlayerPrefs.SetInt("MyCharacter", whichCharacter);
                }

                pv.RPC("RPC_AssignShooterCharacter", RpcTarget.AllBuffered, whichCharacter, p);
            }
        }
    }
    [PunRPC]
    void RPC_AssignShooterCharacter(int whichCharacter, Player p)
    {
        foreach (PhotonMenuPlayer pmp in photonMenuPlayers)
        {
            if (pmp.gameObject.GetComponent<PhotonView>().Owner == p)
            {
                //standard gun
                if (whichCharacter == 0)
                {
                    Debug.Log("standard gun");
                    pmp.shooterModel = PhotonMenuPlayer.shooterType.standardGun;
                    pmp.currentMinigunClass = MinigunClass.standard;
                }
                //golden gun
                if (whichCharacter == 1)
                {
                    Debug.Log("golden gun");
                    pmp.shooterModel = PhotonMenuPlayer.shooterType.goldenGun;
                    pmp.currentMinigunClass = MinigunClass.gold;
                }
            }
        }
        
    }
    #endregion
}
