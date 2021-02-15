using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTypeManager : MonoBehaviour
{
    [SerializeField] private float groupingDriverDelay = 0.5f, groupingShooterDelay = 0.5f;

    [SerializeField] private PhotonDriver[] alldrivers;
    [SerializeField] private PhotonShooter[] allshooters;


    void Start()
    {
        StartCoroutine(GroupDrivers(groupingDriverDelay));
        StartCoroutine(GroupShooters(groupingShooterDelay));
    }

    

    #region drivers
    private IEnumerator GroupDrivers(float time)
    {
        yield return new WaitForSeconds(time);

        alldrivers = (PhotonDriver[])Object.FindObjectsOfType(typeof(PhotonDriver));

        AssignDriverNumber();
    }

    void AssignDriverNumber()
    {
        for (int i = 0; i < alldrivers.Length; i++)
        {
            alldrivers[i].myDriverNumber = i;
        }
    }
    #endregion

    #region shooters
    private IEnumerator GroupShooters(float time)
    {
        yield return new WaitForSeconds(time);

        allshooters = (PhotonShooter[])Object.FindObjectsOfType(typeof(PhotonShooter));

        AssignShooterNumber();
    }

    void AssignShooterNumber()
    {
        for (int i = 0; i < allshooters.Length; i++)
        {
            allshooters[i].myShooterNumber = i;
        }
    }
    #endregion
}
