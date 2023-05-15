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
        // List<float> pixels = new List<float>();
        float[,] pixels = new float[width, height]; 
        Texture2D tex = new Texture2D(width, height);
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                float noise = CalculateNoise(i, j, width, height);
                pixels[i,j] = (noise * noiseHeight);
                tex.SetPixel(i, j, new Color(noise, noise, noise));
            }
        }
        tex.Apply();
        debugImage.texture = tex;
        return pixels;
    }

    private float CalculateNoise(int x, int y, int width, int height)
    {
        float xCoord = ((float)x / width) * noiseScale;
        float yCoord = ((float)y / height) * noiseScale;

        return Mathf.PerlinNoise(xCoord, yCoord);
    }
}
