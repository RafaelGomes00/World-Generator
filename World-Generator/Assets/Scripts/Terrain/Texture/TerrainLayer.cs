using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TerrainLayer
{
    public Texture2D tex;
    public Color color;
    [Range(0,1)] public float colorStrength;
    [Range(0,1)] public float startHeight;
    [Range(0,1)] public float blendStrength;
    public float textureScale;
}
