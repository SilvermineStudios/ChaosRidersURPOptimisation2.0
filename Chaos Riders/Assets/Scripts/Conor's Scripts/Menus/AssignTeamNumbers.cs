using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun.UtilityScripts;
public class AssignTeamNumbers : MonoBehaviour
{
    public void AssignTeamNumber()
    {
        if (PlayerDataManager.Drivers.Count > 0)
        {
            for (int i = 1; i < PlayerDataManager.Drivers.Count; i++)
            {
                PlayerDataManager.Drivers[i].GetComponent<PhotonMenuPlayer>().teamNumber = i;
                PlayerDataManager.Drivers[i].GetComponent<PhotonMenuPlayer>().Player.JoinTeam(1);
                Debug.Log(PlayerDataManager.Drivers[i].GetComponent<PhotonMenuPlayer>().Player.JoinTeam(1));
                
                //Debug.Log(PlayerDataManager.Drivers[i].GetComponent<PhotonMenuPlayer>().Player.NickName + "'s team number is " + PlayerDataManager.Drivers[i].GetComponent<PhotonMenuPlayer>().teamNumber);
            }
        }

        if (PlayerDataManager.Shooters.Count > 0)
        {
            for (int i = 1; i < PlayerDataManager.Shooters.Count; i++)
            {
                PlayerDataManager.Shooters[i].GetComponent<PhotonMenuPlayer>().teamNumber = i;
                PlayerDataManager.Drivers[i].GetComponent<PhotonMenuPlayer>().Player.JoinTeam((byte)i);
                //Debug.Log(PlayerDataManager.Shooters[i].GetComponent<PhotonMenuPlayer>().Player.NickName + "'s team number is " + PlayerDataManager.Shooters[i].GetComponent<PhotonMenuPlayer>().teamNumber);
            }
        }
    }
}
