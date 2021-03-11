using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhotonMenuCopy : MonoBehaviour
{
    private int spawnNumberValue; //the index used for spawning this player (0 = at spawn 1, 1 = at spawn 2 etc.)

    private PhotonView pv;
    private GameVariables gameVariables;
    private PlayerDataManager playerDataManager;

    //decided by player data manager
    public int teamNumber; // from 0, decides which team you are in; driver 0 and shooter 0 will be in the same car, driver 1 and shooter 1 will be in the same car.

    public Player Player;
    public int playerNumber;

    public bool driver = false;
    public bool shooter = false;

    public enum carType { Braker, Shredder, Colt, None };
    public carType carModel;

    public enum shooterType { standardGun, goldenGun, None };
    public shooterType shooterModel;

    public MinigunClass currentMinigunClass;
    public CarClass currentCarClass;

    [SerializeField] private GameObject characterTypeSelectionScreen;
    [SerializeField] private GameObject driverSelectionScreen;
    [SerializeField] private GameObject shooterSelectionScreen;


    [SerializeField] private Button BackButton;

    void Awake()
    {
        pv = GetComponent<PhotonView>();
        gameVariables = FindObjectOfType<GameVariables>();
        playerDataManager = FindObjectOfType<PlayerDataManager>();
        BackButton = GameObject.FindGameObjectWithTag("Back").GetComponent<Button>();
        carModel = carType.None;
        shooterModel = shooterType.None;
    }

    private void Start()
    {
        if (pv.IsMine)
        {
            characterTypeSelectionScreen.SetActive(true);
            shooterSelectionScreen.SetActive(false);
            driverSelectionScreen.SetActive(false);
        }
        else //make all other players selection screens invisible
        {
            characterTypeSelectionScreen.SetActive(false);
            shooterSelectionScreen.SetActive(false);
            driverSelectionScreen.SetActive(false);
        }

        Player = pv.Owner;
    }

    void Update()
    {
        //Debug.Log("Player = " + Player);

        if (!characterTypeSelectionScreen.activeInHierarchy)
        {
            BackButton.interactable = true;
        }
        else
        {
            BackButton.interactable = false;
        }
    }

    void GetPlayeNumber()
    {
        foreach (Player p in PhotonNetwork.PlayerList)
        {
            if (p == PhotonNetwork.LocalPlayer)
            {

            }
        }
    }



    #region Driver Buttons
    //button for choosing to be a driver
    public void DriverButton()
    {
        if (pv.IsMine)
        {
            //go to next selection screen
            characterTypeSelectionScreen.SetActive(false);
            shooterSelectionScreen.SetActive(false);
            driverSelectionScreen.SetActive(true);
        }
    }
    [PunRPC]
    void AddToDrivers()
    {
        //increase the global amount of drivers
        gameVariables.amountOfDrivers++;
        playerDataManager.drivers.Add(this.gameObject);

        //show that the player is a driver
        driver = true;
        shooter = false;
        Debug.Log("Driver");
    }


    //pick between driver characters (braker / shredder etc.)
    public void DriverTypeButton(int whichCharacter)
    {
        if (pv.IsMine)
        {
            if (!driver)
            {
                pv.RPC("AddToDrivers", RpcTarget.AllBuffered);
            }
            if (DriverPlayerInfo.pi != null) //check if a playerInfo singleton exists
            {
                DriverPlayerInfo.pi.mySelectedCharacter = whichCharacter;
                PlayerPrefs.SetInt("MyCharacter", whichCharacter);
            }
            if (PhotonNetwork.IsMasterClient)
            {
                FindObjectOfType<CustomMatchmakingRoomController>().startButton.GetComponent<Button>().interactable = true;
            }
            pv.RPC("AssignDriverCharacter", RpcTarget.AllBuffered, whichCharacter);
        }
    }
    [PunRPC]
    void AssignDriverCharacter(int whichCharacter)
    {
        //braker
        if (whichCharacter == 0)
        {
            Debug.Log("Braker");
            currentCarClass = CarClass.Braker;
        }
        //shredder
        if (whichCharacter == 1)
        {
            Debug.Log("Shredder");
            currentCarClass = CarClass.Shredder;
        }
    }
    #endregion





    #region Shooter Buttons
    //button for choosing to be a shooter
    public void ShooterButton()
    {
        if (pv.IsMine)
        {
            //go to next selection screen
            characterTypeSelectionScreen.SetActive(false);
            shooterSelectionScreen.SetActive(true);
            driverSelectionScreen.SetActive(false);


        }
    }
    [PunRPC]
    void AddToShooters()
    {
        //increase the global amount of shooters
        gameVariables.amountOfShooters++;
        playerDataManager.shooters.Add(this.gameObject);

        //show that the player is a shooter
        shooter = true;
        driver = false;
    }


    //pick between shooter characters (Dreagen Max / Celia Lock etc.)
    public void ShooterTypeButton(int whichCharacter)
    {
        if (pv.IsMine)
        {
            if (!shooter)
            {
                pv.RPC("AddToShooters", RpcTarget.AllBuffered);
            }
            if (ShooterPlayerInfo.pi != null) //check if a playerInfo singleton exists
            {
                ShooterPlayerInfo.pi.mySelectedCharacter = whichCharacter;
                PlayerPrefs.SetInt("MyCharacter", whichCharacter);
            }
            if (PhotonNetwork.IsMasterClient)
            {
                FindObjectOfType<CustomMatchmakingRoomController>().startButton.GetComponent<Button>().interactable = true;
            }
            pv.RPC("AssignShooterCharacter", RpcTarget.AllBuffered, whichCharacter);
        }
    }
    [PunRPC]
    void AssignShooterCharacter(int whichCharacter)
    {
        //standard gun
        if (whichCharacter == 0)
        {
            Debug.Log("standard gun");
            currentMinigunClass = MinigunClass.standard;
        }
        //golden gun
        if (whichCharacter == 1)
        {
            Debug.Log("golden gun");
            currentMinigunClass = MinigunClass.gold;
        }
    }
    #endregion


    public void BackButtonPress()
    {
        characterTypeSelectionScreen.SetActive(true);
        shooterSelectionScreen.SetActive(false);
        driverSelectionScreen.SetActive(false);
    }


    public void Starting()
    {
        characterTypeSelectionScreen.SetActive(false);
        shooterSelectionScreen.SetActive(false);
        driverSelectionScreen.SetActive(false);
    }
}
