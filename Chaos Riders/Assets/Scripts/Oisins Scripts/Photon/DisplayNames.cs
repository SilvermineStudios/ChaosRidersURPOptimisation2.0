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
    string myName = "";
    string shooterName = "";
    bool shooterNameAssigned;

    void Start()
    {
        DriverNameText = DriverName.GetComponent<TextMeshProUGUI>();
        ShooterNameText = ShooterName.GetComponent<TextMeshProUGUI>();

        pv = GetComponent<PhotonView>();

        if(!isAI)
        {
            myName = GetComponent<PhotonView>().Owner.NickName;
        }
        else
        {
            myName = "AI Dave";
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
                else
                {
                    shooterName = "AI Jim";
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
                shooterName = "AI Jim";
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
