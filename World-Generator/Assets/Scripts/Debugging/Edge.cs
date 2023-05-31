using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edge
{
    public Vector3 start;
    public Vector3 end;

    public Edge(Vector3 start, Vector3 end)
    {
        this.start = start;
        this.end = end;
    }
}
