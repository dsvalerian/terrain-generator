using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDisplay : MonoBehaviour {
    // A texture renderer attached to the object on which the noise map will be displayed.
    public Renderer textureRenderer;

    public void DrawMap(float[,] noiseValues) {
        int width = noiseValues.GetLength(0);
        int height = noiseValues.GetLength(1);

        // Create a 2D texture to visualize the noise values.
        Texture2D noiseTexture = new Texture2D(width, height);

        // Loop through each pixel.
        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                // Assign a color value between black (0) and white (1) that corresponds to the
                // noise value in this position. 
                // Ex. If the noise value is 0.5, the color is halfway between black and white.
                Color color = Color.Lerp(Color.black, Color.white, noiseValues[x, y]);
                noiseTexture.SetPixel(x, y, color);
            }
        }

        // Apply the updates to the texture.
        noiseTexture.Apply();

        // Apply the noise texture to the texture renderer and scale it properly.
        textureRenderer.sharedMaterial.mainTexture = noiseTexture;
        textureRenderer.transform.localScale = new Vector3(width, 1, height);
    }
}
