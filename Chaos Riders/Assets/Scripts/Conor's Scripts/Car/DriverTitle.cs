using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriverTitle : MonoBehaviour
{
    public enum carType { Braker, Shredder, Colt };
    public carType carModel = carType.Braker;
    public int carIndex = -1;


    private void Awake()
    {
        AssaignCarIndex();
    }

    private void OnDrawGizmos()
    {
        AssaignCarIndex();
    }

    void AssaignCarIndex()
    {
        if (carModel == carType.Braker)
            carIndex = 0;
        if (carModel == carType.Shredder)
            carIndex = 1;
        if (carModel == carType.Colt)
            carIndex = 2;
    }
}
