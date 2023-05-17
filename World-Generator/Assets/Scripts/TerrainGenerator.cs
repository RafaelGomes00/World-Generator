using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    [SerializeField] private Noise noiseFunction;
    [SerializeField] private Shader terrainShader;
    [SerializeField] private Shader waterShader;
    [SerializeField] private Gradient gradient;

    [Header("Values")]
    [SerializeField] private int chunkSize;
    [Range(0, 5)][SerializeField] private float waterlevel;
    [Range(0, 5)][SerializeField] private int LODLevel;

    // [Header("Debug")]
    // [OnValueChanged("UpdateGizmos")][SerializeField] private bool drawGizmos;
    // [SerializeField] private MeshDrawer meshGizmo;

    private MeshDrawer instantiatedMeshGizmo;
    private MeshData terrainMeshData;
    private MeshData waterMeshData;

    private GameObject terrain;
    private GameObject water;

    [Button]
    public void CreateTerrain()
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

        terrain = GenerateTerrain();
        water = GenerateWater(terrain);

        // UpdateGizmos();
    }

    private GameObject GenerateTerrain()
    {
        GameObject terrain = new GameObject("Terrain");
        MeshRenderer terrainRenderer = terrain.AddComponent<MeshRenderer>();
        MeshFilter terrainMeshFilter = terrain.AddComponent<MeshFilter>();

        float[,] noiseValues = noiseFunction.GenerateNoise(chunkSize, chunkSize);

        terrainMeshData = MeshGenerator.GenerateMesh(noiseValues, chunkSize, LODLevel);
        Mesh mesh = new Mesh();
        mesh.vertices = terrainMeshData.vertices;
        mesh.triangles = terrainMeshData.triangles;
        mesh.uv = terrainMeshData.uv;

        terrainMeshFilter.mesh = mesh;

        Material mat = new Material(terrainShader);
        mat.SetTexture("_MainTexture", TextureCreator.GenerateTexture(noiseValues, gradient, waterlevel, chunkSize));
        terrainRenderer.sharedMaterial = mat;

        return terrain;
    }
    private GameObject GenerateWater(GameObject terrain)
    {
        GameObject water = new GameObject("Water");
        water.transform.position = new Vector3(terrain.transform.position.x, waterlevel, terrain.transform.position.z);
        MeshRenderer waterMeshRenderer = water.AddComponent<MeshRenderer>();
        MeshFilter waterMeshFilter = water.AddComponent<MeshFilter>();

        waterMeshData = MeshGenerator.GenerateMesh(chunkSize, LODLevel);
        Mesh mesh = new Mesh();
        mesh.vertices = waterMeshData.vertices;
        mesh.triangles = waterMeshData.triangles;
        mesh.uv = waterMeshData.uv;

        waterMeshFilter.mesh = mesh;

        water.transform.SetParent(terrain.transform);

        Material mat = new Material(waterShader);
        waterMeshRenderer.sharedMaterial = mat;

        return water;
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
