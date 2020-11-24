using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbiliyCooldowns : MonoBehaviour
{
    public Transform grenadeCooldownBar;
    public Transform nitroGuzzlerCooldownBar;

    [SerializeField] private float currentAmount;
    [SerializeField] private float speed;


    void Update()
    {
        if(currentAmount < 100)
        {
            currentAmount += speed * Time.deltaTime;
        }

        grenadeCooldownBar.GetComponent<Image>().fillAmount = currentAmount / 100;
        nitroGuzzlerCooldownBar.GetComponent<Image>().fillAmount = currentAmount / 100;
    }
}
