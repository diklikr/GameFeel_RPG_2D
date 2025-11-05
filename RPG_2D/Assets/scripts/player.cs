using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class player : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 12f;

    [SerializeField]
    private int _health;
    private bool hasShield;

    public PlayerState pState;

   
    public Transform groundCheck; 
    public float groundCheckRadius = 0.1f;
    public LayerMask groundLayer;   
    public Animator animator;

    public HealthUI healthUI;
    Rigidbody2D rb;
    zombie z;
    private void UpdateState(PlayerState state)
    {
        pState = state;

        switch (state)
        {
            case PlayerState.Idle:
                animator.SetBool("Walk", false);
                break;

            case PlayerState.Walk:
                animator.SetBool("Walk", true);
                break;

            case PlayerState.Jump:
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                animator.SetTrigger("Jump");
                break;

            case PlayerState.Attack:
               animator.SetTrigger("Attack");
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
            UpdateState(PlayerState.Attack);
        }

        if (h != 0)
        {
            UpdateState(PlayerState.Walk);
            Vector3 scale = transform.localScale;
            scale.x = h > 0 ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
            transform.localScale = scale;
        }
        else
        {
            UpdateState(PlayerState.Idle);
        }

        // Jump
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            UpdateState(PlayerState.Jump);
        }
    }
    public void Heal(int healBoost)
    {
                _health += healBoost;
        health(_health);
    }
    public void TakeDamageP(int damage)
    {
        _health -= damage;
        if (hasShield)
        {
            hasShield = false;
            health(_health);
        }
        else
        {
            
            health(_health);
        }
    }
    public void health(int healthPoints)
    {
        
        _health = healthPoints;

        if (_health >= 7)
        { 
            hasShield = true;
            _health = 7;
            healthUI.HPUI(_health);
        }
        if (_health <= 0)
        {
            UpdateState(PlayerState.Dead);
        }
        healthUI.HPUI(_health);
    }
    bool IsGrounded()
    {
        if (groundCheck == null) return false;
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer) != null;
    }
}
