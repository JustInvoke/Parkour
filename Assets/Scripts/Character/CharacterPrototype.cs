using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPrototype : MonoBehaviour
{
    private Rigidbody2D rb;
    public Collider2D standCol; // Collider used while standing
    public Collider2D crouchCol; // Collider used while crouching
    private SpriteRenderer rend;
    public Sprite standSprite; // Sprite used while standing
    public Sprite crouchSprite; // Sprite used while crouching
    public float minSpeed = 1.0f; // Minimum allowed movement speed
    public float maxSpeed = 2.0f; // Maximum allowed movement speed
    public float accel = 1.0f;
    public float jumpForce = 1.0f;
    public KeyCode rightInput = KeyCode.RightArrow; // Max speed movement
    public KeyCode leftInput = KeyCode.LeftArrow; // Min speed movement
    public KeyCode jumpInput = KeyCode.UpArrow;
    public KeyCode crouchInput = KeyCode.DownArrow;

    private void Start() {
        // Cache component references
        rb = GetComponent<Rigidbody2D>();
        rend = GetComponent<SpriteRenderer>();
    }

    private void Update() {
        // Jump action
        if (Input.GetKeyDown(jumpInput)) {
            // Reset vertical speed so it doesn't reduce jump height
            if (rb.velocity.y < 0) {
                rb.velocity = new Vector2(rb.velocity.x, 0.0f);
            }
            // Add jump force
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    private void FixedUpdate() {
        // Convert key inputs to single float ranging from 0 to 1
        float moveInput = (Input.GetKey(rightInput) ? 1.0f : 0.5f) + (Input.GetKey(leftInput) ? -0.5f : 0.0f);
        // Interpolate between min and max speed
        float targetSpeed = Mathf.Lerp(minSpeed, maxSpeed, moveInput);
        // Add movement force
        rb.AddForce(Vector2.right * (targetSpeed - rb.velocity.x) * accel, ForceMode2D.Force);

        // Crouch logic with colliders and sprites
        bool crouching = Input.GetKey(crouchInput);
        standCol.enabled = !crouching;
        crouchCol.enabled = crouching;
        rend.sprite = crouching ? crouchSprite : standSprite;
    }
}
