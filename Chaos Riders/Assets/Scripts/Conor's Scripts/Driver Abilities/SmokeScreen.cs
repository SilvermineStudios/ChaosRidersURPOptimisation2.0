using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeScreen : MonoBehaviour
{
    public GameObject smokeGameObject;
    public GameObject smokeSpawn;


    void Start()
    {

    }

    
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && AbiliyCooldowns.canUseSmoke)
        {
            Instantiate(smokeGameObject, smokeSpawn.transform.position, smokeSpawn.transform.rotation);
            AbiliyCooldowns.resetEquipment = true;
            AbiliyCooldowns.equipmentCharge.fillAmount = 0;
        }
    }
}
