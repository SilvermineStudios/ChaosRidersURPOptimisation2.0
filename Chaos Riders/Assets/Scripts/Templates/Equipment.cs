using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newEquipmentData", menuName = "Data/Equipment Data")]

public class Equipment : ScriptableObject
{
    [Header("GameObjects")]
    public GameObject prefab;
    public string photonPrefab;

    public float equipmentUseTime;
    public float chargeRate;
    public string sound;

}
