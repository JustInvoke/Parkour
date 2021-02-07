using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;

public class CharacterControl : MonoBehaviour
{
    private Rigidbody2D rb;
    public Collider2D standCol; // Collider used while standing
    public Collider2D crouchCol; // Collider used while crouching
    public LayerMask groundMask; // Layer mask representing ground objects
    private bool grounded = false; // Whether the character is standing on the ground
    public Rect groundBox = new Rect(Vector2.zero, Vector2.one); // Dimensions for the ground overlap box
    private float airTime = 0.0f; // Time spent in the air since last grounded
    public float airJumpTime = 0.1f; // Time after leaving a platform that the character can still jump
    // Hitboxes for detecting wall collision
    public Vector3 hitBoxScale;
    public Vector3 hitBoxOffset;
    public Vector3 hitBoxScaleCrouch;
    public Vector3 hitBoxOffsetCrouch;
    public float fallLimit = -7; // If the character's y-position is less than this, then he dies

    private SpriteRenderer rend;
    public Sprite standSprite; // Sprite used while standing
    public Sprite crouchSprite; // Sprite used while crouching
    public float minSpeed = 1.0f; // Minimum allowed movement speed
    public float maxSpeed = 2.0f; // Maximum allowed movement speed
    public float accel = 1.0f; // acceleration
    public float speedup = 0.0f; // Current speedup
    public float maxSpeedup = 6.0f; // Maximum speedup
    public float speedupIncrement = 1.0f;
    private float milestoneDistance = 1000.0f; // How long until next speedup increase
    private Vector3 lastMilestonePos = new Vector3(0, 0, 0); // Keeps track of last milestone position

    public float jumpForce = 1.0f;
    private float timeSinceLastJump = 0.0f;
    public float jumpTimeLimit = 0.15f; // Minimum time allowed between repeated jumps
    public float fastFallForce = 1.0f; // Force applied when crouching while in air

    public KeyCode rightInput = KeyCode.RightArrow; // Max speed movement
    public KeyCode leftInput = KeyCode.LeftArrow; // Min speed movement
    public KeyCode jumpInput = KeyCode.UpArrow;
    public KeyCode crouchInput = KeyCode.DownArrow;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip slideSound;
    [SerializeField] private AudioClip stepLSound;
    [SerializeField] private AudioClip stepRSound;

    public UnityEvent dieEvent; // Public event invoked when dying

    private void Start() {
        // Cache component references
        rb = GetComponent<Rigidbody2D>();
        rend = GetComponentInChildren<SpriteRenderer>();
        StartCoroutine(PlayStepSound(stepRSound));
    }

    private void Update() {
        // Jump action
        if (Input.GetKeyDown(jumpInput) && timeSinceLastJump > jumpTimeLimit && (grounded || airTime < airJumpTime)) {
            timeSinceLastJump = 0.0f;
            // Reset vertical speed so it doesn't reduce jump height
            if (rb.velocity.y < 0) {
                rb.velocity = new Vector2(rb.velocity.x, 0.0f);
            }
            // Add jump force
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            //Play jump sound
            audioSource.PlayOneShot(jumpSound);
        }

        // Add fast fall force
        if (Input.GetKeyDown(crouchInput) && !grounded) {
            rb.AddForce(Vector2.down * fastFallForce, ForceMode2D.Impulse);
        }
    }

    private void FixedUpdate() {
        // Check if standing on ground
        grounded = Physics2D.OverlapBox(new Vector2(transform.position.x, transform.position.y) + groundBox.center, groundBox.size, 0.0f, groundMask) != null;
        airTime = grounded ? 0.0f : airTime + Time.fixedDeltaTime;
        timeSinceLastJump += Time.fixedDeltaTime;

        // Convert key inputs to single float ranging from 0 to 1
        float moveInput = (Input.GetKey(rightInput) ? 1.0f : 0.5f) + (Input.GetKey(leftInput) ? -0.5f : 0.0f);
        // Update min and max speed if need be
        if (speedup < maxSpeedup) {
            if (Mathf.Abs(transform.position.x) - Mathf.Abs(lastMilestonePos.x) >= milestoneDistance) {
                speedup += speedupIncrement;
                minSpeed += speedupIncrement;
                maxSpeed += speedupIncrement;
                lastMilestonePos = transform.position;
            }
        }
        // Interpolate between min and max speed
        float targetSpeed = Mathf.Lerp(minSpeed, maxSpeed, moveInput);
        // Add movement force
        rb.AddForce(Vector2.right * (targetSpeed - rb.velocity.x) * accel, ForceMode2D.Force);

        // Crouch logic with colliders and sprites
        bool crouching = Input.GetKey(crouchInput);
        standCol.enabled = !crouching;
        if (!standCol.enabled && !crouchCol.enabled) {
            audioSource.PlayOneShot(slideSound);
        }
        crouchCol.enabled = crouching;
        rend.sprite = crouching ? crouchSprite : standSprite;

        // Reload the level if the character touches a wall
        Collider2D wallCollider = null;
        if (crouching) {
            wallCollider = Physics2D.OverlapBox(transform.position + hitBoxOffsetCrouch, hitBoxScaleCrouch, 0.0f, groundMask);
        }
        else {
            wallCollider = Physics2D.OverlapBox(transform.position + hitBoxOffset, hitBoxScale, 0.0f, groundMask);
        }

        if (wallCollider != null || transform.position.y < fallLimit) {
            Die();
        }
    }

    private void OnDrawGizmosSelected() {
        // Visualize ground hitbox
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(new Vector2(transform.position.x, transform.position.y) + groundBox.center, groundBox.size);
        // Visualize wall hitbox
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(transform.position + hitBoxOffset, hitBoxScale);
        Gizmos.color = new Color(1.0f, 0.5f, 0.5f);
        Gizmos.DrawWireCube(transform.position + hitBoxOffsetCrouch, hitBoxScaleCrouch);
    }

    // Corouting for playing step sounds
    private IEnumerator PlayStepSound(AudioClip audioClip) {
        if (grounded && standCol.enabled) {
            audioSource.PlayOneShot(audioClip);
        }

        yield return new WaitForSeconds(0.2f);

        AudioClip newSound = audioClip == stepRSound ? stepLSound : stepRSound;

        StartCoroutine(PlayStepSound(newSound));
    }

    private void Die() {
        dieEvent.Invoke(); // Invoke any functions that should be called upon death
    }

    private void OnCollisionStay2D(Collision2D collision) {
        foreach (ContactPoint2D contact in collision.contacts) {
            // Kill the character if he hits a spike
            if (contact.collider.CompareTag("Kill")) {
                Die();
            }
        }
    }
}
