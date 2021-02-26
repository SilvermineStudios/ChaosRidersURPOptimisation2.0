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

        if(this.transform.parent.tag == "car")
            c = this.transform.parent.gameObject.GetComponent<Controller>();

        if(this.transform.parent.tag != "car")
            aic = this.transform.parent.gameObject.GetComponent<AICarController>();
    }

    void Update()
    {
        //driver player
        if(this.transform.parent.tag == "car")
        {
            if (pv.IsMine || c != null && c.Shooter != null && c.Shooter.GetComponent<PhotonView>().IsMine) //if this belongs to the driver or the shooter attached to the driver
            {
                sr.color = Color.green;
            }
            else
            {
                sr.color = Color.red;
            }
        }

        //ai driver
        if(this.transform.parent.tag != "car")
        {
            if(aic != null && aic.Shooter != null && aic.Shooter.GetComponent<PhotonView>().IsMine) //if this belongs to the shooter attached to an ai car
            {
                sr.color = Color.green;
            }
            else
            {
                sr.color = Color.red;
            }
        }
    }
}
