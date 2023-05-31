using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public abstract class Noise : ScriptableObject
{
    [OnValueChanged("PreviewNoise")][SerializeField] protected int seed;
    [OnValueChanged("PreviewNoise")][SerializeField] protected int octaves;
    [OnValueChanged("PreviewNoise")][SerializeField] protected float noiseScale;
    [OnValueChanged("PreviewNoise")][SerializeField] protected float lacunarity;
    [OnValueChanged("PreviewNoise")][SerializeField] protected float mapHeight;
    [OnValueChanged("PreviewNoise")][SerializeField] protected Vector2 offset;
    [OnValueChanged("PreviewNoise")][SerializeField] protected AnimationCurve heightCurve;
    [OnValueChanged("PreviewNoise")][Range(0, 1)][SerializeField] protected float persistance;

    [ShowAssetPreview(64, 64)][SerializeField] private Texture2D _Preview;

    public abstract float[,] GenerateNoise(int width, int height, Vector2 center, Action<float[,], float, float> callback = null);

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
