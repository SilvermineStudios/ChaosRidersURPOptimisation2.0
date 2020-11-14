using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LapCounter : MonoBehaviour
{
    public static TMP_Text lapsText;
    [SerializeField] private TMP_Text lapsTextUI;

    

    void Awake()
    {
        lapsText = lapsTextUI;

        //lapsTextGo.SetActive(false);
        lapsText.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
