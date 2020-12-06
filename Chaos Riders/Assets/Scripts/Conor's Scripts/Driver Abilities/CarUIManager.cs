using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class CarUIManager : MonoBehaviour
{
    private PhotonView pv;

    [SerializeField] private GameObject youWinTextGo;
    public static GameObject youWinText;

    
    [SerializeField] private TMP_Text lapsTextUI;
    public static TMP_Text lapsText;


    [SerializeField] private GameObject minimapCam;
    public static GameObject minimapCamera;

    void Awake()
    {
        youWinText = youWinTextGo;
        lapsText = lapsTextUI;
        minimapCamera = minimapCam;
        pv = GetComponent<PhotonView>();
    }

    private void Start()
    {
        if (pv.IsMine && IsThisMultiplayer.Instance.multiplayer || !IsThisMultiplayer.Instance.multiplayer)
        {
            minimapCam.SetActive(true); //activate the minimap camera if it belongs to you
        } 
        else
            minimapCam.SetActive(false); //disable all the minimap cameras that dont belong to you
    }

    void Update()
    {
        
    }
}
