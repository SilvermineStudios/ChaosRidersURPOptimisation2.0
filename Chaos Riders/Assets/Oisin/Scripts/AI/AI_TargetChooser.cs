using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_TargetChooser : MonoBehaviour
{
    public List <GameObject> nearbyPlayers = new List<GameObject>();

    bool chosenTarget;

    AI_Aiming aiScript;

    void Start()
    {
        aiScript = GetComponent<AI_Aiming>();
    }

    void Update()
    {
        if(!chosenTarget && nearbyPlayers.Count >= 1)
        {
            aiScript.targetTransform = nearbyPlayers[Random.Range(0, nearbyPlayers.Count-1)].transform;
            chosenTarget = true;
        }
        

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            nearbyPlayers.Add(other.gameObject);
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if(aiScript.targetTransform == other.gameObject.transform)
            {
                chosenTarget = false;
                aiScript.targetTransform = null;
            }
            nearbyPlayers.Remove(other.gameObject);

        }
    }
}
