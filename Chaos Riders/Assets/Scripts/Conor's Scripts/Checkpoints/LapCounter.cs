using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LapCounter : MonoBehaviour
{
    [SerializeField] private GameVariables gameVariables;

    public static int AmountOfLaps;
    //[SerializeField] private int amountOfLaps = 3;

    public static TMP_Text lapsText;
    [SerializeField] private TMP_Text lapsTextUI;

    public static GameObject FinishLine;
    [SerializeField] private GameObject finishLine;

    //public static GameObject YouWinText;
    [SerializeField] private GameObject youWinText;

    

    void Awake()
    {
        //YouWinText = youWinText;
        //YouWinText.SetActive(false);

        //AmountOfLaps = amountOfLaps;
        AmountOfLaps = GameVariables.Laps;

        FinishLine = finishLine;
        FinishLine.SetActive(false);

        lapsText = lapsTextUI;

        //lapsTextGo.SetActive(false);
        lapsText.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        /*
        ////////////Get access to the race variables from the Main Menu
        if (gameVariables == null)
            gameVariables = (GameVariables)FindObjectOfType(typeof(GameVariables));
        else
            return;
        */
    }
}
