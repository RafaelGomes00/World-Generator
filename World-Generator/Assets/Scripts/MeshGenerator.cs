using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public static class MeshGenerator
{
    public static Mesh GenerateMesh(float[,] noiseValues, int area, out MeshInfo meshInfo)
    {
        Mesh mesh = new Mesh();
        mesh.Clear();

        Vector3[,] verticesGrid = new Vector3[0, 0];
        Vector3[] vertices = GenerateVertices(noiseValues, area, out verticesGrid);
        mesh.vertices = vertices;
        Edge[] edges = new Edge[0];
        mesh.triangles = GenerateTriangles(vertices, area, out edges);
        mesh.uv = GenerateUv(area);

        meshInfo = new MeshInfo(edges, verticesGrid);

        return mesh;
    }

    private static Vector3[] GenerateVertices(float[,] noise, int area, out Vector3[,] verticesGrid)
    {
        List<Vector3> vertices = new List<Vector3>();
        Vector3[,] generatedVerticesGrid = new Vector3[area, area];

        for (int i = 0; i < area; i++)
        {
            for (int j = 0; j < area; j++)
            {
                Vector3 pos = new Vector3(i + ((area - 1) / -2f), noise[i, j], j - ((area - 1) / 2f));
                vertices.Add(pos);
                generatedVerticesGrid[i, j] = pos;
            }
        }
        verticesGrid = generatedVerticesGrid;
        return vertices.ToArray();
    }

    private static Vector2[] GenerateUv(int density)
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

    private static int[] GenerateTriangles(Vector3[] points, int density, out Edge[] edges)
    {
        List<int> newTriangles = new List<int>();
        List<Edge> edgesList = new List<Edge>();
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
                edgesList.Add(new Edge(points[i], points[i + 1]));
                //Current to down
                edgesList.Add(new Edge(points[i], points[i + (int)Mathf.Sqrt(verticesCount)]));
                //Right to down
                edgesList.Add(new Edge(points[i + 1], points[i + (int)Mathf.Sqrt(verticesCount)]));

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
        edges = edgesList.ToArray();
        return newTriangles.ToArray();
    }

    public static Mesh GenerateWater(int size, out MeshInfo meshInfo)
    {
        Mesh mesh = new Mesh();
        Vector3[,] verticesGrid = new Vector3[0, 0];

        Vector3[] vertices = GenerateVertices(new float[size,size], size, out verticesGrid);
        mesh.vertices = vertices;
        Edge[] edges = new Edge[0];
        mesh.triangles = GenerateTriangles(vertices, size, out edges);
        mesh.uv = GenerateUv(size);

        meshInfo = new MeshInfo(edges, verticesGrid);

        return mesh;
    }
}
