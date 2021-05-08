using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOnAtStart : MonoBehaviour
{
    [Header ("Turns on all the objects in the array at start")]
    public GameObject[] objectsToTurnOn;


    void Awake()
    {
        //if there array is empty dont do anything
        if (objectsToTurnOn.Length <= 0)
        {
            Debug.Log("No Objects to turn on!");
            return;
        }
            
        //turn on all the gameobjects in the array
        foreach(GameObject go in objectsToTurnOn)
        {
            go.SetActive(true);
        }
    }
}
