using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CarPositionHolder : MonoBehaviour
{
    public Position myPosition { get; private set; }
    DisplayNames dN;
    PhotonView pv;
    [SerializeField] TrackNearbyWaypoints TNW;

    void Start()
    {
        pv = GetComponent<PhotonView>();
        dN = GetComponent<DisplayNames>();
        myPosition = new Position(dN.myName, dN.shooterName, 0);
    }


    void FixedUpdate()
    {
        if(pv.IsMine)
        {
            //in case Shooter spawns in late
            if(dN.shooterName != myPosition.shooterName || dN.myName != myPosition.driverName)
            {
                myPosition.UpdateNames(dN.myName, dN.shooterName);
            }


            //Calculate Position
            myPosition.UpdatePosition(TNW.distToNearest, TNW.nearestNum);
        }
    }
}
