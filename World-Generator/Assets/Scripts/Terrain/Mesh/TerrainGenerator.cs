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
    [SerializeField] private Material terrainMat;
    [SerializeField] private Material waterMaterial;
    
    [Header("Values")]
    [SerializeField] private int chunkSize;
    [Range(0, 100)][SerializeField] private float waterlevel;
    [Range(0, 5)][SerializeField] private int LODLevel;


    // [SerializeField] private Color deepSeaColor;
    // [SerializeField] private Color[] baseColors;
    // [Range(0,1)][SerializeField] private float[] baseBlends;
    // [Range(0, 1)][SerializeField] private float[] baseStartHeights;

    private MeshDrawer instantiatedMeshGizmo;
    private MeshData terrainMeshData;
    private MeshData waterMeshData;

    private GameObject terrain;
    private GameObject water;

    private Queue<MapThreadInfo<ChunkData>> chunkDataInfoQueue = new Queue<MapThreadInfo<ChunkData>>();
    private Queue<MapThreadInfo<ChunkData>> meshDataInfoQueue = new Queue<MapThreadInfo<ChunkData>>();

    private void Start()
    {
        // terrainMat.SetFloat("minHeight", noiseFunction.GetMinNoiseHeight());
        // terrainMat.SetFloat("maxHeight", noiseFunction.GetMaxNoiseHeight());
        // terrainMat.SetColor("deepSeaColor", deepSeaColor);
        // terrainMat.SetColorArray("baseColors", baseColors);
        // terrainMat.SetFloatArray("baseStartHeights", baseStartHeights);
        // terrainMat.SetInt("baseColorCount", baseColors.Length);
        // terrainMat.SetFloatArray("baseBlends", baseBlends);
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

    // private GameObject GenerateTerrain()
    // {
    //     GameObject terrain = new GameObject("Terrain");
    //     MeshRenderer terrainRenderer = terrain.AddComponent<MeshRenderer>();
    //     MeshFilter terrainMeshFilter = terrain.AddComponent<MeshFilter>();


    //     terrainMeshData = 
    //     Mesh mesh = new Mesh();
    //     mesh.vertices = terrainMeshData.vertices;
    //     mesh.triangles = terrainMeshData.triangles;
    //     mesh.uv = terrainMeshData.uv;

    //     terrainMeshFilter.mesh = mesh;


    //     terrainRenderer.sharedMaterial = mat;

    //     return terrain;
    // }
    // private GameObject GenerateWater(GameObject terrain)
    // {
    //     GameObject water = new GameObject("Water");
    //     water.transform.position = new Vector3(terrain.transform.position.x, waterlevel, terrain.transform.position.z);
    //     MeshRenderer waterMeshRenderer = water.AddComponent<MeshRenderer>();
    //     MeshFilter waterMeshFilter = water.AddComponent<MeshFilter>();

    //     waterMeshData = MeshGenerator.GenerateMesh(chunkSize, LODLevel);
    //     Mesh mesh = new Mesh();
    //     mesh.vertices = waterMeshData.vertices;
    //     mesh.triangles = waterMeshData.triangles;
    //     mesh.uv = waterMeshData.uv;

    //     waterMeshFilter.mesh = mesh;

    //     water.transform.SetParent(terrain.transform);

    //     Material mat = new Material(waterShader);
    //     waterMeshRenderer.sharedMaterial = mat;

    //     return water;
    // }

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

    // private void UpdateGizmos()
    // {
    //     if (!drawGizmos)
    //     {
    //         if (instantiatedMeshGizmo != null)
    //             DestroyImmediate(instantiatedMeshGizmo.gameObject);
    //         return;
    //     }

    //     if (instantiatedMeshGizmo == null)
    //     {
    //         instantiatedMeshGizmo = Instantiate(meshGizmo);
    //     }

    //     instantiatedMeshGizmo.Draw(terrainMeshData.edges, terrainMeshData.vertices);
    // }

}
