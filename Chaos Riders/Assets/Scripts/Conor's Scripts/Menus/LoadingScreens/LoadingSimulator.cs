using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class LoadingSimulator : MonoBehaviour
{
    private PhotonView pv;

    private float loadingTime;
    [SerializeField] private Image loadingCircle; //increase the fill amount to complete
    [SerializeField] private Image loadingBar; //increase the fill amount to complete
    [SerializeField] private TextMeshProUGUI loadingPercentageText;
    private float loadAmount = 0;
    private float loadingValueNormalized;

    private void Awake()
    {
        pv = GetComponent<PhotonView>();

        loadingTime = CustomMatchmakingRoomController.LoadingTime;

        loadingCircle.fillAmount = 0;
        loadingBar.fillAmount = 0;
        loadingPercentageText.text = "0";
    }

    void Update()
    {
        //counter for the loading bars
        if (loadAmount < loadingTime)
        {
            loadAmount += 1 * Time.deltaTime;
            loadingValueNormalized = (loadAmount / loadingTime);
        }

        //fill the loading bars with the normalized loading value
        loadingCircle.fillAmount = loadingValueNormalized;
        loadingBar.fillAmount = loadingValueNormalized;

        //update load percentage
        loadingPercentageText.text = (loadingValueNormalized * 100).ToString("f0"); //calculate the percentage loaded and display it without decimals
    }
}
