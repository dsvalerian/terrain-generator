using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

// This custom editor is for a MapGenerator class.
[CustomEditor(typeof(MapGenerator))]
public class MapGeneratorEditor : Editor {
    public override void OnInspectorGUI() {
        // Get the map generator.
        MapGenerator mapGenerator = (MapGenerator)target;

        // Draw a regular inspector window.
        DrawDefaultInspector();

        // Add a generate button that generates a new noise map.
        if (GUILayout.Button("Generate Noise Map")) {
            mapGenerator.GenerateMap();
        }
    }
}
