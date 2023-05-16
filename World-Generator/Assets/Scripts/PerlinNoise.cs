using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PerlinNoise : Noise
{
    [SerializeField] RawImage debugImage;
    public override float[,] GenerateNoise(int width, int height)
    {
        float[,] pixels = new float[width, height];
        Texture2D tex = new Texture2D(width, height);

        System.Random prng = new System.Random(seed);
        Vector2[] octaveOffsets = new Vector2[octaves];
        for (int i = 0; i < octaves; i++)
        {
            float offsetX = prng.Next(-10000, 10000) + offset.x;
            float offsetY = prng.Next(-10000, 10000) + offset.y;
            octaveOffsets[i] = new Vector2(offsetX, offsetY);
        }

        if (noiseScale <= 0)
        {
            noiseScale = 0.001f;
        }

        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;

                for (int i = 0; i < octaves; i++)
                {
                    float xCoord = x / noiseScale * frequency + octaveOffsets[i].x;
                    float yCoord = y / noiseScale * frequency + octaveOffsets[i].y;

                    float noiseValue = Mathf.PerlinNoise(xCoord, yCoord) * 2 - 1;
                    noiseHeight += noiseValue * amplitude;

                    amplitude *= persistance;
                    frequency *= lacunarity;
                }

                if (noiseHeight > maxNoiseHeight)
                    maxNoiseHeight = noiseHeight;
                else if (noiseHeight < minNoiseHeight)
                    minNoiseHeight = noiseHeight;


                pixels[x, y] = noiseHeight;
            }
        }

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                pixels[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, pixels[x, y]);
                tex.SetPixel(x, y, Color.Lerp(Color.black, Color.white, pixels[x, y]));

                pixels[x, y] = heightCurve.Evaluate(pixels[x,y]) * mapHeight;
            }
        }

        tex.Apply();
        debugImage.texture = tex;
        return pixels;
    }

    void OnValidate()
    {
        if (lacunarity < 1)
            lacunarity = 1;
        if (octaves < 0)
            octaves = 0;
    }
}
