using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MeshGenerator
{
    public static MeshData GenerateMesh(float[,] noiseValues, int area, int LODLevel)
    {
        List<int> newTriangles = new List<int>();
        List<Vector2> uvList = new List<Vector2>();
        MeshData meshData = new MeshData(area, area);
        Edge[] edges = new Edge[0];

        int lodIncrement = (LODLevel == 0) ? 1 : LODLevel * 2;
        int verticesPerLine = (area - 1) / lodIncrement + 1;
        Vector3[] verticesArray = new Vector3[verticesPerLine * verticesPerLine];

        int index = 0;

        for (int x = 0; x < area; x += lodIncrement)
        {
            for (int y = 0; y < area; y += lodIncrement)
            {
                Vector3 pos = new Vector3(x + ((area - 1) / -2f), noiseValues[x, y], y - ((area - 1) / 2f));
                uvList.Add(new Vector2(x / ((float)area - 1), y / ((float)area - 1)));
                verticesArray[index] = pos;

                if (x < area - 1 && y < area - 1)
                {
                    meshData.AddTriangle(index, index + verticesPerLine + 1, index + verticesPerLine);
                    meshData.AddTriangle(index + verticesPerLine + 1, index, index + 1);
                }

                index++;
            }
        }

        meshData.vertices = verticesArray;
        meshData.uv = uvList.ToArray();
        return meshData;
    }

    public static MeshData GenerateMesh(int area, int LODLevel)
    {
        return GenerateMesh(new float[area, area], area, 3);
    }
}
