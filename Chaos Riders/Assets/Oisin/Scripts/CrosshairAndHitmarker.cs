using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrosshairAndHitmarker : MonoBehaviour
{
    [SerializeField] RectTransform reticle; // The RecTransform of reticle UI element.
    [SerializeField] GameObject CrossHairGameobject;

    Pause pauseMenu;
    public float restingSize;
    public float maxSize;
    public float speed;
    public float currentSize;
    private Shooter shooterScript;
    private float spreadSize;
    private void Start()
    {
        pauseMenu = GetComponent<Pause>();
        shooterScript = GetComponent<Shooter>();
    }

    private void Update()
    {
        if(pauseMenu.paused)
        {
            CrossHairGameobject.SetActive(false);
        }
        else
        {
            CrossHairGameobject.SetActive(true);
        }
        //Debug.Log(shooterScript.isShooting);
        if (shooterScript.isShooting)
        {
            spreadSize = shooterScript.spread * 10 + restingSize;
            if (spreadSize < restingSize)
            {
                spreadSize = restingSize;
            }
            currentSize = Mathf.Lerp(currentSize, spreadSize, Time.deltaTime * speed);
        }
        else
        {
            currentSize = Mathf.Lerp(currentSize, restingSize, Time.deltaTime * speed);
        }

        reticle.sizeDelta = new Vector2(currentSize, currentSize);

    }
}
