using UnityEngine;
using UnityEngine.SceneManagement;

public class player : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 12f;

    [SerializeField]
    private int _health;
    private bool hasShield;

    public PlayerState pState;

   
    public Transform groundCheck;       // assign a child Transform at the player's feet
    public float groundCheckRadius = 0.1f;
    public LayerMask groundLayer;       // set to your ground layer(s)
    public Animator animator;

    HealthUI healthUI;
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
                //shoot projectile
                break;

            case PlayerState.Dead:
                animator.SetTrigger("isDead");
                break;
        }
    }
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        hasShield = false;
    }

    void Update()
    {
        // Move
        float h = Input.GetAxisRaw("Horizontal"); // -1,0,1
        rb.linearVelocity = new Vector2(h * moveSpeed, rb.linearVelocity.y);
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            pState = PlayerState.Attack;
        }

        if (h != 0)
        {
            pState = PlayerState.Walk;
            Vector3 scale = transform.localScale;
            scale.x = h > 0 ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
            transform.localScale = scale;
        }
        else
        {
            pState = PlayerState.Idle;
        }

        // Jump
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            pState = PlayerState.Jump;
        }
    }
    public void TakeDamageP(int damage)
    {
        _health -= damage;
        if (hasShield)
        {
            hasShield = false;
            healthUI.HPUI(_health);
            healthClamp(_health);
        }
        else
        {
            healthUI.HPUI(_health);
            healthClamp(_health);
        }
    }
    public void healthClamp(int healthPoints)
    {
        _health = healthPoints;

        if (_health >= 7)
        { 
            hasShield = true;
            _health = 7;
        }
        if (_health <= 0)
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
