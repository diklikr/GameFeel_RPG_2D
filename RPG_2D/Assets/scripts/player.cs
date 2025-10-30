using UnityEngine;
using UnityEngine.SceneManagement;

public class player : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 12f;
    [SerializeField]
    private float damage = 2;
    [SerializeField]
    private float _health = 10f;

    public PlayerState pState;

    public Transform groundCheck;       // assign a child Transform at the player's feet
    public float groundCheckRadius = 0.1f;
    public LayerMask groundLayer;       // set to your ground layer(s)
    public Animator animator;

    Rigidbody2D rb;
    zombie z;
    private void UpdateState(PlayerState state)
    {
        pState = state;

        switch (state)
        {
            case PlayerState.Idle:
                animator.SetBool("isWalking", false);
                break;

            case PlayerState.Walk:
                animator.SetBool("isWalking", true);
                break;

            case PlayerState.Jump:
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                animator.SetTrigger("isJumping");

                break;

            case PlayerState.Attack:
               animator.SetTrigger("isAttacking");
                break;

            case PlayerState.Dead:
                animator.SetTrigger("isDead");
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
            pState = PlayerState.Jump;
        }
    }
    public void DealDamageP()
    {
        z.TakeDamageZ(damage);
    }
    public void TakeDamageP(float damage)
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
