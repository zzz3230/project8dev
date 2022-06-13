using UnityEngine;

public class TerrainGeneratorTest : MonoBehaviour
{
    public WorldGenerator worldGenerator;
    public Terrain terrain;

    public Vector2 chunkPos;
    public int chunkSize;
    public int octaves;
    public string seed;
    public float frequency;
    public float persistence;
    public float lacunarity;
    public Vector2 offset;

    float lastH;

    void Update()
    {
        var h = worldGenerator.GenerateNoise(chunkPos, chunkSize, octaves, seed, frequency, persistence, lacunarity, offset);
        float sum = 0;
        for (int i = 0; i < h.GetLength(0); i++)
        {
            for (int j = 0; j < h.GetLength(1); j++)
            {
                sum += h[i, j];
            }
        }

        if (lastH != sum)
        {
            Debug.Log("updated");
            terrain.terrainData.SetHeights(0, 0, h);
            lastH = sum;
        }

        //Debug.Log(sum);
    }
}
