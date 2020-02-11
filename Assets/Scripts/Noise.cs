using UnityEngine;
using System.Collections;

public static class Noise
{

  public static float[,] GenerateNoiseMap(int mapWidth, int mapHeight, int seed, float scale, int octaves, float persistance, float lacunarity)
  {
    float[,] noiseMap = new float[mapWidth, mapHeight];

    System.Random pseudoRandNum = new System.Random(seed);
    Vector2[] octaveOffsets = new Vector2[octaves];

    for (int i = 0; i < octaves; i++)
    {
      float offsetX = pseudoRandNum.Next(-10000, 10000);
      float offsetY = pseudoRandNum.Next(-10000, 10000);
      octaveOffsets[i] = new Vector2(offsetX, offsetY);
    }

    if (scale <= 0)
    {
      scale = 0.0001f;
    }

    float maxNoiseHeight = float.MinValue;
    float minNoiseHeight = float.MaxValue;

    float halfWidth = mapWidth / 2f;
    float halfHeight = mapHeight / 2f;

    for (int y = 0; y < mapHeight; y++)
    {
      for (int x = 0; x < mapWidth; x++)
      {

        float amplitude = 1;
        float frequency = 1;
        float noiseHeight = 0;


        for (int o = 0; o < octaves; o++)
        {
          float sampleX = (x - halfWidth) / scale * frequency + octaveOffsets[o].x;
          float sampleY = (y - halfHeight) / scale * frequency + octaveOffsets[o].y;

          float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;

          noiseHeight += perlinValue * amplitude;
          amplitude *= persistance;
          frequency *= lacunarity;
        }

        if (noiseHeight > maxNoiseHeight)
        {
          maxNoiseHeight = noiseHeight;
        }
        else if (noiseHeight < minNoiseHeight)
        {
          minNoiseHeight = noiseHeight;
        }

        noiseMap[x, y] = noiseHeight;
      }
    }

    for (int x = 0; x < mapWidth; x++)
    {
      for (int y = 0; y < mapHeight; y++)
      {
        noiseMap[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[x, y]);
      }
    }

    return noiseMap;
  }
}