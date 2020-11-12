using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerCarPrefabs : MonoBehaviour
{
    public Transform gunSpawnPoint;

    private PlayerSpawner ps;

    private void OnEnable()
    {
        ps = FindObjectOfType<PlayerSpawner>();
        ps.gunSpawnPoints.Add(gunSpawnPoint);
    }
}
