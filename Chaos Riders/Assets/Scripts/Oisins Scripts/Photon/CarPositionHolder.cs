using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CarPositionHolder : MonoBehaviour
{
    public Position myPosition { get; private set; }
    DisplayNames dN;
    PhotonView pv;
    Checkpoint checkpoint;
    AICarController aICarController;
    [SerializeField] TrackNearbyWaypoints TNW;
    [SerializeField] bool isAI;

    void Start()
    {
        if (isAI)
        {
            aICarController = GetComponent<AICarController>();
        }
        else
        {
            checkpoint = GetComponent<Checkpoint>();
        }

        pv = GetComponent<PhotonView>();
        dN = GetComponent<DisplayNames>();
        myPosition = new Position(dN.myName, dN.shooterName, 0, pv);
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
            if(isAI)
            {
                myPosition.UpdatePosition(TNW.distToNearest, TNW.nearestNum, aICarController.currentLap);
            }
            else
            {
                myPosition.UpdatePosition(TNW.distToNearest, TNW.nearestNum, checkpoint.currentLap);
            }
            
        }
    }
}
