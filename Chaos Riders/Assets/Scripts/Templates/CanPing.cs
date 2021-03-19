using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newPingableData", menuName = "Data/Pingable Data")]

public class CanPing : ScriptableObject
{
    [TagSelector]
    public List<string> tags = new List<string>();
}
