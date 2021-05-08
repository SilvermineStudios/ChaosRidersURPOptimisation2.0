using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;


public class GoToMenu : MonoBehaviourPunCallbacks
{
    [Header("Move to menu scene when play is pressed in inspector")]
    [SerializeField] private int sceneIndex = 0;


    void Awake()
    {
        if(!PhotonNetwork.IsConnected)
        {
            SceneManager.LoadScene(sceneIndex);
            return;
        }
    }

    
    void Update()
    {
        
    }
}
