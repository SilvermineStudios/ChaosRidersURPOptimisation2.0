using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MiniMapPlayerColour : MonoBehaviour
{
    private PhotonView pv;
    private SpriteRenderer sr; //use this to change the colour of the player on the map
    private Controller c; //use this to get access to the shooter attached
    private AICarController aic;
 
    private void Awake()
    {
        pv = this.transform.parent.gameObject.GetComponent<PhotonView>();
        sr = GetComponent<SpriteRenderer>();

        if(this.gameObject.tag == "car")
            c = this.transform.parent.gameObject.GetComponent<Controller>();

        if(this.gameObject.tag != "car")
            aic = this.transform.parent.gameObject.GetComponent<AICarController>();
    }

    void Update()
    {
        //for cars
        if(this.gameObject.tag == "car")
        {
            //if theres a shooter attached
            if (c != null && c.Shooter != null)
            {
                if (c.Shooter.GetComponent<PhotonView>().IsMine || pv.IsMine) //make the icon green for this car and for the gun attached to this car
                {
                    sr.color = Color.green;
                }
                else //make every other car in the race red
                {
                    sr.color = Color.red;
                }
            }

            //if there is not a shooter attached
            if (c.Shooter == null)
            {
                if (pv.IsMine) //make the icon for this car green
                {
                    sr.color = Color.green;
                }
                else //make every other car in the race red
                {
                    sr.color = Color.red;
                }
            }
        }

        //for ai cars
        if (this.gameObject.tag == "car")
        {
            //if theres a shooter attached
            if (aic != null && aic.Shooter != null)
            {
                if (aic.Shooter.GetComponent<PhotonView>().IsMine) //make the icon green for this car and for the gun attached to this car
                {
                    sr.color = Color.green;
                }
                else //make every other car in the race red
                {
                    sr.color = Color.red;
                }
            }

            //if there is not a shooter attached
            if (c.Shooter == null)
            {
                sr.color = Color.red;
            }
        }
    }
}
