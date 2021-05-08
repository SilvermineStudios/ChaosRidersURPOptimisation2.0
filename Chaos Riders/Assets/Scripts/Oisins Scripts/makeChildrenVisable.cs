using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class makeChildrenVisable : MonoBehaviour
{
    [SerializeField] GameObject Holder, Canvas, buttons;

    public void Activate()
    {
        Holder.transform.parent = Canvas.transform;
        Holder.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -200);
        buttons.SetActive(true);
    }
}
