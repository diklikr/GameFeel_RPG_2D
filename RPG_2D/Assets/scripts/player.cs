using UnityEngine;
using UnityEngine.SceneManagement;

public class player : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 12f;

    [SerializeField]
    private float _health = 10f;

    public PlayerState pState;

    public Transform groundCheck;       // assign a child Transform at the player's feet
    public float groundCheckRadius = 0.1f;
    public LayerMask groundLayer;       // set to your ground layer(s)

    Rigidbody2D rb;
    private void UpdateState(PlayerState state)
    {
        pState = state;

        switch (state)
        {
            case PlayerState.Idle:
                break;

            case PlayerState.Walk:
                break;

            case PlayerState.Jump:
             
                break;

            case PlayerState.Attack:
               
                break;

            case PlayerState.Dead:
                SceneManager.LoadScene(2);
                break;
        }
    }
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Move
        float h = Input.GetAxisRaw("Horizontal"); // -1,0,1
        rb.linearVelocity = new Vector2(h * moveSpeed, rb.linearVelocity.y);

        // Jump
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    public void TakeDamage(float damage)
    {

        _health -= damage;

        if (_health <= 0f)
        {
            pState = PlayerState.Dead;
        }
    }
    bool IsGrounded()
    {
        if (groundCheck == null) return false;
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer) != null;
    }
}
