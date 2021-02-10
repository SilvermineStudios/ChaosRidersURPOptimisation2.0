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

    private void OnDrawGizmos()
    {
        spawnPoints = new Transform[this.transform.childCount]; //make the array the same length as the amount of children waypoints
        for (int i = 0; i < this.transform.childCount; i++) //put every waypoint(Child) in the array
        {
            spawnPoints[i] = transform.GetChild(i);
        }
    }

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
