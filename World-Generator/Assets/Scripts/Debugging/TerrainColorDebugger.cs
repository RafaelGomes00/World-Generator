using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TerrainColorDebugger : MonoBehaviour
{
    const int textureSize = 512;
    const TextureFormat textureFormat = TextureFormat.RGB565;
    [SerializeField] private Material mat;
    [SerializeField] private float minHeight, maxHeight;

    public Layer[] layers;

    [NaughtyAttributes.Button]
    public void ChangeColor()
    {
        mat.SetFloat("minHeight", minHeight);
        mat.SetFloat("maxHeight", maxHeight);

        mat.SetInt("layerCount", layers.Length);

        mat.SetColorArray("baseColors", layers.Select(x => x.color).ToArray());
        mat.SetFloatArray("baseStartHeights", layers.Select(x => x.startHeight).ToArray());
        mat.SetFloatArray("baseBlends", layers.Select(x => x.blendStrength).ToArray());
        mat.SetFloatArray("baseColorStrength", layers.Select(x => x.colorStrength).ToArray());
        mat.SetFloatArray("baseTextureScales", layers.Select(x => x.textureScale).ToArray());
       
       Texture2DArray texturesArray = GenerateTextureArray(layers.Select(x => x.tex).ToArray());
       mat.SetTexture("baseTextures", texturesArray);

       Debug.Log("Setted");
    }

    Texture2DArray GenerateTextureArray(Texture2D[] textures)
    {
        Texture2DArray textureArray = new Texture2DArray(textureSize, textureSize, textures.Length, textureFormat, true);
        for(int i = 0; i < textures.Length; i++)
        {
            textureArray.SetPixels(textures[i].GetPixels(), i);
        }

        textureArray.Apply();
        return textureArray;
    }
}

[System.Serializable]
public class Layer
{
    public Texture2D tex;
    public Color color;
    [Range(0,1)] public float colorStrength;
    [Range(0,1)] public float startHeight;
    [Range(0,1)] public float blendStrength;
    public float textureScale;
}
