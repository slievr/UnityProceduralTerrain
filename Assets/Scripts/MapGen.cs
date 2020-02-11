using System.Collections;
using UnityEngine;

public class MapGen : MonoBehaviour
{

  public enum DrawMode { NoiseMap, ColorMap };
  public DrawMode drawMode;
  public int mapWidth;
  public int mapHeight;
  public float noiseScale;
  public int octaves;
  public float persistance;
  public float lacunarity;
  public bool autoUpdate;
  public int seed;

  public TerrainType[] regions;
  public void GenerateMap()
  {
    float[,] noiseMap = Noise.GenerateNoiseMap(mapWidth, mapHeight, seed, noiseScale, octaves, persistance, lacunarity);

    Color[] colorMap = new Color[mapWidth * mapHeight];
    for (int x = 0; x < mapWidth; x++)
    {
      for (int y = 0; y < mapHeight; y++)
      {
        float currentHeight = noiseMap[x, y];
        foreach (TerrainType region in regions)
        {
          if (currentHeight <= region.height)
          {
            colorMap[y * mapWidth + x] = region.colour;
            break;
          }
        }
      }
    }

    MapDisplay display = FindObjectOfType<MapDisplay>();
    if (drawMode == DrawMode.NoiseMap)
    {
      display.DrawTexture(TextureGenerator.TextureFromNoise(noiseMap));
    }

    if (drawMode == DrawMode.ColorMap)
    {
      display.DrawTexture(TextureGenerator.TextureFromColour(colorMap, mapWidth, mapHeight));
    }
  }

  void OnValidate()
  {
    if (mapWidth < 1)
    {
      mapWidth = 1;
    }
    if (mapHeight < 1)
    {
      mapHeight = 1;
    }
    if (lacunarity < 1)
    {
      lacunarity = 1;
    }
    if (octaves < 0)
    {
      octaves = 0;
    }
  }
}
[System.Serializable]
public struct TerrainType
{
  public string name;
  public float height;
  public Color colour;
}