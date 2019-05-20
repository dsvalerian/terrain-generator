using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureGenerator : MonoBehaviour {
    public Texture2D GenerateNoiseTexture(float[,] noiseValues, int width, int height) {
        // Create a 2D texture to visualize the noise values.
        Texture2D noiseTexture = new Texture2D(width, height);

        // Create a set of color values from the noise values.
        Color[,] colorValues = new Color[width, height];

        // Loop through each pixel.
        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                // Assign a color value between black (0) and white (1) that corresponds to the
                // noise value in this position. 
                // Ex. If the noise value is 0.5, the color is halfway between black and white.
                colorValues[x, y] = Color.Lerp(Color.black, Color.white, noiseValues[x, y]);
            }
        }

        return GenerateColorTexture(colorValues, width, height);
    }

    public Texture2D GenerateColorTexture(Color[,] colorValues, int width, int height) {
        // Create a 2D texture to visualize the noise values.
        Texture2D colorTexture = new Texture2D(width, height);

        // Loop through each pixel.
        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                // Assign a color value from the colorValues 2D array.
                colorTexture.SetPixel(x, y, colorValues[x, y]);
            }
        }

        // Remove blurriness from the texture.
        colorTexture.filterMode = FilterMode.Point;
        colorTexture.wrapMode = TextureWrapMode.Clamp;

        // Apply the updates to the texture.
        colorTexture.Apply();

        return colorTexture;
    }
}
