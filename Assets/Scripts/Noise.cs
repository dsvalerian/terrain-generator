using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Noise {
    private const float OFFSET_RANGE = 100000f;
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
    public static float[,] GeneratorNoiseValues(int width, int height, float noiseScale, 
            int octaves, float octaveFrequencyIncrease, float octaveAmplitudeDecrease) {
        // Prevent divide-by-0 errors.
        if (noiseScale <= 0) {
            noiseScale = NEARLY_ZERO;
        }

        // A random offset to apply to the perlin noise.
        float offset = Random.Range(-OFFSET_RANGE, OFFSET_RANGE);

        // Instantiate a 2D array for the noise map.
        float[,] noiseValues = new float[width, height];

        // Keep track of the range of noise values so we can normalize it to [0, 1].
        float[] noiseRange = new float[] {float.MaxValue, float.MinValue};

        // Loop through each value in the noise map.
        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                // Keep track of the frequency and amplitude for each additional octave.
                float currentFrequency = 1;
                float currentAmplitude = 1;
                float noiseValue = 0;

                // Calculate noise influence for each octave.
                for (int octave = 0; octave < octaves; octave++) {
                    float sampleX = x / noiseScale * currentFrequency;
                    float sampleY = y / noiseScale * currentFrequency;

                    // Add this octave's height influence to the noise value.
                    noiseValue += Mathf.PerlinNoise(sampleX + offset, sampleY + offset) * 2 - 1;

                    // Update the amplitude/frequency for the next octave.
                    currentAmplitude *= octaveAmplitudeDecrease;
                    currentFrequency *= octaveFrequencyIncrease;
                }

                // Finally, assign the final noise value to the map.
                noiseValues[x, y] = noiseValue;

                // Update the noise range if noiseValue falls outside of it.
                if (noiseValue > noiseRange[1]) {
                    noiseRange[1] = noiseValue;
                }
                else if (noiseValue < noiseRange[0]) {
                    noiseRange[0] = noiseValue;
                }
            }
        }

        return NormalizeValues(noiseValues, noiseRange);
    }

    /*
        Normalize values to a range of [0, 1].

        values: A 2D array of values
        range: The range [min, max] of the given values.
    */
    private static float[,] NormalizeValues(float [,] values, float[] range) {
        int width = values.GetLength(0);
        int height = values.GetLength(1);

        float[,] normalized = new float[width, height];

        // Loop through each value.
        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                // Calculate the distance (between 0 and 1) that the noise value is between the min
                // and max of the noise range. 
                // Ex. range = [-10, 5], value = 0, then the distance = 0.6666666
                normalized[x, y] = Mathf.InverseLerp(range[0], range[1], values[x, y]);
            }
        }

        return normalized;
    }
}
