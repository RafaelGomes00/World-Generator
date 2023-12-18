using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public abstract class Noise : ScriptableObject
{
    [SerializeField] protected int seed;
    [SerializeField] protected int octaves;
    [SerializeField] protected float noiseScale;
    [SerializeField] protected float lacunarity;
    [SerializeField] protected float mapHeight;
    [SerializeField] protected Vector2 offset;
    [SerializeField] protected AnimationCurve heightCurve;
    [Range(0, 1)][SerializeField] protected float persistance;

    [ShowAssetPreview(64, 64)][SerializeField] private Texture2D _Preview;

    public abstract float[,] GenerateNoise(int width, int height, Vector2 center, Action<float[,], float, float> callback = null);

    [Button]
    private void PreviewNoise()
    {
        int size = 251;
        GenerateNoise(size, size, Vector2.zero, SetTexture);
    }

    private void SetTexture(float[,] generatedNoise, float minNoiseHeight, float maxNoiseHeight)
    {
        int size = 251;
        Texture2D tex = new Texture2D(size, size);
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                tex.SetPixel(x, y, new Color(Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, generatedNoise[x, y]),
                Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, generatedNoise[x, y]),
                Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, generatedNoise[x, y])));
            }
        }
        tex.Apply();
        _Preview = tex;
    }
}
