using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Position : IComparable<Position>
{

    public string driverName;
    public string shooterName;
    public string teamName;

    public int checkpointNumber = 0;
    public float currentPosition;

    public Position(string newDriverName, string newShooterName, float newCurrentPosition)
    {
        driverName = newDriverName;
        shooterName = newShooterName;
        teamName = driverName + " and " + shooterName;
        currentPosition = newCurrentPosition;
    }

    public void UpdateNames(string newDriverName, string newShooterName)
    {
        driverName = newDriverName;
        shooterName = newShooterName;
        teamName = driverName + " and " + shooterName;
    }

    public void UpdatePosition(float newCurrentPosition, int newCheckpointNumber)
    {
        currentPosition = newCurrentPosition;
        checkpointNumber = newCheckpointNumber;
    }


    public int CompareTo(Position other)
    {
        if (other == null)
        {
            return 1;
        }
        if (checkpointNumber - other.checkpointNumber == 0)
        {
            if (currentPosition - other.currentPosition > 0)
            {
                return -1;
            }
            if (currentPosition - other.currentPosition < 0)
            {
                return 1;
            }
        }
        if (checkpointNumber - other.checkpointNumber > 0)
        {
            return -1;
        }
        if (checkpointNumber - other.checkpointNumber > 0)
        {
            return 1;
        }

        return 0;
    }
}
