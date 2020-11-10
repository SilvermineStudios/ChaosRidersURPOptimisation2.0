using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomMatchmakingStartup : MonoBehaviour
{
    [SerializeField] private GameObject menuPanel; 
    [SerializeField] private GameObject lobbyPanel; 
    [SerializeField] private GameObject roomPanel; 
    [SerializeField] private GameObject joinLobbyButton; 

    void Awake()
    {
        menuPanel.SetActive(true);
        lobbyPanel.SetActive(false);
        roomPanel.SetActive(false);
        joinLobbyButton.SetActive(false);
    }
}
