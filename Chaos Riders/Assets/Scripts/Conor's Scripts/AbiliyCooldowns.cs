using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbiliyCooldowns : MonoBehaviour
{
    public Transform GrenadeCooldownBar;

    [SerializeField] private float currentAmount;
    [SerializeField] private float speed;


    void Update()
    {
        if(currentAmount < 100)
        {
            currentAmount += speed * Time.deltaTime;
        }
    }
}
