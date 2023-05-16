using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private MeshFilter meshFilter;
    [SerializeField] private Noise noiseFunction;

    [Header("Values")]
    [SerializeField] private int chunkSize;

    [Header("Debug")]
    [OnValueChanged("UpdateGizmos")][SerializeField] private bool drawGizmos;
    [SerializeField] private MeshDrawer meshGizmo;

    private Mesh mesh;
    private MeshDrawer instantiatedMeshGizmo;
    private MeshInfo meshInfo;

    [Button]
    public void CreateTerrain()
    {
        mesh.Clear();
        meshInfo = new MeshInfo();
        mesh = MeshGenerator.GenerateMesh(noiseFunction, chunkSize, out meshInfo);
        meshFilter.mesh = mesh;
        UpdateGizmos();
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

        instantiatedMeshGizmo.Draw(meshInfo.edges, meshInfo.vertices);
    }
}
