using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ChunkData
{
    public readonly MeshData terrainData;
    public readonly MaterialInfo terrainMatInfo;

    public readonly MeshData waterData;
    public readonly Shader waterShader;

    public ChunkData(MeshData terrain, MaterialInfo terrainMatInfo, MeshData water, Shader waterShader)
    {
        terrainData = terrain;
        this.terrainMatInfo = terrainMatInfo;
        waterData = water;
        this.waterShader = waterShader;
    }
}

public struct MaterialInfo
{
    public float[,] noiseValues;
    public Shader terrainShader;
    public Gradient gradient;
    public float waterlevel;
    public int chunkSize;

    public MaterialInfo(float[,] noiseValues, Shader terrainShader, Gradient gradient, float waterLevel, int chunkSize)
    {
        this.noiseValues = noiseValues;
        this.terrainShader = terrainShader;
        this.gradient = gradient;
        this.waterlevel = waterLevel;
        this.chunkSize = chunkSize;
    }
}
