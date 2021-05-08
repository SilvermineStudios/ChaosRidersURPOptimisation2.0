using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;
using ExitGames.Client.Photon;
public class CarPositionHolder : MonoBehaviourPun
{
    public Position myPosition { get; private set; }
    DisplayNames dN;
    PhotonView pv;
    Checkpoint checkpoint;
    AICarController aICarController;
    [SerializeField] TrackNearbyWaypoints TNW;
    [SerializeField] bool isAI;
    bool aiFinished = false;
    bool sent;
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
        myPosition = new Position(dN.myName, dN.shooterName, 0, pv, dN.pvS);
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

            if(checkpoint != null && checkpoint.youFinishedTheRace)
            {
                myPosition.FinishRace();
            }
            
            if(!sent && aiFinished)
            {
                //myPosition.FinishRace();
                sent = true;
                object[] content = new object[] {myPosition.driverName,myPosition.shooterName};
                RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All }; // You would have to set the Receivers to All in order to receive this event on the local client as well
                PhotonNetwork.RaiseEvent(1, content, raiseEventOptions, SendOptions.SendReliable);
                Debug.Log("Sent");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FinishLine"))
        {
            aiFinished = true;
        }
    }
}
