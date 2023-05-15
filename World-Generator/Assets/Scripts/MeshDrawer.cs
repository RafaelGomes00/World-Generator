using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class MeshDrawer : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private MeshFilter meshFilter;
    [SerializeField] private Noise noiseFunction;

    [Header("Values")]
    [SerializeField] private int density;
    [SerializeField] private int area;

    [Header("Debug")]
    [SerializeField] private bool drawGizmos;
    [SerializeField] private Color verticeColor;
    [SerializeField] private float verticeSize;
    [SerializeField] private Color edgeColor;
    private List<Edge> edges = new List<Edge>();
    private List<Vector3> debugVertices = new List<Vector3>();

    private Mesh mesh;

    [Button]
    public void ClearMesh()
    {
        mesh = new Mesh();
        mesh.Clear();
        meshFilter.mesh = mesh;

        debugVertices = new List<Vector3>();
        edges = new List<Edge>();
    }

    [Button]
    public void GenerateMesh()
    {
        ClearMesh();
        
        float[,] noiseValues = noiseFunction.GenerateNoise(density, density);

        Vector3[] vertices = GenerateVertices(noiseValues);
        mesh.vertices = vertices;
        mesh.triangles = GenerateTriangles(vertices);
        mesh.uv = GenerateUv();
    }

    private Vector3[] GenerateVertices(float[,] noise)
    {
        List<Vector3> vertices = new List<Vector3>();
        float spacing = (float)area / (float)density;
        for (int i = 0; i < density; i++)
        {
            for (int j = 0; j < density; j++)
            {
                Vector3 pos = new Vector3(i * spacing, noise[i,j], j * spacing);
                vertices.Add(pos);
                debugVertices.Add(pos);
            }
        }
        return vertices.ToArray();
    }

    private Vector2[] GenerateUv()
    {
        List<Vector2> uv = new List<Vector2>();
        for (int i = 0; i < density; i++)
        {
            for (int j = 0; j < density; j++)
            {
                uv.Add(new Vector2(i / ((float)density - 1), j / ((float)density - 1)));
            }
        }

        return uv.ToArray();
    }

    private int[] GenerateTriangles(Vector3[] points)
    {
        List<int> newTriangles = new List<int>();
        int verticesCount = points.Length;
        for (int i = 0; i < verticesCount - density; i++)
        {
            if (i % Mathf.Sqrt(verticesCount) < Mathf.Sqrt(verticesCount) - 1)
            {
                //Curent Vertex
                newTriangles.Add(i);
                //Right Vertex
                newTriangles.Add(i + 1);
                //Down vertex
                newTriangles.Add(i + (int)Mathf.Sqrt(verticesCount));

                //Current to right
                edges.Add(new Edge(points[i], points[i + 1]));
                //Current to down
                edges.Add(new Edge(points[i], points[i + (int)Mathf.Sqrt(verticesCount)]));
                //Right to down
                edges.Add(new Edge(points[i + 1], points[i + (int)Mathf.Sqrt(verticesCount)]));

                if (i > 0 && i % Mathf.Sqrt(verticesCount) > 0)
                {
                    newTriangles.Add(i);
                    newTriangles.Add(i + (int)Mathf.Sqrt(verticesCount));
                    newTriangles.Add(i + (int)Mathf.Sqrt(verticesCount) - 1);
                }
            }
            else
            {
                newTriangles.Add(i);
                newTriangles.Add(i + (int)Mathf.Sqrt(verticesCount));
                newTriangles.Add(i + (int)Mathf.Sqrt(verticesCount) - 1);
            }
        }
        return newTriangles.ToArray();
    }

    private void OnDrawGizmos()
    {
        if (!drawGizmos)
            return;

        Gizmos.color = Color.white;
        if (debugVertices == null)
            return;

        for (int i = 0; i < edges.Count; i++)
        {
            // Gizmos.color = gizmosColors[i % gizmosColors.Length];
            Gizmos.color = edgeColor;
            Gizmos.DrawLine(edges[i].start, edges[i].end);
        }

        foreach (Vector3 vertice in debugVertices)
        {
            Gizmos.color = verticeColor;
            Gizmos.DrawSphere(vertice, verticeSize);
        }
    }
}
