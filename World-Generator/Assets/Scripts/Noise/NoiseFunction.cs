using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseFunction : ScriptableObject
{
    [SerializeField] protected int seed;
    [SerializeField] protected int octaves;
    [SerializeField] protected float noiseScale;
    [SerializeField] protected float lacunarity;
    [SerializeField] protected float mapHeight;
    [SerializeField] protected Vector2 offset;
    [SerializeField] protected AnimationCurve heightCurve;
    [Range(0, 1)][SerializeField] protected float persistance;
}
