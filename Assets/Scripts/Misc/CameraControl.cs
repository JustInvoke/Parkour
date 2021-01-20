using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Transform target; // Target (character) to follow
    public float offset; // x-offset to sit at relative to the character

    private void Update() {
        if (target != null) {
            // Position relative to the character
            transform.position = new Vector3(target.position.x + offset, 0.0f, -10f);
        }
        else if (CharacterManager.Character != null) {
            // Find target in character singleton
            target = CharacterManager.Character.transform;
        }
    }
}
