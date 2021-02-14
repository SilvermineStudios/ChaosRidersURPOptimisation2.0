using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.IO;

public class PhotonDriver : MonoBehaviour
{
    public PhotonView pv;
    public int characterValue;
    public GameObject myCharacter;
    private bool characterAdded = false;

    Player[] allPlayers;
    [SerializeField] private int myNumberInRoom;
    public int myDriverNumber;
    [SerializeField] private float spawnDelay = 2f;


    private void Awake()
    {
        pv = GetComponent<PhotonView>();
        

        if (pv.IsMine) //make sure it only calls it for the person it belongs to
        {
            pv.RPC("RPC_AddCharacter", PhotonNetwork.LocalPlayer, DriverPlayerInfo.pi.mySelectedCharacter);
            AssignPlayerNumber();
        }
    }

    //calculate players number in the server
    private void AssignPlayerNumber()
    {
        allPlayers = PhotonNetwork.PlayerList;
        foreach (Player p in allPlayers)
        {
            if (p != PhotonNetwork.LocalPlayer)
            {
                myNumberInRoom++;
            }
        }
    }

    [PunRPC]
    void RPC_AddCharacter(int whichCharacter)
    {
        characterValue = whichCharacter;
        //myCharacter = Instantiate(DriverPlayerInfo.pi.allCharacters[whichCharacter], transform.position, transform.rotation, transform);

        myCharacter = DriverPlayerInfo.pi.allCharacters[whichCharacter];
        //PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", myCharacter.name), transform.position, transform.rotation, 0);
        //PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", myCharacter.name), GameSetup.SpawnPoints[myDriverNumber].position, GameSetup.SpawnPoints[myDriverNumber].rotation, 0);
        characterAdded = true;
        StartCoroutine(SpawnDelay(spawnDelay));
    }

    private IEnumerator SpawnDelay(float time)
    {
        yield return new WaitForSeconds(time);

        //pv.RPC("RPC_SpawnMyCharacter", PhotonNetwork.PlayerList[myNumberInRoom], GameSetup.SpawnPoints[myDriverNumber].position, GameSetup.SpawnPoints[myDriverNumber].rotation);
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", myCharacter.name), GameSetup.SpawnPoints[myDriverNumber].position, GameSetup.SpawnPoints[myDriverNumber].rotation, 0);
    }
}
