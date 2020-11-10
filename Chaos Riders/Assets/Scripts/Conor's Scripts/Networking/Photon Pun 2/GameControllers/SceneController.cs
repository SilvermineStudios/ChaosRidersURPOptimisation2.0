using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class SceneController : MonoBehaviour
{
    public int mainMenuSceneIndex = 0;

    public void MainMenuButton()
    {
        PhotonNetwork.LoadLevel(mainMenuSceneIndex);
    }
}
