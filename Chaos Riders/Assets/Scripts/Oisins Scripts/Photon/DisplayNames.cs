using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Cinemachine;

public class DisplayNames : MonoBehaviour
{

    [SerializeField] GameObject DriverName;
    [SerializeField] GameObject ShooterName;
    [SerializeField] bool isAI;
    [SerializeField] GameObject canvas;
    TextMeshProUGUI DriverNameText;
    TextMeshProUGUI ShooterNameText;
    PhotonView pv, pvS;
    CinemachineVirtualCamera cineCam;
    PlayerDataManager playerDataManager;
    public string myName { get; private set; } = "";
    public string shooterName { get; private set; } = "";
    bool shooterNameAssigned;
    int myAIShooterName;
    string[] AINames = new string[]
    {
        "[AI]Allen",
        "[AI]Greg",
        "[AI]Steve",
        "[AI]Bob",
        "[AI]Jim",
        "[AI]Dave",
        "[AI]Susan",
        "[AI]Meg",
        "[AI]Sally",
        "[AI]Ann",
        "[AI]Ellen",
        "[AI]Oisin",
        "[AI]Conor",
        "[AI]Will",
        "[AI]Mayowa",
    };

    void Start()
    {
        DriverNameText = DriverName.GetComponent<TextMeshProUGUI>();
        ShooterNameText = ShooterName.GetComponent<TextMeshProUGUI>();
        myAIShooterName = Random.Range(0, 12);
        pv = GetComponent<PhotonView>();

        if(!isAI)
        {
            myName = GetComponent<PhotonView>().Owner.NickName;
        }
        else
        {
            myName = AINames[Random.Range(0, 12)];
            shooterName = AINames[Random.Range(0, 12)];
        }

        if (!isAI)
        {
            cineCam = GetComponent<LinkToCM>().CMcamera;
        }
    }

    void Update()
    {
        if (!shooterNameAssigned)
        {
            if(isAI)
            {
                if (GetComponent<AICarController>().Shooter != null)
                {
                    pvS = GetComponent<AICarController>().Shooter.GetComponent<PhotonView>();
                    shooterName = pvS.Owner.NickName;
                    shooterNameAssigned = true;
                }
            }
            else if (GetComponent<Controller>().Shooter != null)
            {
                pvS = GetComponent<Controller>().Shooter.GetComponent<PhotonView>();
                shooterName = pvS.Owner.NickName;
                shooterNameAssigned = true;
            }
            else
            {
                shooterName = AINames[myAIShooterName];
            }
        }

        DriverNameText.text = myName + " (Driver)";
        ShooterNameText.text = shooterName + " (Shooter)";

        if (FindPlayerCamera.cineCam)
        {
            canvas.transform.LookAt(FindPlayerCamera.cineCam.transform);
            canvas.transform.Rotate(new Vector3(0, 180, 0));
        }

    }

}
