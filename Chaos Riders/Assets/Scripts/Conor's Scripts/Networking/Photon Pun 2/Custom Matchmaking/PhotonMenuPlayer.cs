using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PhotonMenuPlayer : MonoBehaviour
{
    private PhotonView pv;
    [SerializeField] private PlayerDataManager playerDataManager;
    [SerializeField] private CustomMatchmakingRoomController roomController;

    //decided by player data manager
    // from 0, decides which team you are in; driver 0 and shooter 0 will be in the same car, driver 1 and shooter 1 will be in the same car.
    public int teamNumber; //<-------------------------------------------------------------------------------------------------------------------------------------------

    public Player Player;
    public int playerNumber; //<-----------------------------------------------------------------------------------------------------------------------------------------


    public bool picked = false;
    public bool driver = false;
    public bool shooter = false;

    public enum carType { Braker, Shredder, Colt, None };
    public carType carModel;
    public CarClass currentCarClass;

    public enum shooterType { standardGun, goldenGun , None};
    public shooterType shooterModel;
    public MinigunClass currentMinigunClass;

    


    //[SerializeField] private GameObject characterTypeSelectionScreen;
    //[SerializeField] private GameObject driverSelectionScreen;
    //[SerializeField] private GameObject shooterSelectionScreen;

    void Awake()
    {
        pv = GetComponent<PhotonView>();
        playerDataManager = FindObjectOfType<PlayerDataManager>();

        carModel = carType.None;
        shooterModel = shooterType.None;
        currentCarClass = CarClass.none;
        currentMinigunClass = MinigunClass.none;

        if (roomController == null)
            roomController = FindObjectOfType<CustomMatchmakingRoomController>();

        //characterTypeSelectionScreen.SetActive(true);
        //driverSelectionScreen.SetActive(false);
        //shooterSelectionScreen.SetActive(false);

        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        Player = pv.Owner;
        //Debug.Log("My player is = " + Player.NickName);
    }

    void Update()
    {
        if(roomController != null)
        {
            foreach (GameObject go in roomController.playerNameBoxes)
            {
                //Change the icon for the players in the playerlist
                if (go.GetComponentInChildren<TMP_Text>().text == this.Player.NickName)
                {
                    //Debug.Log("Player Name Box nickname = " + go.GetComponentInChildren<TMP_Text>().text);
                    //Debug.Log("Player nickname = " + this.Player.NickName);

                    if (carModel == carType.None && shooterModel == shooterType.None)
                        go.GetComponent<PlayerNameBox>().NoneSelected();

                    if (carModel == carType.Braker)
                        go.GetComponent<PlayerNameBox>().BrakerSelected();
                    if (carModel == carType.Shredder)
                        go.GetComponent<PlayerNameBox>().ShredderSelected();

                    if (shooterModel == shooterType.standardGun)
                        go.GetComponent<PlayerNameBox>().StandardGunSelected();
                    if (shooterModel == shooterType.goldenGun)
                        go.GetComponent<PlayerNameBox>().GoldenGunSelected();
                }
            }
        }
    }


    #region Driver Buttons
    //button for choosing to be a driver
    public void DriverButton()
    {
        if(pv.IsMine)
        {
            //go to next selection screen
            //characterTypeSelectionScreen.SetActive(false);
            //shooterSelectionScreen.SetActive(false);
            //driverSelectionScreen.SetActive(true);

            pv.RPC("AddToDrivers", RpcTarget.AllBuffered);
        }
    }
    [PunRPC]
    void AddToDrivers()
    {
        playerDataManager.drivers.Add(this.gameObject);

        //show that the player is a driver
        driver = true;
        shooter = false;
    }


    //pick between driver characters (braker / shredder etc.)
    public void DriverTypeButton(int whichCharacter)
    {
        if(pv.IsMine)
        {
            if (DriverPlayerInfo.pi != null) //check if a playerInfo singleton exists
            {
                DriverPlayerInfo.pi.mySelectedCharacter = whichCharacter;
                PlayerPrefs.SetInt("MyCharacter", whichCharacter);
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
            carModel = carType.Braker;
            currentCarClass = CarClass.Braker;
        }
        //shredder
        if (whichCharacter == 1)
        {
            Debug.Log("Shredder");
            carModel = carType.Shredder;
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
            //characterTypeSelectionScreen.SetActive(false);
            //shooterSelectionScreen.SetActive(true);
            //driverSelectionScreen.SetActive(false);
            
            pv.RPC("AddToShooters", RpcTarget.AllBuffered);
        }
    }
    [PunRPC]
    void AddToShooters()
    {
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
            if (ShooterPlayerInfo.pi != null) //check if a playerInfo singleton exists
            {
                ShooterPlayerInfo.pi.mySelectedCharacter = whichCharacter;
                PlayerPrefs.SetInt("MyCharacter", whichCharacter);
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
            shooterModel = shooterType.standardGun;
            currentMinigunClass = MinigunClass.standard;
        }
        //golden gun
        if (whichCharacter == 1)
        {
            Debug.Log("golden gun");
            shooterModel = shooterType.goldenGun;
            currentMinigunClass = MinigunClass.gold;
        }
    }
    #endregion
}
