﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//this script follows the players car for the minimap
//attached to the minimap cam gameobject on the player avatars
public class MiniMap : MonoBehaviour
{
    public Transform player;


    private void LateUpdate()
    {
        Vector3 newPosition = player.position;
        newPosition.y = this.transform.position.y;
        transform.position = newPosition;

        transform.rotation = Quaternion.Euler(90f, player.eulerAngles.y - 180, 0f);
    }
}
