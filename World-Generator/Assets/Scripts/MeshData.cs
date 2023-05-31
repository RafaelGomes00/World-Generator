using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshData
{
    public int[] triangles;
    public Vector3[] vertices;
    public Vector2[] uv;

    private int triangleIndex = 0;

    public MeshData() {}

    public MeshData(int width, int height)
    {
        vertices = new Vector3[width * height];
        triangles = new int[(width - 1) * (height - 1) * 6];
    }

    public void AddTriangle(int a, int b, int c)
    {
        triangles[triangleIndex] = a;
        triangles[triangleIndex + 1] = b;
        triangles[triangleIndex + 2] = c;
        triangleIndex += 3;
    }
}