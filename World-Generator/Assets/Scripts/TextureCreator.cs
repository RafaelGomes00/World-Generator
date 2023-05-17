using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureCreator : MonoBehaviour
{
    public static Texture GenerateTexture(float[,] noiseValues, Gradient gradient, int textureSize)
    {
        Texture2D texture = new Texture2D(textureSize, textureSize);
        Color[] colorMap = new Color[textureSize * textureSize];

        float minNoiseHeight = float.MaxValue;
        float maxNoiseHeight = float.MinValue;

        for (int y = 0; y < textureSize; y++)
        {
            for (int x = 0; x < textureSize; x++)
            {
                // Debug.Log(Mathf.InverseLerp(0, 1, noiseValues[x, y]));

                if (noiseValues[x, y] > maxNoiseHeight)
                    maxNoiseHeight = noiseValues[x, y];
                else if (noiseValues[x, y] < minNoiseHeight)
                    minNoiseHeight = noiseValues[x, y];

                colorMap[y * noiseValues.GetLength(0) + x] = gradient.Evaluate(Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseValues[x, y]));
            }
        }

        texture.SetPixels(colorMap);
        texture.Apply();

        return texture;
    }
}
