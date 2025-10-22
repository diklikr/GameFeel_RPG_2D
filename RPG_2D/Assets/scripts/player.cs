using UnityEngine;

public class player : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 12f;

    public Transform groundCheck;       // assign a child Transform at the player's feet
    public float groundCheckRadius = 0.1f;
    public LayerMask groundLayer;       // set to your ground layer(s)

    Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Horizontal movement (A/D or left/right)
        float h = Input.GetAxisRaw("Horizontal"); // -1,0,1
        rb.linearVelocity = new Vector2(h * moveSpeed, rb.linearVelocity.y);

        // Jump (Space)
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    bool IsGrounded()
    {
        if (groundCheck == null) return false;
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer) != null;
    }
}
