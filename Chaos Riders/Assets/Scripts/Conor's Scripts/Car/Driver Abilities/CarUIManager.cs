using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class CarUIManager : MonoBehaviour
{
    private PhotonView pv;
    public GameObject driverCanvas; //Driver UI canvas

    [SerializeField] private GameObject youWinTextGo;
    public static GameObject youWinText;

    
    [SerializeField] private TMP_Text lapsTextUI;
    public static TMP_Text lapsText;


    private GameObject minimapCam;
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
            driverCanvas.SetActive(true);
        } 
        else if (!pv.IsMine && IsThisMultiplayer.Instance.multiplayer)
        {
            driverCanvas.SetActive(false);
        }
    }

    void Update()
    {
        if(minimapCam == null)
            minimapCam = FindObjectOfType<MiniMap>().gameObject;
    }
}
