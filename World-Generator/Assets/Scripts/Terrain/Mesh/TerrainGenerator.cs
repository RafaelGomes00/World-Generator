using System;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class TerrainGenerator : MonoBehaviour
{
    [Header("Noise config")]
    [SerializeField] private Noise noiseFunction;
    // [SerializeField] private Gradient gradient;

    [Header("Materials")]
    [SerializeField] private TerrainMaterial terrainTexture;
    [SerializeField] private Material terrainMat;
    [SerializeField] private Material waterMaterial;

    [Header("Values")]
    [SerializeField] private int chunkSize;
    [Range(0, 100)][SerializeField] private float waterlevel;
    [Range(0, 5)][SerializeField] private int LODLevel;


    private MeshDrawer instantiatedMeshGizmo;
    private MeshData terrainMeshData;
    private MeshData waterMeshData;

    private GameObject terrain;
    private GameObject water;

    private Queue<MapThreadInfo<ChunkData>> chunkDataInfoQueue = new Queue<MapThreadInfo<ChunkData>>();
    private Queue<MapThreadInfo<ChunkData>> meshDataInfoQueue = new Queue<MapThreadInfo<ChunkData>>();

    private void Start()
    {
        terrainTexture.ChangeTexture(terrainMat, noiseFunction.GetMinNoiseHeight(), noiseFunction.GetMaxNoiseHeight());
    }

    private void Update()
    {
        if (chunkDataInfoQueue.Count > 0)
        {
            for (int i = 0; i < chunkDataInfoQueue.Count; i++)
            {
                MapThreadInfo<ChunkData> threadInfo = chunkDataInfoQueue.Dequeue();
                threadInfo.callback(threadInfo.parameter);
            }
        }
    }

    private void ResetTerrain()
    {
        if (terrain != null)
        {
            DestroyImmediate(terrain);
            terrain = null;
        }
        if (water != null)
        {
            DestroyImmediate(water);
            water = null;
        }

        terrainMeshData = new MeshData();
        waterMeshData = new MeshData();
    }

    public void RequestMapData(Vector2 center, Action<ChunkData> callback)
    {
        ThreadStart threadStart = delegate
        {
            RequestMapDataRoutine(center, callback);
        };

        new Thread(threadStart).Start();
    }

    public void RequestMapDataRoutine(Vector2 center, Action<ChunkData> callback)
    {
        ChunkData chunkData = GenerateChunk(center);
        lock (chunkDataInfoQueue) { chunkDataInfoQueue.Enqueue(new MapThreadInfo<ChunkData>(callback, chunkData)); }
    }

    private ChunkData GenerateChunk(Vector2 center)
    {
        float min, max;
        float[,] noiseValues = noiseFunction.GenerateNoise(chunkSize, chunkSize, center, out min, out max);
        MeshData terrainData = MeshGenerator.GenerateMesh(noiseValues, chunkSize, LODLevel);
        MeshData waterData = MeshGenerator.GenerateMesh(chunkSize, LODLevel);

        return new ChunkData(terrainData, terrainMat, waterData, waterMaterial, waterlevel);
    }

    public int GetChunkSize()
    {
        return chunkSize;
    }

    struct MapThreadInfo<T>
    {
        public readonly Action<T> callback;
        public readonly T parameter;

        public MapThreadInfo(Action<T> callback, T parameter)
        {
            this.callback = callback;
            this.parameter = parameter;
        }
    }
}
