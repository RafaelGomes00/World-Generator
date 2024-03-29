using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Terrain generator/Noise/Perlin Noise", fileName = "NoiseSettings")]
public class PerlinNoise : Noise
{
    public override float[,] GenerateNoise(int width, int height, Vector2 center, out float minNoiseHeight, out float maxNoiseHeight)
    {
        float[,] pixels = new float[width, height];

        float maxPossibleHeight = 0;
        float amplitude = 1;
        float frequency = 1;

        System.Random prng = new System.Random(seed);
        Vector2[] octaveOffsets = new Vector2[octaves];
        for (int i = 0; i < octaves; i++)
        {
            float offsetX = prng.Next(-100000, 100000) + offset.x + center.x;
            float offsetY = prng.Next(-100000, 100000) - offset.y + center.y;
            octaveOffsets[i] = new Vector2(offsetX, offsetY);

            maxPossibleHeight += amplitude;
            amplitude *= persistance;
        }

        if (noiseScale <= 0)
        {
            noiseScale = 0.001f;
        }

        float maxCalculatedNoiseHeight = float.MinValue;
        float minCalculatedNoiseHeight = float.MaxValue;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                amplitude = 1;
                frequency = 1;
                float noiseHeight = 0;

                for (int i = 0; i < octaves; i++)
                {
                    float xCoord = (x + octaveOffsets[i].x) / noiseScale * frequency;
                    float yCoord = (y + octaveOffsets[i].y) / noiseScale * frequency;

                    float noiseValue = Mathf.PerlinNoise(xCoord, yCoord) * 2 - 1;
                    noiseHeight += noiseValue * amplitude;

                    amplitude *= persistance;
                    frequency *= lacunarity;
                }

                if (noiseHeight > maxCalculatedNoiseHeight)
                    maxCalculatedNoiseHeight = noiseHeight;
                else if (noiseHeight < minCalculatedNoiseHeight)
                    minCalculatedNoiseHeight = noiseHeight;

                pixels[x, y] = noiseHeight;
            }
        }

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float normalizedHeight = (pixels[x, y] + 1) / (2f * maxPossibleHeight / 1.75f);
                pixels[x, y] = Mathf.Clamp(normalizedHeight, 0, int.MaxValue);
                // pixels[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, pixels[x, y]);

                AnimationCurve nHeightCurve = new AnimationCurve(heightCurve.keys);
                pixels[x, y] = nHeightCurve.Evaluate(pixels[x, y]) * mapHeight;
            }
        }

        minNoiseHeight = minCalculatedNoiseHeight;
        maxNoiseHeight = maxCalculatedNoiseHeight;

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
