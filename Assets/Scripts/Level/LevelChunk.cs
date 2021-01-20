using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelChunk : MonoBehaviour
{
    public float width = 1.0f;

    // Calculates the point at the bottom-right corner
    public Vector3 RightEnd() {
        return transform.TransformPoint(Vector3.right * width);
    }

    // Visualize width
    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position, RightEnd());
    }
}
