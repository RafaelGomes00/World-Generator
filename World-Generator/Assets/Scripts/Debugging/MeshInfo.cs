using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct MeshInfo
{
    public Edge[] edges;
    public Vector3[,] vertices;

    public MeshInfo(Edge[] edges, Vector3[,] vertices)
    {
        this.edges = edges;
        this.vertices = vertices;
    }
}
