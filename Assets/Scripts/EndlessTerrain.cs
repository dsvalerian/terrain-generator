using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessTerrain : MonoBehaviour {
    private int chunkSize;
    private int numVisibleChunks;

    public Transform player;
    public static Vector2 playerPosition;
    public float renderDistance = 450f;

    private void Start() {
        // Remember, the actual size of our map is one less because of the LOD system.
        chunkSize = MapGenerator.MAP_CHUNK_SIZE - 1;
        numVisibleChunks = Mathf.RoundToInt(renderDistance / chunkSize);
    }
}
