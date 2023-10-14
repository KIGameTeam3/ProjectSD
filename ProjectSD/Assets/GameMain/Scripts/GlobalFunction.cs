using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalFunction
{
    public static int GetLayerMask(string layerName)
    {
        return 1 << LayerMask.NameToLayer(layerName);
    }

    public static void ChangeMaterialColor<T>(this T material, Color color) where T : Material 
    {
        material.color = color;
    }
}
