using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MiniMapPlayerColour : MonoBehaviour
{
    private PhotonView pv;
    private SpriteRenderer sr; //use this to change the colour of the player on the map
    private Controller c; //use this to get access to the shooter attached
 
    private void Awake()
    {
        pv = this.transform.parent.gameObject.GetComponent<PhotonView>();
        sr = GetComponent<SpriteRenderer>();
        c = this.transform.parent.gameObject.GetComponent<Controller>();
    }

    void Update()
    {
        //if theres a shooter attached
        if(c.Shooter != null)
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
        if(c.Shooter == null)
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
}
