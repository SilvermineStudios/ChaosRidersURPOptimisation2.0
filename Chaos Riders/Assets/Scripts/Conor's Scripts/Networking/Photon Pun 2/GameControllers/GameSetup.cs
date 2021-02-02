    using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameSetup : MonoBehaviour
{
    public static GameSetup gs;
    public Transform[] spawnPoints;
    public Player[] players = PhotonNetwork.PlayerList;
    public PhotonPlayer[] photonPlayers;


    //new
    public int nextPlayersTeam;

    private void OnEnable()
    {
        if(GameSetup.gs == null)
        {
            GameSetup.gs = this;
        }
    }
}
