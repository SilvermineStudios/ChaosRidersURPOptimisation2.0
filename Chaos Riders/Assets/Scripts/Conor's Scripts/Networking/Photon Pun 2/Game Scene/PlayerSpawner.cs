using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.IO;
using UnityEngine.UI;

public class PlayerSpawner : MonoBehaviourPunCallbacks
{
    [SerializeField]private float spawnTimer = 8.5f; //have this variable match the length of time the camera animation is

    [SerializeField] public static GameObject car,shooter;

    //Bool to flip if spawn car or shooter
    private bool driver = false;
    private float spawnCounter;

    private void Awake()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(Timer(spawnTimer));
        }
    }

    private IEnumerator Timer(float time)
    {
        yield return new WaitForSeconds(time);

        StartGame();
    }

    public void StartGame()
    {
        spawnCounter = 0;

        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            //Original
            //photonView.RPC("RPC_StartGame", PhotonNetwork.PlayerList[i], GameSetup.gs.spawnPoints[i].position, GameSetup.gs.spawnPoints[i].rotation);



            //Added code - Oisin

            if(driver)
            {
                photonView.RPC("RPC_StartGame", PhotonNetwork.PlayerList[i], GameSetup.gs.spawnPoints[i].position, GameSetup.gs.spawnPoints[i].rotation);
                driver = !driver;
                spawnCounter++;
            }
            else
            {
                //after the cars are spawned make a transform array of the shooter spawn positions
                //change -> GameSetup.gs.spawnPoints[i].position, GameSetup.gs.spawnPoints[i].rotation to 
                photonView.RPC("RPC_StartGameShooter", PhotonNetwork.PlayerList[i], car.transform.position, car.transform.rotation);
                driver = !driver;
            }


        }







    }

    [PunRPC]
    void RPC_StartGameShooter(Vector3 spawnPos, Quaternion spawnRot)
    {
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Shooter"), spawnPos, spawnRot, 0);

    }


    [PunRPC]
    void RPC_StartGame(Vector3 spawnPos, Quaternion spawnRot)
    {
        car = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "CarAvatar"), spawnPos, spawnRot, 0);
        car = car.transform.GetChild(0).gameObject;
    }



}
