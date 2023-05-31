using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Noise : MonoBehaviour
{
    // [SerializeField] protected int width;
    // [SerializeField] protected int height;
    [SerializeField] protected int seed;
    [SerializeField] protected int octaves;
    [SerializeField] protected float noiseScale;
    [SerializeField] protected float lacunarity;
    [SerializeField] protected float mapHeight;
    [SerializeField] protected Vector2 offset;
    [SerializeField] protected AnimationCurve heightCurve;
    [Range(0, 1)][SerializeField] protected float persistance;

    public abstract float[,] GenerateNoise(int width, int height, Vector2 center);
}
