using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureCreator : MonoBehaviour
{
    public static Texture GenerateTexture(float[,] noiseValues, Gradient gradient, float waterLevel, int textureSize)
    {
        Texture2D texture = new Texture2D(textureSize, textureSize);
        Color[] colorMap = new Color[textureSize * textureSize];

        float maxNoiseHeight = float.MinValue;

        for (int y = 0; y < textureSize; y++)
        {
            for (int x = 0; x < textureSize; x++)
            {
                // Debug.Log(Mathf.InverseLerp(0, 1, noiseValues[x, y]));

                if (noiseValues[x, y] > maxNoiseHeight)
                    maxNoiseHeight = noiseValues[x, y];

                colorMap[y * noiseValues.GetLength(0) + x] = gradient.Evaluate(Mathf.InverseLerp(waterLevel, maxNoiseHeight, noiseValues[x, y]));
            }
        }

        texture.SetPixels(colorMap);
        texture.Apply();

        return texture;
    }
}
