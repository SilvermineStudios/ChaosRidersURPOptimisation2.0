using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonMenuPlayer : MonoBehaviour
{
    private PhotonView pv;
    private GameVariables gameVariables;

    public bool driver = false;
    public bool shooter = false;

    public enum carType { Braker, Shredder, Colt, None };
    public carType carModel;

    public enum shooterType { standardGun, goldenGun , None};
    public shooterType shooterModel;

    [SerializeField] private GameObject characterTypeSelectionScreen;
    [SerializeField] private GameObject driverSelectionScreen;
    [SerializeField] private GameObject shooterSelectionScreen;

    void Awake()
    {
        pv = GetComponent<PhotonView>();
        gameVariables = FindObjectOfType<GameVariables>();

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
    }

    void Update()
    {
        
    }

    #region Driver Buttons
    //button for choosing to be a driver
    public void DriverButton()
    {
        if(pv.IsMine)
        {
            //go to next selection screen
            characterTypeSelectionScreen.SetActive(false);
            shooterSelectionScreen.SetActive(false);
            driverSelectionScreen.SetActive(true);

            pv.RPC("AddToDrivers", RpcTarget.AllBuffered);
        }
    }
    [PunRPC]
    void AddToDrivers()
    {
        //increase the global amount of drivers
        gameVariables.amountOfDrivers++;

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
        }
        //shredder
        if (whichCharacter == 1)
        {
            Debug.Log("Shredder");
            carModel = carType.Shredder;
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

            pv.RPC("AddToShooters", RpcTarget.AllBuffered);
        }
    }
    [PunRPC]
    void AddToShooters()
    {
        //increase the global amount of shooters
        gameVariables.amountOfShooters++;

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
        }
        //golden gun
        if (whichCharacter == 1)
        {
            Debug.Log("golden gun");
            shooterModel = shooterType.goldenGun;
        }
    }
    #endregion
}
