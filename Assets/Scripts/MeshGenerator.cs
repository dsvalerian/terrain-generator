using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MeshGenerator {
    public static Mesh GenerateTerrainMesh(float[,] heightMap, float heightScale, 
            AnimationCurve heightCurve, int LOD) {
        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);

        // The x and y position for the top left vertex on the mesh. In 3D, the represent the x and
        // z values. This is used so we can make sure the center vertex of the heightMap will be
        // at the center of the 3D world.
        float topLeftX = (width - 1) / -2f;
        float topLeftY = (height - 1) / 2f;

        // Based on the LOD, the increment in which vertices are chosen.
        int vertexSkip = 1;
        if (LOD > 0) {
            vertexSkip = LOD * 2;
        }

        int verticesPerLine = (width - 1) / vertexSkip + 1;

        // Log some info about vertex decrease
        int totalVertices = verticesPerLine * verticesPerLine;
        float vertexDecrease = (1f / ((width * height) / totalVertices)) * 100;
        Debug.Log("Vertices per line: " + verticesPerLine + "; Total: " + totalVertices + 
            "; Vertex Decrease: " + vertexDecrease + "%");

        // We need a TerrainData variable to store vertices and triangles for our terrain mesh.
        TerrainData terrainData = new TerrainData(verticesPerLine, verticesPerLine);

        // Loop through heights.
        int vertexIndex = 0;
        for (int y = 0; y < height; y += vertexSkip) {
            for (int x = 0; x < width; x += vertexSkip) {
                // Push a vertex to the end of the array, with the height pulled from the heightmap.
                terrainData.vertices[vertexIndex] = new Vector3(topLeftX + x, 
                    heightCurve.Evaluate(heightMap[x, y]) * heightScale, topLeftY - y);

                // Push a uv mapping to the end of the array.
                terrainData.uvData[vertexIndex] = new Vector2(x / (float)width, y / (float)height);

                // Squares originate from the top left corners, so we ignore the vertices lining the
                // right side and bottom of the vertex map.
                if (x < width - 1 && y < height - 1) {
                    // Add the bottom left triangle of the square to the mesh.
                    terrainData.AddTriangle(vertexIndex, vertexIndex + verticesPerLine + 1, 
                        vertexIndex + verticesPerLine);
                    // Add the top right triangle of the square to the mesh.
                    terrainData.AddTriangle(vertexIndex + verticesPerLine + 1, vertexIndex, vertexIndex + 1);
                }

                vertexIndex++;
            }
        }

        // Instead of directly creating and returning a mesh, we only return the mesh data. The 
        // reason for this is because Unity does not allow for creating meshes within a thread. 
        // Therefore, we must calculate the mesh data on the thread and create the mesh itself
        // outside of it.
        return terrainData.ConstructMesh(); // this currently DOES return the mesh.
    }
}

public class TerrainData {
    // The total number of vertices that are on the surface of the terrain mesh. A 2x2 grid of 
    // squares has 9 vertices.
    public Vector3[] vertices;
    // Every each int represents 1 vertex, and every 3 vertices represent one triangle on the mesh. 
    // A 2x2 grid of squares has 8 triangles. Each triangle is represented by 3 vertices, so there
    // are 24 vertices.
    public int[] triangles;
    // The index that points to the end of the triangles array.
    private int triangleIndex;
    // UV data for mapping 2D textures onto the 3D mesh. One for each vertex.
    public Vector2[] uvData;

    public TerrainData(int width, int height) {
        vertices = new Vector3[width * height];
        triangles = new int[(width - 1) * (height - 1) * 6];
        triangleIndex = 0;
        uvData = new Vector2[width * height];
    }

    public void AddTriangle(int vertexA, int vertexB, int vertexC) {
        triangles[triangleIndex] = vertexA;
        triangles[triangleIndex + 1] = vertexB;
        triangles[triangleIndex + 2] = vertexC;

        // Point to the new end of the triangles array.
        triangleIndex += 3;
    }

    public Mesh ConstructMesh() {
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvData;
        // Recalculate the normals to remove lighting artifacts.
        mesh.RecalculateNormals();

        return mesh;
    }
}
