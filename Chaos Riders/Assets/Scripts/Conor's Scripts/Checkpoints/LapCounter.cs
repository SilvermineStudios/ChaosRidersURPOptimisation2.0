using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LapCounter : MonoBehaviour
{
    public static int AmountOfLaps;
    [SerializeField] private int amountOfLaps = 3;

    public static TMP_Text lapsText;
    [SerializeField] private TMP_Text lapsTextUI;

    public static GameObject FinishLine;
    [SerializeField] private GameObject finishLine;

    public static GameObject YouWinText;
    [SerializeField] private GameObject youWinText;

    

    void Awake()
    {
        YouWinText = youWinText;
        YouWinText.SetActive(false);

        AmountOfLaps = amountOfLaps;

        FinishLine = finishLine;
        FinishLine.SetActive(false);

        lapsText = lapsTextUI;

        //lapsTextGo.SetActive(false);
        lapsText.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
