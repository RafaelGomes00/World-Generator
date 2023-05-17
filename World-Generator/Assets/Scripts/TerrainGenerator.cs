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

    [Header("Debug")]
    [OnValueChanged("UpdateGizmos")][SerializeField] private bool drawGizmos;
    [SerializeField] private MeshDrawer meshGizmo;

    private MeshDrawer instantiatedMeshGizmo;
    private MeshInfo terrainMeshInfo;
    private MeshInfo waterMeshInfo;

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

        terrainMeshInfo = new MeshInfo();
        waterMeshInfo = new MeshInfo();

        terrain = GenerateTerrain();
        water = GenerateWater(terrain);

        UpdateGizmos();
    }

    private GameObject GenerateTerrain()
    {
        GameObject terrain = new GameObject("Terrain");
        MeshRenderer terrainRenderer = terrain.AddComponent<MeshRenderer>();
        MeshFilter terrainMeshFilter = terrain.AddComponent<MeshFilter>();

        float[,] noiseValues = noiseFunction.GenerateNoise(chunkSize, chunkSize);

        terrainMeshFilter.sharedMesh = MeshGenerator.GenerateMesh(noiseValues, chunkSize, out terrainMeshInfo);

        Material mat = new Material(terrainShader);
        mat.SetTexture("_MainTexture", TextureCreator.GenerateTexture(noiseValues, gradient, chunkSize));
        terrainRenderer.sharedMaterial = mat;

        return terrain;
    }
    private GameObject GenerateWater(GameObject terrain)
    {
        GameObject water = new GameObject("Water");
        water.transform.position = new Vector3(terrain.transform.position.x, waterlevel, terrain.transform.position.z);
        MeshRenderer waterMeshRenderer = water.AddComponent<MeshRenderer>();
        MeshFilter waterMeshFilter = water.AddComponent<MeshFilter>();
        waterMeshFilter.mesh = MeshGenerator.GenerateWater(chunkSize, out waterMeshInfo);
        water.transform.SetParent(terrain.transform);

        Material mat = new Material(waterShader);
        waterMeshRenderer.sharedMaterial = mat;

        return water;
    }

    private void UpdateGizmos()
    {
        if (!drawGizmos)
        {
            if (instantiatedMeshGizmo != null)
                DestroyImmediate(instantiatedMeshGizmo.gameObject);
            return;
        }

        if (instantiatedMeshGizmo == null)
        {
            instantiatedMeshGizmo = Instantiate(meshGizmo);
        }

        instantiatedMeshGizmo.Draw(terrainMeshInfo.edges, terrainMeshInfo.vertices);
    }
}
