using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLine : MonoBehaviour
{
    public static bool GameWon = false;
    [SerializeField] private bool gameWon = false;

    void Start()
    {
        GameWon = gameWon;
    }

    private void Update()
    {
        gameWon = GameWon;
    }

    private void OnTriggerEnter(Collider other)
    {
        GameWon = true;
    }
}
