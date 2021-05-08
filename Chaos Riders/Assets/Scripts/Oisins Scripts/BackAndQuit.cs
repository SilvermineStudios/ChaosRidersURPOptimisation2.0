using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class BackAndQuit : MonoBehaviourPun
{

    public void LeaveRaceButton()
    {
        StartCoroutine(DisconnectAndLoad());
    }
    IEnumerator DisconnectAndLoad()
    {
        PhotonNetwork.LeaveRoom();

        while (PhotonNetwork.InRoom)
            yield return null;

        SceneManager.LoadScene(0);
    }

    public void QuitGameButton()
    {
        StartCoroutine(Disconnect());
    }
    IEnumerator Disconnect()
    {
        PhotonNetwork.LeaveRoom();

        while (PhotonNetwork.InRoom)
            yield return null;

        Application.Quit();
    }

}
