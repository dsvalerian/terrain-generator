using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Noise {
    public static float[,] GeneratorNoiseValues(int width, int height, float noiseScale) {
        // Prevent divide-by-0 errors.
        if (noiseScale <= 0) {
            noiseScale = 0.00001f;
        }

        // A random offset to apply to the perlin noise.
        float offset = Random.Range(-10000f, 10000f);

        // Instantiate a 2D array for the noise map.
        float[,] noiseValues = new float[width, height];

        // Loop through each pixel in the noise map.
        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                // Sample from the current position / noiseScale.
                float sampleX = x / noiseScale;
                float sampleY = y / noiseScale;

                // Create a perlin noise value for the position.
                noiseValues[x, y] = Mathf.PerlinNoise(sampleX + offset, sampleY + offset);
            }
        }

        return noiseValues;
    }
}
