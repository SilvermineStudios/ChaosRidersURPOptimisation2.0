using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CarUIManager : MonoBehaviour
{
    [SerializeField] private GameObject youWinTextGo;
    public static GameObject youWinText;

    
    [SerializeField] private TMP_Text lapsTextUI;
    public static TMP_Text lapsText;

    void Awake()
    {
        youWinText = youWinTextGo;
        lapsText = lapsTextUI;
    }

    
    void Update()
    {
        
    }
}
