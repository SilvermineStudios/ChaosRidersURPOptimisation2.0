using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class makeChildrenVisable : MonoBehaviour
{
    [SerializeField] GameObject Holder, Canvas, buttons;

    public void Activate()
    {
        Holder.transform.parent = Canvas.transform;
        buttons.SetActive(true);
    }
}
