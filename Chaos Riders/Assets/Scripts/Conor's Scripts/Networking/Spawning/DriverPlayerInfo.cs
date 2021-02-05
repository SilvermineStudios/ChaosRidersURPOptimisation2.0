using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriverPlayerInfo : MonoBehaviour
{
    public static DriverPlayerInfo pi;

    public int mySelectedCharacter;

    public GameObject[] allCharacters;

    private void OnEnable() //creating a singleton on this gameobject that does not destroy on load
    {
        if (DriverPlayerInfo.pi == null)
        {
            DriverPlayerInfo.pi = this;
        }
        else
        {
            if (DriverPlayerInfo.pi != this)
            {
                Destroy(DriverPlayerInfo.pi.gameObject);
                DriverPlayerInfo.pi = this;
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
