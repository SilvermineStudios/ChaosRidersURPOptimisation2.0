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

                myPhotonMenuPlayer.carModel = PhotonMenuPlayer.carType.None;
                myPhotonMenuPlayer.shooterModel = PhotonMenuPlayer.shooterType.None;
                myPhotonMenuPlayer.driver = false;
                myPhotonMenuPlayer.shooter = false;
            }
        }
    }

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

                pv.RPC("AddToDrivers", PhotonNetwork.MasterClient);
            }
        }
    }

    [PunRPC]
    void AddToDrivers()
    {
        gameVariables.amountOfDrivers++;
        playerDataManager.drivers.Add(myPhotonMenuPlayer.gameObject);

        myPhotonMenuPlayer.driver = true;
        myPhotonMenuPlayer.shooter = false;
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

                pv.RPC("AssignDriverCharacter", RpcTarget.AllBuffered, whichCharacter);
            }
        }
    }

    [PunRPC]
    void AssignDriverCharacter(int whichCharacter)
    {
        //braker
        if (whichCharacter == 0)
        {
            Debug.Log("Braker");
            myPhotonMenuPlayer.carModel = PhotonMenuPlayer.carType.Braker;
            //carModel = carType.Braker;
        }
        //shredder
        if (whichCharacter == 1)
        {
            Debug.Log("Shredder");
            myPhotonMenuPlayer.carModel = PhotonMenuPlayer.carType.Shredder;
            //carModel = carType.Shredder;
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

                pv.RPC("AddToShooters", PhotonNetwork.MasterClient);
            }
        }
    }

    [PunRPC]
    void AddToShooters()
    {
        gameVariables.amountOfShooters++;
        playerDataManager.shooters.Add(myPhotonMenuPlayer.gameObject);

        myPhotonMenuPlayer.shooter = true;
        myPhotonMenuPlayer.driver = false;
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

                pv.RPC("AssignShooterCharacter", RpcTarget.AllBuffered, whichCharacter);
            }
        }
    }

    [PunRPC]
    void AssignShooterCharacter(int whichCharacter)
    {
        //standard gun
        if (whichCharacter == 0)
        {
            Debug.Log("standard gun");
            myPhotonMenuPlayer.shooterModel = PhotonMenuPlayer.shooterType.standardGun;
            //shooterModel = shooterType.standardGun;
        }
        //golden gun
        if (whichCharacter == 1)
        {
            Debug.Log("golden gun");
            myPhotonMenuPlayer.shooterModel = PhotonMenuPlayer.shooterType.goldenGun;
            //shooterModel = shooterType.goldenGun;
        }
    }
    #endregion
}
