using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Terrain generator/Texture/Terrain Texture", fileName = "Terrain texture")]
public class TerrainMaterial : ScriptableObject
{
    const int textureSize = 512;
    const TextureFormat textureFormat = TextureFormat.RGB565;

    public TerrainLayer[] layers;

    public void ChangeTexture(Material mat, float minHeight, float maxHeight)
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
