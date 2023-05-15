using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Noise : MonoBehaviour
{
    // [SerializeField] protected int width;
    // [SerializeField] protected int height;
    [SerializeField] protected int noiseScale;
    [SerializeField] protected float noiseHeight;

    public abstract float[,] GenerateNoise(int width, int height);
}
