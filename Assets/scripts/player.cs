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
    public BulletMove arrow;

    public Transform groundCheck; 
    public float groundCheckRadius = 0.1f;
    public LayerMask groundLayer;   
    public Animator animator;

    public HealthUI healthUI;
    Rigidbody2D rb;
    zombie z;
    public CineCamara shake;
    private float lookingDirection;
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
                animator.SetTrigger("Jump");
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                break;

            case PlayerState.Attack:
               animator.SetTrigger("Attack");
                Instantiate(arrow, transform.position, Quaternion.identity);
                arrow.SetDirection(lookingDirection);
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
        lookingDirection = h != 0 ? Mathf.Sign(h) : lookingDirection;
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            UpdateState(PlayerState.Attack);
        }
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            UpdateState(PlayerState.Jump);
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

    }
    public void Heal(int healBoost)
    {
        _health += healBoost;
        health(_health);
    }
    public void TakeDamageP(int damage)
    {
        shake.ShakeCamera();
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

        if (_health >= 2)
        { 
            hasShield = true;
            _health = 2;
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
