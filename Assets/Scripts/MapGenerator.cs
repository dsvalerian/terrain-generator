using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour {
    private const int SEED_RANGE = 1000000;

    public int mapWidth;
    public int mapHeight;
    public float noiseScale;
    public int octaves;
    public float octaveFrequencyIncrease;
    public float octaveAmplitudeDecrease;
    public int seed;

    public TerrainType[] terrainsTypes;

    public void GenerateMap(bool randomize) {
        // Prevent Unity from crashing on empty textures.
        if (mapWidth < 1) {
            mapWidth = 1;
        }

        if (mapHeight < 1) {
            mapHeight = 1;
        }

        // Get a random seed for the map.
        if (randomize) {
            seed = Random.Range(-SEED_RANGE, SEED_RANGE);
        }

        // Generate the noise values for the map.
        float[,] noiseValues = Noise.GenerateNoiseValues(mapWidth, mapHeight, noiseScale,
            octaves, octaveFrequencyIncrease, octaveAmplitudeDecrease, seed);

        // Process the noise values into a 2D texture.
        MapDisplay mapDisplay = FindObjectOfType<MapDisplay>();
        mapDisplay.DrawMap(noiseValues);
    }
}

/*
    A struct to store information about certain terrain types on the map.
*/
[System.Serializable]
public struct TerrainType {
    public string label;
    public float height;
    public Color color;
}
