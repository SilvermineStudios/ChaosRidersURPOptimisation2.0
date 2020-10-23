using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    public static PlayerInfo pi;

    public int mySelectedCharacter;

    public GameObject[] allCharacters;

    private void OnEnable() //creating a singleton on this gameobject that does not destroy on load
    {
        if (PlayerInfo.pi == null)
        {
            PlayerInfo.pi = this;
        }
        else
        {
            if(PlayerInfo.pi != this)
            {
                Destroy(PlayerInfo.pi.gameObject);
                PlayerInfo.pi = this;
            }
        }
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        if (PlayerPrefs.HasKey("MyCharacter"))
        {
            mySelectedCharacter = PlayerPrefs.GetInt("MyCharacter");
        }
        else
        {
            mySelectedCharacter = 0;
            PlayerPrefs.SetInt("MyCharacter", mySelectedCharacter);
        }
    }
}
