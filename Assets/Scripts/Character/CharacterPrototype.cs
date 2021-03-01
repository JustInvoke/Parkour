using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPrototype : MonoBehaviour
{
    private Rigidbody2D rb;
    public Collider2D standCol; // Collider used while standing
    public Collider2D crouchCol; // Collider used while crouching
    public Vector3 hitBoxPosition;
    public Vector3 hitBoxScale;
    public Vector3 hitBoxOffset;
    public LayerMask levelMaskLayer;
    private SpriteRenderer rend;
    public Sprite standSprite; // Sprite used while standing
    public Sprite crouchSprite; // Sprite used while crouching
    public float minSpeed = 5.0f; // Minimum allowed movement speed
    public float maxSpeed = 18.0f; // Maximum allowed movement speed
    public float accel = 5.0f;
    public float jumpForce = 14.0f;
    public KeyCode rightInput = KeyCode.RightArrow; // Max speed movement
    public KeyCode leftInput = KeyCode.LeftArrow; // Min speed movement
    public KeyCode jumpInput = KeyCode.UpArrow;
    public KeyCode crouchInput = KeyCode.DownArrow;
    public Animator animator;
    public static int jumpCount = 0;
    public static int crouchCount = 0;
    private void Start() {
        // Cache component references
        rb = GetComponent<Rigidbody2D>();
        rend = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    private void Update() {
        // Jump action
        if (Input.GetKeyDown(jumpInput)) {
            animator.SetBool("isCrouching", false);
            animator.SetBool("isJumping", true);
            jumpCount++;
            // Reset vertical speed so it doesn't reduce jump height
            if (rb.velocity.y < 0) {
                rb.velocity = new Vector2(rb.velocity.x, 0.0f);
            }
            // Add jump force
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
        // Crouch action
        else if (Input.GetKeyDown(crouchInput)) {
            animator.SetBool("isCrouching", true);
            animator.SetBool("isJumping", false);
            crouchCount++;
        }
        else {
            animator.SetBool("isCrouching", false);
            animator.SetBool("isJumping", false);
        }
    }

    private void FixedUpdate() {
        // Reload the level if the character touches a wall
        hitBoxPosition = transform.position + hitBoxOffset;
        hitBoxScale = transform.localScale;
        Collider2D wallCollider = Physics2D.OverlapBox(hitBoxPosition, hitBoxScale, 0, levelMaskLayer);
        if (wallCollider != null) {
            LevelManager.ReloadLevel();
        }

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
        //rend.sprite = crouching ? crouchSprite : standSprite;
    }

    // Visualize the overlap box
    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(hitBoxPosition, hitBoxScale);
    }
}