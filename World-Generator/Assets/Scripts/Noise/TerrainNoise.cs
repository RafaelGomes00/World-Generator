using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Terrain generator/Noise settings", fileName = "NoiseSettings")]
public class TerrainNoise : ScriptableObject
{
    public string objectsPath;
    [Expandable] public List<Noise> noiseFunctions;

    public float[,] GenerateNoise(int chunkSize, Vector2 center)
    {
        return noiseFunctions[0].GenerateNoise(chunkSize, chunkSize, center);
    }

    public void AddNewItem(Noise function)
    {
        noiseFunctions.Add(function);
    }

    private string GeneratePath()
    {
        return $"{System.IO.Path.Combine(objectsPath, "Noise")}_{noiseFunctions.Count}.asset";
    }

}
