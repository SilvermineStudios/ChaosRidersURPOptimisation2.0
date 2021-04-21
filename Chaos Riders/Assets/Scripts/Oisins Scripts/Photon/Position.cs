using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Position : IComparable<Position>
{

    public string teamName;
    public float currentPosition;

    public Position(string newName, float newCurrentPosition)
    {
        teamName = newName;
        currentPosition = newCurrentPosition;
    }


    public int CompareTo(Position other)
    {
        if(other == null)
        {
            return 1;
        }
        else if (currentPosition - other.currentPosition > 0)
        {
            return 1;
        }
        else if (currentPosition - other.currentPosition < 0)
        {
            return -1;
        }
        else
        {
            return 0;
        }

    }
}
