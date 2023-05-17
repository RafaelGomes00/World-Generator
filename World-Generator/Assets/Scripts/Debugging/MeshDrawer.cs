using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshDrawer : GizmosDrawer
{
    [SerializeField] private float verticeSize;
    [SerializeField] private Color edgeColor;
    [SerializeField] private Color verticeColor;

    private Vector3[,] vertices;
    private Edge[] edges;

    public override void Draw(params object[] parameters)
    {
        foreach(object obj in parameters)
        {
            if (obj is Vector3[])
                vertices = obj as Vector3[,];
            else if (obj is Edge[])
                edges = obj as Edge[];
        }
    }

    protected override void OnDrawGizmos()
    {
        for (int i = 0; i < edges.Length; i++)
        {
            Gizmos.color = edgeColor;
            Gizmos.DrawLine(edges[i].start, edges[i].end);
        }

        foreach (Vector3 vertice in vertices)
        {
            Gizmos.color = verticeColor;
            Gizmos.DrawSphere(vertice, verticeSize);
        }
    }
}
