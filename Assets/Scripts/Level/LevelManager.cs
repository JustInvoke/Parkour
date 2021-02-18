using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    // Singleton Reference
    public static LevelManager Instance { get { return _instance; } }
    private static LevelManager _instance;
    public GameObject firstChunk; // First chunk to spawn, useful so that the level starts smoothly
    public int firstChunkCount = 1; // Number of times to repeat the starting chunk
    public GameObject[] levelChunks; // Array of possible chunks to spawn
    public GameObject[] hardChunks; // Array of hard chunks to spawn
    private int totalChunksSpawned = 0; // Total number of chunks ever spawned
    public int hardChunkFreq = 10; // How many chunk spawns until next hard chunk spawn?
    public int minHardChunkFreq = 4; // Minimum amount of chunks that need to spawn until next hard chunk
    public int chunkMilestone = 100; // How many chunks need to spawn until next hardChunkFreq Decrement
    private Queue<LevelChunk> spawnedChunks = new Queue<LevelChunk>(); // Queue of currently spawned chunks
    private LevelChunk lastSpawnedChunk; // Reference to the most recently spawned chunk
    public Camera mainCam; // Reference to the main camera in the scene
    public string mainMenuScene; // Main menu scene to load upon quitting

    private void Awake() {
        //Setup of Monobehaviour Singleton
        if (_instance != this && _instance != null) {
            Destroy(_instance.gameObject);
        }
        _instance = this;

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

        GlobalGameManager.Unpause();
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

    // Spawn a random chunk from one array of chunks
    void SpawnChunk() {
        // Spawn from hard chunks if enough easy chunks have spawned
        if (totalChunksSpawned != 0 && totalChunksSpawned % hardChunkFreq == 0)
        {
            SpawnChunk(hardChunks[Random.Range(0, hardChunks.Length)]);
        }
        else {
            SpawnChunk(levelChunks[Random.Range(0, levelChunks.Length)]);
        }
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
        // Increment the number of total chunks ever spawned
        totalChunksSpawned++;
        // Update the frequency of hard chunk spawns if the chunk milestone is surpassed
        if (totalChunksSpawned != 0 && totalChunksSpawned % chunkMilestone == 0) {
            if (hardChunkFreq > minHardChunkFreq) {
                hardChunkFreq--;
            }
        }
    }

    // Reload the current level
    public static void ReloadLevel() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GlobalGameManager.Unpause();
    }

    // Return to the main menu
    public static void LoadMainMenu() {
        if (Instance == null) { return; }
        SceneManager.LoadScene(Instance.mainMenuScene);
        GlobalGameManager.Unpause();
    }

    // Clean up singleton reference
    private void OnDestroy() {
        if (_instance == this) {
            _instance = null;
        }
    }
}
