using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CalculateFPS : MonoBehaviour
{
    public TMP_Text FPSText;
    private float timer, refresh, avgFramerate;

    void Update()
    {
        float timelapse = Time.smoothDeltaTime;
        timer = timer <= 0 ? refresh : timer -= timelapse;

        if (timer <= 0) avgFramerate = (int) (1f / timelapse);
        FPSText.text = avgFramerate.ToString();
    }
}
