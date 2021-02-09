using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISpawner : MonoBehaviour
{
    //get the amount of ai cars to spawn from Game Variables script *********

    public Transform[] spawnPoints;
    public GameObject[] aiCars;

    //from the main menu settings
    [SerializeField] private bool spawnAI;
    [SerializeField] private int amountOfAI;

    private void Awake()
    {
        ////////////turn these off to test in offline///////////
        spawnAI = GameVariables.ToggleAI;
        amountOfAI = GameVariables.AmountOfAICars;
        //Debug.Log("Spawn AI = " + spawnAI + " Amount of AI = " + amountOfAI);
    }

    void Start()
    {
        if (spawnAI)
        {
            for (int i = 0; i < amountOfAI; i++)
            {
                GameObject aiCar = Instantiate(aiCars[Random.Range(0, aiCars.Length)], spawnPoints[i].position, spawnPoints[i].rotation);
            }
        }
        else
            return;
    }

    void Update()
    {
        
    }
}
