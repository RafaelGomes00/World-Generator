using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ChunkData
{
    public readonly MeshData terrainData;
    public readonly Material terrainMaterial;

    public readonly float waterLevel;

    public readonly MeshData waterData;
    public readonly Material waterMaterial;

    public ChunkData(MeshData terrain, Material terrainMaterial, MeshData water, Material waterMaterial, float waterLevel)
    {
        terrainData = terrain;
        waterData = water;
        this.terrainMaterial = terrainMaterial;
        this.waterMaterial = waterMaterial;
        this.waterLevel = waterLevel;
    }
}

public struct MaterialInfo
{
    public float[,] noiseValues;
    public Shader terrainShader;
    public float maxNoiseHeight;
    public float minNoiseHeight;
    public Color deepSeaColor;
    public Color[] baseColors;
    public float[] baseStartHeights;

    public MaterialInfo(float[,] noiseValues, Shader terrainShader, float maxNoiseHeight, float minNoiseHeight, Color[] baseColors, float[] baseStartHeights, Color deepSeaColor)
    {
        this.noiseValues = noiseValues;
        this.terrainShader = terrainShader;
        this.maxNoiseHeight = maxNoiseHeight;
        this.minNoiseHeight = minNoiseHeight;
        this.baseColors = baseColors;
        this.baseStartHeights = baseStartHeights;
        this.deepSeaColor = deepSeaColor;
    }
}
