using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssignTeamNumbers : MonoBehaviour
{
    public void AssignTeamNumber()
    {
        if (PlayerDataManager.Drivers.Count > 0)
        {
            for (int i = 0; i < PlayerDataManager.Drivers.Count; i++)
            {
                PlayerDataManager.Drivers[i].GetComponent<PhotonMenuPlayer>().teamNumber = i;
                Debug.Log(PlayerDataManager.Drivers[i].GetComponent<PhotonMenuPlayer>().Player.NickName + "'s team number is " + PlayerDataManager.Drivers[i].GetComponent<PhotonMenuPlayer>().teamNumber);
            }
        }

        if (PlayerDataManager.Shooters.Count > 0)
        {
            for (int i = 0; i < PlayerDataManager.Shooters.Count; i++)
            {
                PlayerDataManager.Shooters[i].GetComponent<PhotonMenuPlayer>().teamNumber = i;
                Debug.Log(PlayerDataManager.Shooters[i].GetComponent<PhotonMenuPlayer>().Player.NickName + "'s team number is " + PlayerDataManager.Shooters[i].GetComponent<PhotonMenuPlayer>().teamNumber);
            }
        }
    }
}
