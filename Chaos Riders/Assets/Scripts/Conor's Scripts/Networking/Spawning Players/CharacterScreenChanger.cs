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
    [SerializeField] private PlayerDataManager playerDataManager;

    public GameObject shooterCharacterScreen, driverCharacterScreen;
    public GameObject choosePlayerTypePanel; //panel for choosing whether to be a driver or shooter 
    public GameObject brakerButton, shredderButton, standardGunButton, goldenGunButton;
    public Color regularButtonColour, selectedButtonColour;
    public GameObject backButton;

    [SerializeField] private PhotonMenuPlayer[] photonMenuPlayers;

    [Header("Loading Screen")]
    [SerializeField] private string[] shooterHints;
    [SerializeField] private string[] driverHints;
    [SerializeField] private CustomMatchmakingRoomController roomController;

    private void Awake()
    {
        //playerDataManager = FindObjectOfType<PlayerDataManager>();
        pv = GetComponent<PhotonView>();
        gameVariables = FindObjectOfType<GameVariables>();

        if (roomController == null)
            roomController = FindObjectOfType<CustomMatchmakingRoomController>();
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
        if(playerDataManager == null)
        {
            playerDataManager = FindObjectOfType<PlayerDataManager>();
        }

        //PlayerType();
        photonMenuPlayers = FindObjectsOfType<PhotonMenuPlayer>();

        if(photonMenuPlayers.Length > 0)
        {
            for(int i = 0; i < photonMenuPlayers.Length; i++)
            {
                //photonMenuPlayers[i].playerNumber = i;
            }
        }
    }


    [PunRPC]
    void RPC_ButtonColour(GameObject button, Color buttonColour)
    {
        button.GetComponent<Image>().color = buttonColour;
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
                pmp.currentCarClass = CarClass.none;
                pmp.currentMinigunClass = MinigunClass.none;
                pmp.driver = false;
                pmp.shooter = false;
                pmp.picked = false;
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

                roomController.hintText.text = driverHints[Random.Range(0, driverHints.Length)];
                //Debug.Log("Tip: " + driverHints[Random.Range(0, driverHints.Length)]);
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
                //playerDataManager.drivers.Add(pmp.gameObject);

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

                //braker
                if(whichCharacter == 0)
                {
                    pv.RPC("RPC_ButtonColour", p, brakerButton, selectedButtonColour);
                    pv.RPC("RPC_ButtonColour", p, shredderButton, regularButtonColour);
                }

                //shredder
                if (whichCharacter == 1)
                {
                    pv.RPC("RPC_ButtonColour", p, shredderButton, selectedButtonColour);
                    pv.RPC("RPC_ButtonColour", p, brakerButton, regularButtonColour);
                }
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
                    //Debug.Log("Braker");
                    pmp.carModel = PhotonMenuPlayer.carType.Braker;
                    pmp.currentCarClass = CarClass.Braker;
                }
                //shredder
                if (whichCharacter == 1)
                {
                    //Debug.Log("Shredder");
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

                roomController.hintText.text = shooterHints[Random.Range(0, shooterHints.Length)];
                //Debug.Log("Tip: " + shooterHints[Random.Range(0, shooterHints.Length)]);
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
                //playerDataManager.shooters.Add(pmp.gameObject);
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

                //standard gun
                if(whichCharacter == 0)
                {
                    pv.RPC("RPC_ButtonColour", p, standardGunButton, selectedButtonColour);
                    pv.RPC("RPC_ButtonColour", p, goldenGunButton, regularButtonColour);
                }

                //golden gun
                if(whichCharacter == 1)
                {
                    pv.RPC("RPC_ButtonColour", p, goldenGunButton, selectedButtonColour);
                    pv.RPC("RPC_ButtonColour", p, standardGunButton, regularButtonColour);
                }
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
                pmp.picked = true;

                //standard gun
                if (whichCharacter == 0)
                {
                    //Debug.Log("standard gun");
                    pmp.shooterModel = PhotonMenuPlayer.shooterType.standardGun;
                    pmp.currentMinigunClass = MinigunClass.standard;
                }

                //golden gun
                if (whichCharacter == 1)
                {
                    //Debug.Log("golden gun");
                    pmp.shooterModel = PhotonMenuPlayer.shooterType.goldenGun;
                    pmp.currentMinigunClass = MinigunClass.gold;
                }
            }
        }
    }
    #endregion
}
