using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public GameObject characterPrefab;
    // Singleton reference to spawned character
    public static GameObject Character { get { return _character; } }
    private static GameObject _character;
    public Vector3 spawnPoint;

    private void Awake() {
        // Immediately spawn the character
        SpawnCharacter();
    }

    // Instantiate the character prefab at the spawn point
    public void SpawnCharacter() {
        _character = Instantiate(characterPrefab, spawnPoint, Quaternion.identity).gameObject;
    }

    private void Update() {
        // Reload the level if the character falls off screen
        if (Character != null && Character.transform.position.y < -10) {
            LevelManager.ReloadLevel();
        }
    }

    // Visualize the spawn point and character overlapBox
    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(spawnPoint, 0.5f);
    }
}
