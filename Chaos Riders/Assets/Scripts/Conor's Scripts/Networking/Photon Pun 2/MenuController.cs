using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public void OnClickCharacterPick(int whichCharacter)
    {
        if (PlayerInfo.pi != null) //check if a playerInfo singleton exists
        {
            PlayerInfo.pi.mySelectedCharacter = whichCharacter;
            PlayerPrefs.SetInt("MyCharacter", whichCharacter);
        }
    }
}
