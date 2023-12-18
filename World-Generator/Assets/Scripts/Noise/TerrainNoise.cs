using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Terrain generator/Noise settings", fileName = "NoiseSettings")]
public class TerrainNoise : ScriptableObject
{
    public string objectsPath;
    [Expandable] public Noise noiseFunction;

    public float[,] GenerateNoise(int chunkSize, Vector2 center)
    {
        return noiseFunction.GenerateNoise(chunkSize, chunkSize, center);
    }
}
