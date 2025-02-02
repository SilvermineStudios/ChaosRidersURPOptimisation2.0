﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Photon.Pun;

public class Position : IComparable<Position>
{

    public string driverName;
    public string shooterName;
    public string teamName;
    public PhotonView pv, pvS;
    public int checkpointNumber = 0;
    public int lapNumber = 0;
    public float currentPosition;
    public bool finishedRace { get; private set; }

    public Position(string newDriverName, string newShooterName, float newCurrentPosition, PhotonView newPv, PhotonView newPvS)
    {
        driverName = newDriverName;
        shooterName = newShooterName;
        pv = newPv;
        pvS = newPvS;
        teamName = "(Shooter) " + shooterName + "(Driver) " + driverName;
        currentPosition = newCurrentPosition;
    }

    public void UpdateNames(string newDriverName, string newShooterName)
    {
        driverName = newDriverName;
        shooterName = newShooterName;
        teamName = "(Shooter) " + shooterName + "(Driver) " + driverName;
    }

    public void UpdatePosition(float newCurrentPosition, int newCheckpointNumber, int lap)
    {
        currentPosition = newCurrentPosition;
        checkpointNumber = newCheckpointNumber;
        lapNumber = lap;
    }

    public void FinishRace()
    {
        finishedRace = true;
    }

    public int CompareTo(Position other)
    {
        if (other == null)
        {
            return 1;
        }
        if (lapNumber - other.lapNumber == 0)
        {

            if (checkpointNumber - other.checkpointNumber == 0)
            {

                if (currentPosition - other.currentPosition < 0)
                {
                    return 1;
                }
                if (currentPosition - other.currentPosition > 0)
                {
                    return -1;
                }
            }
            if (checkpointNumber - other.checkpointNumber < 0)
            {
                return 1;
            }
            if (checkpointNumber - other.checkpointNumber > 0)
            {
                return -1;
            }
        }
        if (lapNumber - other.lapNumber < 0)
        {
            return 1;
        }
        if (lapNumber - other.lapNumber > 0)
        {
            return -1;
        }

        return 0;
    }
}
