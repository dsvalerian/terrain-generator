using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour {
    public int mapWidth;
    public int mapHeight;
    public float noiseScale;
    public int octaves;
    public float octaveFrequencyIncrease;
    public float octaveAmplitudeDecrease;

    public void GenerateMap() {
        // Prevent Unity from crashing on empty textures.
        if (mapWidth < 1) {
            mapWidth = 1;
        }

        if (mapHeight < 1) {
            mapHeight = 1;
        }

        // Generate the noise values for the map.
        float[,] noiseValues = Noise.GeneratorNoiseValues(mapWidth, mapHeight, noiseScale,
            octaves, octaveFrequencyIncrease, octaveAmplitudeDecrease);

        // Process the noise values into a 2D texture.
        MapDisplay mapDisplay = FindObjectOfType<MapDisplay>();
        mapDisplay.DrawMap(noiseValues);
    }
}
