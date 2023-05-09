using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshDrawer : MonoBehaviour
{
    [SerializeField] private MeshFilter meshFilter;
    [SerializeField] private int density;
    [SerializeField] private int area;
    [SerializeField] private Color[] gizmosColors;

    private Mesh mesh;
    private Vector3[,] pointsMatriz;
    private List<Edge> edges = new List<Edge>();
    private int verticesCount;

    private void Start()
    {
        mesh = new Mesh();
        meshFilter.mesh = mesh;

        Vector3[] vertices = GenerateVertices();
        mesh.vertices = vertices;
        mesh.triangles = GenerateTriangles(vertices);
        // GenerateTriangles(vertices);
    }

    private void GenerateVerticesVisualization()
    {
        pointsMatriz = new Vector3[area * density, area * density];
        float spacing = area / density;
        for (int i = 0; i < area * density; i++)
        {
            for (int j = 0; j < area * density; j++)
            {
                Vector3 pos = new Vector3(i * spacing, 0, j * spacing);
                pointsMatriz[i, j] = pos;
            }
        }
    }

    private Vector3[] GenerateVertices()
    {
        GenerateVerticesVisualization();
        List<Vector3> vertices = new List<Vector3>();
        verticesCount = area * area * density;
        float spacing = area / density;
        for (int i = 0; i < area * density; i++)
        {
            for (int j = 0; j < area * density; j++)
            {
                Vector3 pos = new Vector3(i * spacing, 0, j * spacing);
                vertices.Add(pos);
            }
        }
        return vertices.ToArray();
    }

    private int[] GenerateTriangles(Vector3[] points)
    {
        List<int> newTriangles = new List<int>();
        for (int i = 0; i < 100; i++)
        {
            if (i % Mathf.Sqrt(verticesCount) < Mathf.Sqrt(verticesCount) - 1)
            {
                if (!(i % Mathf.Sqrt(verticesCount) < Mathf.Sqrt(verticesCount) - 1))
                    Debug.Log($"{i % Mathf.Sqrt(verticesCount)} < {Mathf.Sqrt(verticesCount)}");

                //Curent Vertex
                newTriangles.Add(i);
                //Right Vertex
                newTriangles.Add(i + 1);
                //Down vertex
                newTriangles.Add(i + (int)Mathf.Sqrt(verticesCount));

                //Current to right
                // edges.Add(new Edge(points[i], points[i + 1]));
                //Current to down
                // edges.Add(new Edge(points[i], points[i + (int)Mathf.Sqrt(verticesCount)]));
                //Right to down
                // edges.Add(new Edge(points[i + 1], points[i + (int)Mathf.Sqrt(verticesCount)]));

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
        Gizmos.color = Color.white;
        if (pointsMatriz == null)
            return;

        foreach (Vector3 vertice in pointsMatriz)
        {
            Gizmos.DrawSphere(vertice, 0.5f);
        }

        for (int i = 0; i < edges.Count; i++)
        {
            Gizmos.color = gizmosColors[i % gizmosColors.Length];
            Gizmos.DrawLine(edges[i].start, edges[i].end);
        }
    }
}
