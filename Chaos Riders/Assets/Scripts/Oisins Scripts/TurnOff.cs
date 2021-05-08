using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class TurnOff : MonoBehaviour
{
    [SerializeField] bool visualEffect;
    [SerializeField] bool gameobject;
    [SerializeField] float TimeToTurnOff;


    VisualEffect effect;
    GameObject gO;

    void OnEnable()
    {
        if(visualEffect)
        {
            effect = GetComponent<VisualEffect>();   
        }
    }

    public void TriggerMe()
    {
        StartCoroutine(Running());
    }


    IEnumerator Running()
    {
        yield return new WaitForSeconds(TimeToTurnOff);
        if (visualEffect)
        {
            effect.Stop();
        }
        if(gameobject)
        {
            gameObject.SetActive(false);
        }
    }

}
