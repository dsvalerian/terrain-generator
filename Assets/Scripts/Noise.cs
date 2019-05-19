using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Noise {
    private const int OFFSET_RANGE = 100000;
    private const float NEARLY_ZERO = 0.0001f;

    /*  
        width, height: control the number of noise values to be generated
        noiseScale: size of the noise area to grab values from
        octaves: number of "levels" of perlin noise to be used together (ex. 3 for mountains, 
            boulders, and small rocks)
        octaveFrequencyIncrease: the increase in noise frequency for additional octaves (ex. small
            rocks require a higher noise frequency since there are more of them than large mountains)
        octaveAmplitudeDecrease: the decrease in noise amplitude for addition octaves (ex. small 
            rocks require a lower noise amplitude since they are smaller than large mountains)
    
        Octaves are useful because they allow for noise values to represent fine details. Finer
        details should be more frequent and have a smaller affect on the height. This helps create 
        terrain that will take into account smaller imperfections due to rocks, erosion, etc.
    */
    public static float[,] GenerateNoiseValues(int width, int height, float noiseScale, 
            int octaves, float octaveFrequencyIncrease, float octaveAmplitudeDecrease, int seed) {
        // Prevent divide-by-0 errors.
        if (noiseScale <= 0) {
            noiseScale = NEARLY_ZERO;
        }

        // Get new offsets for getting the noise values for each octave.
        Vector2[] offsets = RandomizeOffsets(octaves, seed);

        // Instantiate a 2D array for the noise map.
        float[,] noiseValues = new float[width, height];

        // Keep track of the range of noise values so we can normalize it to [0, 1].
        Vector2 noiseRange = new Vector2(float.MaxValue, float.MinValue);

        // Loop through each value in the noise map.
        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                // Keep track of the frequency and amplitude for each additional octave.
                float currentFrequency = 1;
                float currentAmplitude = 1;
                float noiseValue = 0;

                // Calculate noise influence for each octave.
                for (int octave = 0; octave < octaves; octave++) {
                    float sampleX = x / noiseScale * currentFrequency + offsets[octave].x;
                    float sampleY = y / noiseScale * currentFrequency + offsets[octave].y;

                    // Add this octave's height influence to the noise value.
                    noiseValue += (Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1) * currentAmplitude;

                    // Update the amplitude/frequency for the next octave.
                    currentAmplitude *= octaveAmplitudeDecrease;
                    currentFrequency *= octaveFrequencyIncrease;
                }

                // Finally, assign the final noise value to the map.
                noiseValues[x, y] = noiseValue;

                // Update the noise range if noiseValue falls outside of it.
                if (noiseValue > noiseRange.y) {
                    noiseRange.y = noiseValue;
                }
                else if (noiseValue < noiseRange.x) {
                    noiseRange.x = noiseValue;
                }
            }
        }

        return NormalizeValues(noiseValues, noiseRange);
    }

    /*
        Get an array of Vector2s which store the x and y offsets for each octave.
    */
    private static Vector2[] RandomizeOffsets(int octaves, int seed) {
        // Create a new randomizer from the seed.
        System.Random random = new System.Random(seed);

        // Randomize offsets for each octave.
        Vector2[] offsets = new Vector2[octaves];
        for (int octave = 0; octave < octaves; octave++) {
            float xOffset = random.Next(-OFFSET_RANGE, OFFSET_RANGE);
            float yOffset = random.Next(-OFFSET_RANGE, OFFSET_RANGE);

            offsets[octave] = new Vector2(xOffset, yOffset);
        }

        return offsets;
    }

    /*
        Normalize values to a range of [0, 1].

        values: A 2D array of values
        range: The range [min, max] of the given values.
    */
    private static float[,] NormalizeValues(float [,] values, Vector2 range) {
        int width = values.GetLength(0);
        int height = values.GetLength(1);

        float[,] normalized = new float[width, height];

        // Loop through each value.
        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                // Calculate the distance (between 0 and 1) that the noise value is between the min
                // and max of the noise range. 
                // Ex. range = [-10, 5], value = 0, then the distance = 0.6666666
                normalized[x, y] = Mathf.InverseLerp(range.x, range.y, values[x, y]);
            }
        }

        return normalized;
    }
}
