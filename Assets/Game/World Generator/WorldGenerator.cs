using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    public float[,] GenerateNoise(Vector2 chunkPos, int chunkSize, int octaves, string seed, float frequency, float persistence, float lacunarity, Vector2 offset)
    {
        float[,] noiseMap = new float[chunkSize, chunkSize];

        for (int x = 0; x < chunkSize; x++)
        {
            for (int y = 0; y < chunkSize; y++)
            {
                float sample = Noise.Sum(Noise.methods[1][1], new Vector3(x, y), frequency, octaves, lacunarity, persistence);

                noiseMap[x, y] = Mathf.Clamp01(sample);

            }
        }

        return noiseMap;
    }
}
