using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public GameObject firstChunk; // First chunk to spawn, useful so that the level starts smoothly
    public int firstChunkCount = 1; // Number of times to repeat the starting chunk
    public GameObject[] levelChunks; // Array of possible chunks to spawn
    private Queue<LevelChunk> spawnedChunks = new Queue<LevelChunk>(); // Queue of currently spawned chunks
    private LevelChunk lastSpawnedChunk; // Reference to the most recently spawned chunk
    public Camera mainCam; // Reference to the main camera in the scene

    private void Awake() {
        // Spawn starting chunk(s)
        if (firstChunkCount > 0) {
            for (int i = 0; i < firstChunkCount; i++) {
                SpawnChunk(firstChunk);
            }
        }
        else {
            // Spawn any chunk if no first chunks to spawn
            SpawnChunk();
        }
    }

    private void Update() {
        // Reload the level when 'R' is pressed (for convenience)
        if (Application.isEditor && Input.GetKeyDown(KeyCode.R)) {
            ReloadLevel();
        }

        if (mainCam != null) {
            if (lastSpawnedChunk != null) {
                // This loop will spawn new chunks to the right if there is any visible empty space
                while (lastSpawnedChunk.RightEnd().x < mainCam.ViewportToWorldPoint(Vector3.right).x) {
                    SpawnChunk();
                }
            }

            // Destroy any old chunks off screen to the left
            if (spawnedChunks.Count > 0) {
                if (spawnedChunks.Peek().RightEnd().x < mainCam.ViewportToWorldPoint(Vector3.zero).x) {
                    Destroy(spawnedChunks.Dequeue().gameObject);
                }
            }
        }
    }

    // Spawn a random chunk from the array of chunks
    void SpawnChunk() {
        SpawnChunk(levelChunks[Random.Range(0, levelChunks.Length)]);
    }

    // Spawn a specific chunk
    void SpawnChunk(GameObject chunk) {
        Vector3 spawnPos = Vector3.zero;
        if (lastSpawnedChunk != null) {
            // Spawn following most recently spawned chunk
            spawnPos = lastSpawnedChunk.RightEnd();
        }
        else if (lastSpawnedChunk == null && mainCam != null) {
            // Default spawn point
            spawnPos = mainCam.ViewportToWorldPoint(Vector3.zero);
        }
        // Spawn the chunks and add it to the queue
        lastSpawnedChunk = Instantiate(chunk, new Vector3(spawnPos.x, spawnPos.y, 0.0f), Quaternion.identity).GetComponent<LevelChunk>();
        spawnedChunks.Enqueue(lastSpawnedChunk);
    }

    // Reload the current level
    public static void ReloadLevel() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
