﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerCarPrefabs : MonoBehaviour
{
    public Transform gunSpawnPoint, gunstand;

    //private PlayerSpawner ps;

    private void OnEnable()
    {
        //ps = FindObjectOfType<PlayerSpawner>();
        //ps.gunSpawnPoints.Add(gunSpawnPoint);
    }
}
