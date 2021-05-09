using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MatchText : MonoBehaviour
{
    public TMP_Text textToMatch;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.GetComponent<TMP_Text>().text = textToMatch.text;
    }
}
