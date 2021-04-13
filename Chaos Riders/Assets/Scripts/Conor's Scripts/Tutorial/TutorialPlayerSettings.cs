using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPlayerSettings : MonoBehaviour
{
    public PlayerType playerType;

    void Start()
    {
        DontDestroyOnLoad(this);
        playerType = PlayerType.None;
    }

    
    void Update()
    {
        
    }
}
