using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessTerrainController : MonoBehaviour
{
    [SerializeField] private TerrainGenerator terrainGenerator;
    [SerializeField] private float maxViewDistance = 300;
    [SerializeField] private Transform viewrTransform;
    public Vector2 viewerPositon;
    int chunkSize;
    int chunksVisible;
    Dictionary<Vector2, TerrainChunk> terrainChunkDictionary = new Dictionary<Vector2, TerrainChunk>();
    List<TerrainChunk> terrainChunksVisible = new List<TerrainChunk>();
    private void Start()
    {
        chunkSize = terrainGenerator.GetChunkSize() - 1;
        chunksVisible = Mathf.RoundToInt(maxViewDistance / chunkSize);
    }
    private void Update()
    {
        viewerPositon = new Vector2(viewrTransform.position.x, viewrTransform.position.z);
        UpdateVisibleChunks();
    }
    private void UpdateVisibleChunks()
    {
        for (int i = 0; i < terrainChunksVisible.Count; i++)
        {
            terrainChunksVisible[i].SetVisible(false);
        }
        terrainChunksVisible.Clear();
        int currentChunkCoordX = Mathf.RoundToInt(viewerPositon.x / chunkSize);
        int currentChunkCoordY = Mathf.RoundToInt(viewerPositon.y / chunkSize);
        for (int xOffset = -chunksVisible; xOffset <= chunksVisible; xOffset++)
        {
            for (int yOffset = -chunksVisible; yOffset <= chunksVisible; yOffset++)
            {
                Vector2 viewedChunk = new Vector2(currentChunkCoordX + xOffset, currentChunkCoordY + yOffset);
                if (terrainChunkDictionary.ContainsKey(viewedChunk))
                {
                    terrainChunkDictionary[viewedChunk].UpdateTerrainChunk(viewerPositon, maxViewDistance);
                    if (terrainChunkDictionary[viewedChunk].IsVisible())
                    {
                        terrainChunksVisible.Add(terrainChunkDictionary[viewedChunk]);
                    }
                }
                else
                    terrainChunkDictionary.Add(viewedChunk, new TerrainChunk(viewedChunk, chunkSize, terrainGenerator, this.transform));
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(viewrTransform.position, maxViewDistance);
    }
}
