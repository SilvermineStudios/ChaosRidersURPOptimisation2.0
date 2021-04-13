using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPlayerSettings : MonoBehaviour
{
    public enum PlayerType
    {
        none,
        Braker,
        Shredder,
        standardShooter,
        goldenGun
    };

    public PlayerType playerType;

    void Start()
    {
        DontDestroyOnLoad(this);
    }

    
    void Update()
    {
        
    }
}
