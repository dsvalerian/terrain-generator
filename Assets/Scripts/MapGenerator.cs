using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour {
    private const int SEED_RANGE = 1000000;
    private const float NEARLY_ONE = 1.0001f;

    public enum DrawMode {Noise, Color, Mesh};
    public DrawMode drawMode;

    public int mapWidth = 128;
    public int mapHeight = 128;
    [Range(1, 200)]
    public float noiseScale = 50f;
    [Range(1, 30)]
    public int octaves = 5;
    [Range(1, 15)]
    public float octaveFrequencyIncrease = 2f;
    [Range(0, 1)]
    public float octaveAmplitudeDecrease = 0.5f;
    public int seed;

    [Range(0, 500)]
    public float meshHeightScale = 200f;
    public AnimationCurve meshHeightCurve;

    public TerrainType[] terrainTypes;

    /*
        Generates a noise map and a color map based on the noise map.
    */
    public void GenerateMap(bool randomize) {
        // Clamp values to prevent weird things from happening/crashing.
        if (mapWidth < 1) mapWidth = 1;
        if (mapHeight < 1) mapHeight = 1;

        // Get a random seed for the map.
        if (randomize) {
            seed = Random.Range(-SEED_RANGE, SEED_RANGE);
        }

        // Generate the noise values for the map.
        float[,] noiseValues = Noise.GenerateNoiseValues(mapWidth, mapHeight, noiseScale,
            octaves, octaveFrequencyIncrease, octaveAmplitudeDecrease, seed);

        // Process the noise values based on the draw mode.
        MapDisplay mapDisplay = FindObjectOfType<MapDisplay>();

        if (drawMode == DrawMode.Noise) {
            // Process the noise values into a 2D texture.
            mapDisplay.DrawNoiseMap(noiseValues);
        }
        else if (drawMode == DrawMode.Color) {
            // Generate and process color values into a 2D texture.
            Color[,] colorValues = GetTerrainColorValues(noiseValues);
            mapDisplay.DrawColorMap(colorValues);
        }
        else if (drawMode == DrawMode.Mesh) {
            Color[,] colorValues = GetTerrainColorValues(noiseValues);

            // Generate and process heights onto a 3D mesh.
            mapDisplay.DrawMesh(
                MeshGenerator.GenerateTerrainMesh(noiseValues, meshHeightScale, meshHeightCurve), 
                colorValues);
        }
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
                    if (noiseValues[x, y] < terrain.height) {
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
    [Range(-0.1f, 1.1f)]
    public float height;
    public Color color;
}