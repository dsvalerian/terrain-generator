using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDisplay : MonoBehaviour {
    public const int MODE_NOISE = 0;
    public const int MODE_COLOR = 1;

    // A texture renderer attached to the object on which the textures are displayed.
    public Renderer textureRenderer;
    // Objects to help with creating a mesh.
    public MeshFilter meshFilter;
    public MeshRenderer meshRenderer;

    public void DrawNoiseMap(float[,] noiseValues) {
        int width = noiseValues.GetLength(0);
        int height = noiseValues.GetLength(1);

        // Construct a texture from the noise values.
        TextureGenerator textureGenerator = FindObjectOfType<TextureGenerator>();
        Texture2D noiseTexture = textureGenerator.GenerateNoiseTexture(noiseValues, width, height);

        // Apply the noise texture to the texture renderer and scale it properly.
        textureRenderer.sharedMaterial.mainTexture = noiseTexture;
        textureRenderer.transform.localScale = new Vector3(width, 1, height);
    }

    public void DrawColorMap(Color[,] colorValues) {
        int width = colorValues.GetLength(0);
        int height = colorValues.GetLength(1);

        // Construct a texture from the color values.
        TextureGenerator textureGenerator = FindObjectOfType<TextureGenerator>();
        Texture2D colorTexture = textureGenerator.GenerateColorTexture(colorValues, width, height);

        // Apply the noise texture to the texture renderer and scale it properly.
        textureRenderer.sharedMaterial.mainTexture = colorTexture;
        textureRenderer.transform.localScale = new Vector3(width, 1, height);
    }

    public void DrawMesh(Mesh terrainMesh, Color[,] colorValues) {
        int width = colorValues.GetLength(0);
        int height = colorValues.GetLength(1);

        // Construct a texture from the color values.
        TextureGenerator textureGenerator = FindObjectOfType<TextureGenerator>();
        Texture2D colorTexture = textureGenerator.GenerateColorTexture(colorValues, width, height);

        meshFilter.sharedMesh = terrainMesh;
        meshRenderer.sharedMaterial.mainTexture = colorTexture;
    }
}
