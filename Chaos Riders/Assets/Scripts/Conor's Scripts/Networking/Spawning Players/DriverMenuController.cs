using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriverMenuController : MonoBehaviour
{
    public void OnClickCharacterPick(int whichCharacter)
    {
        if (DriverPlayerInfo.pi != null) //check if a playerInfo singleton exists
        {
            DriverPlayerInfo.pi.mySelectedCharacter = whichCharacter;
            PlayerPrefs.SetInt("MyCharacter", whichCharacter);
        }
    }
}
