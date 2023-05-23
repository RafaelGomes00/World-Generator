using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PerlinNoise : Noise
{
    [SerializeField] RawImage debugImage;
    public override float[,] GenerateNoise(int width, int height, Vector2 center)
    {
        float[,] pixels = new float[width, height];
        // Texture2D tex = new Texture2D(width, height);

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

        // float maxNoiseHeight = float.MinValue;
        // float minNoiseHeight = float.MaxValue;

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

                // if (noiseHeight > maxNoiseHeight)
                //     maxNoiseHeight = noiseHeight;
                // else if (noiseHeight < minNoiseHeight)
                //     minNoiseHeight = noiseHeight;

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

        // tex.Apply();
        // debugImage.texture = tex;
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
