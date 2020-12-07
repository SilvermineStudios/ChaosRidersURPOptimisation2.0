using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class ShooterUIManager : MonoBehaviour
{
    private PhotonView pv;
    public GameObject gunnerCanvas; //Driver UI canvas

    [SerializeField] private GameObject youWinTextGo;
    public static GameObject youWinText;


    [SerializeField] private TMP_Text lapsTextUI;
    public static TMP_Text lapsText;


    [SerializeField] private GameObject minimapCam;
    public static GameObject minimapCamera;


    private void Awake()
    {
        pv = GetComponent<PhotonView>();
        youWinText = youWinTextGo;
        lapsText = lapsTextUI;
        minimapCamera = minimapCam;
    }

    void Start()
    {
        if (pv.IsMine && IsThisMultiplayer.Instance.multiplayer || !IsThisMultiplayer.Instance.multiplayer)
        {
            gunnerCanvas.SetActive(true); //activate the gunners UI canvas
            minimapCam.SetActive(true);
        }
        else if (!pv.IsMine && IsThisMultiplayer.Instance.multiplayer)
        {
            gunnerCanvas.SetActive(false); //deactivate all the UI canvas' that dont belong to you
            minimapCam.SetActive(false);
        }
            
    }
}
