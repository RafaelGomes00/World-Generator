using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshDrawer : MonoBehaviour
{
    [SerializeField] private MeshFilter meshFilter;
    // [SerializeField] private float spacing;
    [SerializeField] private int density;
    [SerializeField] private int area;
    [SerializeField] private Color[] gizmosColors;

    private Mesh mesh;
    private List<Edge> edges = new List<Edge>();
    private List<Vector3> debugVertices = new List<Vector3>();

    private void Start()
    {
        mesh = new Mesh();
        mesh.Clear();
        meshFilter.mesh = mesh;

        Vector3[] vertices = GenerateVertices();
        mesh.vertices = vertices;
        mesh.triangles = GenerateTriangles(vertices);
        mesh.uv = GenerateUv();
    }

    private Vector3[] GenerateVertices()
    {
        List<Vector3> vertices = new List<Vector3>();
        float spacing = (float)area / (float)density;
        for (int i = 0; i < density; i++)
        {
            for (int j = 0; j < density; j++)
            {
                Vector3 pos = new Vector3(i * spacing, 0, j * spacing);
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        if (debugVertices == null)
            return;

        foreach (Vector3 vertice in debugVertices)
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
