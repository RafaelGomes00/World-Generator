using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainChunk
{
    private Vector2 position;
    private Bounds bounds;

    private GameObject terrainMeshObject;
    private MeshRenderer terrainRenderer;
    private MeshFilter terrainMeshFilter;

    private GameObject waterMeshObject;
    private MeshRenderer waterRenderer;
    private MeshFilter waterMeshFilter;

    public TerrainChunk(Vector2 viewedChunk, int chunkSize, TerrainGenerator terrainGenerator, Transform parent)
    {
        position = viewedChunk * chunkSize;
        bounds = new Bounds(position, Vector2.one * chunkSize);
        Vector3 positionV3 = new Vector3(position.x, 0, position.y);

        terrainMeshObject = new GameObject("Terrain Chunk");
        terrainRenderer = terrainMeshObject.AddComponent<MeshRenderer>();
        terrainMeshFilter = terrainMeshObject.AddComponent<MeshFilter>();

        waterMeshObject = new GameObject("Water");
        waterRenderer = waterMeshObject.AddComponent<MeshRenderer>();
        waterMeshFilter = waterMeshObject.AddComponent<MeshFilter>();
        waterMeshObject.transform.SetParent(terrainMeshObject.transform);

        terrainMeshObject.transform.position = positionV3;
        waterMeshObject.transform.position = positionV3;
        terrainMeshObject.transform.SetParent(parent);
        SetVisible(false);

        terrainGenerator.RequestMapData(position, OnChunkDataReceived);
    }

    private void OnChunkDataReceived(ChunkData chunkData)
    {
        Mesh terrainMesh = new Mesh();
        terrainMesh.vertices = chunkData.terrainData.vertices;
        terrainMesh.triangles = chunkData.terrainData.triangles;
        terrainMesh.uv = chunkData.terrainData.uv;

        Material terrainMat = new Material(chunkData.terrainMatInfo.terrainShader);
        terrainMat.SetTexture("_MainTexture", TextureCreator.GenerateTexture(chunkData.terrainMatInfo.noiseValues,
        chunkData.terrainMatInfo.gradient,
        chunkData.terrainMatInfo.waterlevel,
        chunkData.terrainMatInfo.chunkSize));

        terrainRenderer.sharedMaterial = terrainMat;
        terrainMeshFilter.mesh = terrainMesh;

        Mesh waterMesh = new Mesh();
        waterMesh.vertices = chunkData.waterData.vertices;
        waterMesh.triangles = chunkData.waterData.triangles;
        waterMesh.uv = chunkData.waterData.uv;

        Material waterMat = new Material(chunkData.waterShader);
        waterRenderer.sharedMaterial = waterMat;
        waterMeshFilter.mesh = waterMesh;
    }

    public void SetVisible(bool value)
    {
        terrainMeshObject.SetActive(value);
    }

    public void UpdateTerrainChunk(Vector2 viewerPosition, float maxViewDist)
    {
        float viewerDistFromNearestEdge = Mathf.Sqrt(bounds.SqrDistance(viewerPosition));
        SetVisible(viewerDistFromNearestEdge <= maxViewDist);
    }

    public bool IsVisible()
    {
        return terrainMeshObject.activeInHierarchy;
    }
}
