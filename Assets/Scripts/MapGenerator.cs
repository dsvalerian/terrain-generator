using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour {
    private const int SEED_RANGE = 1000000;
    private const float NEARLY_ONE = 1.0001f;
    // LOD is based on size - 1. 240 is nice because it divides evenly by 2, 4, 6, 8, 10, and 12.
    public const int MAP_CHUNK_SIZE = 241;

    public const float DEFAULT_NOISE_SCALE = 50f;
    public const int DEFAULT_OCTAVES = 5;
    public const float DEFAULT_OCTAVE_FREQUENCY_INCREASE = 2f;
    public const float DEFAULT_OCTAVE_AMPLITUDE_DECREASE = 0.5f;
    public const float DEFAULT_MESH_HEIGHT_SCALE = 65f;
    public const int DEFAULT_LOD = 0;

    public enum DrawMode {Noise, Color, Mesh};
    public DrawMode drawMode;

    [Range(1.1f, 200f)]
    public float noiseScale = DEFAULT_NOISE_SCALE;
    [Range(1, 30)]
    public int octaves = DEFAULT_OCTAVES;
    [Range(1, 15)]
    public float octaveFrequencyIncrease = DEFAULT_OCTAVE_FREQUENCY_INCREASE;
    [Range(0, 1)]
    public float octaveAmplitudeDecrease = DEFAULT_OCTAVE_AMPLITUDE_DECREASE;
    public int seed;

    [Range(0, 500)]
    public float meshHeightScale = DEFAULT_MESH_HEIGHT_SCALE;
    public AnimationCurve meshHeightCurve;

    public TerrainType[] terrainTypes;

    [Range(0, 6)]
    public int LOD = DEFAULT_LOD;

    /*
        Generates a noise map and a color map based on the noise map.
    */
    public void GenerateMap(bool randomize) {
        // Get a random seed for the map.
        if (randomize) {
            seed = Random.Range(-SEED_RANGE, SEED_RANGE);
        }

        // Generate the noise values for the map.
        float[,] noiseValues = Noise.GenerateNoiseValues(MAP_CHUNK_SIZE, MAP_CHUNK_SIZE, noiseScale,
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
                MeshGenerator.GenerateTerrainMesh(noiseValues, meshHeightScale, meshHeightCurve, LOD), 
                colorValues);
        }
    }

    /*
    Get the color values for the terrain heights that correspond to the noise values.
    */
    private Color[,] GetTerrainColorValues(float[,] noiseValues) {
        Color[,] colorValues = new Color[MAP_CHUNK_SIZE, MAP_CHUNK_SIZE];
        // Loop through the map.
        for (int y = 0; y < MAP_CHUNK_SIZE; y++) {
            for (int x = 0; x < MAP_CHUNK_SIZE; x++) {
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

    public void ResetValues() {
        noiseScale = DEFAULT_NOISE_SCALE;
        octaves = DEFAULT_OCTAVES;
        octaveFrequencyIncrease = DEFAULT_OCTAVE_FREQUENCY_INCREASE;
        octaveAmplitudeDecrease = DEFAULT_OCTAVE_AMPLITUDE_DECREASE;
        meshHeightScale = DEFAULT_MESH_HEIGHT_SCALE;
        LOD = DEFAULT_LOD;
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