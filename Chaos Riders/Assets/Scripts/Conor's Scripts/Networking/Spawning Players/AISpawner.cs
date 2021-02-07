using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISpawner : MonoBehaviour
{
    //get the amount of ai cars to spawn from Game Variables script


    public Transform[] spawnPoints;

    [SerializeField] private GameVariables gameVariables;
    [SerializeField] private bool spawnAI;
    [SerializeField] private int amountOfAI;

    private void Awake()
    {
        gameVariables = FindObjectOfType<GameVariables>();

        spawnAI = GameVariables.ToggleAI;
        amountOfAI = GameVariables.AmountOfAICars;

        Debug.Log("Spawn AI = " + spawnAI + " Amount of AI = " + amountOfAI);
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
