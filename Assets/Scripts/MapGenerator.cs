using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour {
    private const int SEED_RANGE = 1000000;
    private const float NEARLY_ONE = 1.0001f;

    public int mapWidth;
    public int mapHeight;
    public float noiseScale;
    public int octaves;
    public float octaveFrequencyIncrease;
    public float octaveAmplitudeDecrease;
    public int seed;

    public TerrainType[] terrainTypes;

    /*
        Generates a noise map and a color map based on the noise map.
    */
    public void GenerateMap(bool randomize) {
        // Clamp all values to prevent weird things from happening/crashing.
        if (mapWidth < 1) mapWidth = 1;
        if (mapHeight < 1) mapHeight = 1;
        if (noiseScale <= 1) noiseScale = NEARLY_ONE;
        if (octaves < 1) octaves = 1;
        if (octaveFrequencyIncrease < 1) octaveFrequencyIncrease = 1;
        if (octaveAmplitudeDecrease < 0) octaveAmplitudeDecrease = 0;
        if (octaveAmplitudeDecrease > 1) octaveAmplitudeDecrease = 1;

        // Get a random seed for the map.
        if (randomize) {
            seed = Random.Range(-SEED_RANGE, SEED_RANGE);
        }

        // Generate the noise values for the map.
        float[,] noiseValues = Noise.GenerateNoiseValues(mapWidth, mapHeight, noiseScale,
            octaves, octaveFrequencyIncrease, octaveAmplitudeDecrease, seed);

        // Process the noise values into a 2D texture.
        MapDisplay mapDisplay = FindObjectOfType<MapDisplay>();
        mapDisplay.DrawNoiseMap(noiseValues);

        // Generate and process color values into a 2D texture.
        Color[,] colorValues = GetTerrainColorValues(noiseValues);
        mapDisplay.DrawColorMap(colorValues);
    }

    /*
    Get the color values for the terrain heights that correspond to the noise values.
    */
    private Color[,] GetTerrainColorValues(float[,] noiseValues) {
        Color[,] colorValues = new Color[mapWidth, mapHeight];
        // Loop through the map.
        for (int y = 0; y < mapHeight; y++) {
            for (int x = 0; x < mapWidth; x++) {
                Color color = Color.white;

                // Loop through each terrain type.
                foreach (TerrainType terrain in terrainTypes) {
                    // Check if the height corresponds to the noise.
                    if (noiseValues[x, y] < terrain.heightRange.y &&
                            noiseValues[x, y] > terrain.heightRange.x) {
                        color = terrain.color;
                        break;
                    }
                }

                // Finally, assign the color to the color map.
                colorValues[x, y] = color;
            }
        }

        return colorValues;
    }
}

/*
    A struct to store information about certain terrain types on the map.
*/
[System.Serializable]
public struct TerrainType {
    public string label;
    public Vector2 heightRange;
    public Color color;
}