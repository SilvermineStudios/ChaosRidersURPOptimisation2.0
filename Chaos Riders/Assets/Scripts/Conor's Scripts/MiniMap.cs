using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//this script follows the players car for the minimap
public class MiniMap : MonoBehaviour
{
    public Transform player;


    private void LateUpdate()
    {
        Vector3 newPosition = player.position;
        newPosition.y = this.transform.position.y;
        transform.position = newPosition;
    }
}
