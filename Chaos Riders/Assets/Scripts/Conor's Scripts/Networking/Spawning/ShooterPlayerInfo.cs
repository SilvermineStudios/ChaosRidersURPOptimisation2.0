using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterPlayerInfo : MonoBehaviour
{
    public static ShooterPlayerInfo pi;

    public int mySelectedCharacter;

    public GameObject[] allCharacters;

    private void OnEnable() //creating a singleton on this gameobject that does not destroy on load
    {
        if (ShooterPlayerInfo.pi == null)
        {
            ShooterPlayerInfo.pi = this;
        }
        else
        {
            if (ShooterPlayerInfo.pi != this)
            {
                Destroy(ShooterPlayerInfo.pi.gameObject);
                ShooterPlayerInfo.pi = this;
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
