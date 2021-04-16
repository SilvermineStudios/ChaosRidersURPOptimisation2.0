using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPlayerSettings : MonoBehaviour
{
    public PlayerType playerType;
    public int amountOfAI;
    public int amountOfLaps;

    void Start()
    {
        DontDestroyOnLoad(this);
        playerType = PlayerType.None;
    }

    
    void Update()
    {
        
    }
}
