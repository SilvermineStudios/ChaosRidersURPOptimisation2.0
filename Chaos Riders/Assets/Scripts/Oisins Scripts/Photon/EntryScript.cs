using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EntryScript : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI driverName;
    [SerializeField] TextMeshProUGUI shooterName;
    [SerializeField] TextMeshProUGUI position;
    bool isSetup;

    public void Setup(string newDriverName, string newShooterName, string newPosition)
    {
        if (isSetup) { return; }
        driverName.text = newDriverName;
        shooterName.text = newShooterName;
        position.text = newPosition;
    }


}
