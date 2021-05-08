using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class ExtensionMethods 
{

    public static T ChangeAlpha<T>(this T g, float newAlpha)
     where T : Graphic
    {
        var color = g.color;
        color.a = newAlpha;
        g.color = color;
        return g;
    }

    public static T SubtractAlpha<T>(this T g, float change)
    where T : Graphic
    {
        var color = g.color;
        color.a -= change;
        g.color = color;
        return g;
    }


}
