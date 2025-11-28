using System.Collections;
using Unity.VisualScripting;
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
    public TimeManager timeManager;
    public float lookingDirection;

    public SoundList soundList;

    public BoostType currentBoost = BoostType.None;
    public int normalBulletSpeed = 5;
    public int shootBoostedSpeed = 10;
    public float spreadAngle = 15f;

    public enum BoostType
    {
        None,
        Shoot,
        Ballesta
    }

    [SerializeField] private float attackCooldown = 1.2f;
    [SerializeField] private float shootBoostedCooldown = 0.7f;

    private bool deathStarted = false;
    private bool canAttack = true;

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
                SoundList.instance.PlaySound("Jump");
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                break;

            case PlayerState.Attack:
                animator.SetTrigger("Attack");
                if (arrow != null)
                {
                    if (currentBoost == BoostType.Ballesta)
                    {
                        var left = Instantiate(arrow, transform.position, Quaternion.Euler(0f, 0f, spreadAngle));
                        left.SetSpeed(normalBulletSpeed * lookingDirection);
                        left.SetDirection(lookingDirection);

                        var center = Instantiate(arrow, transform.position, Quaternion.Euler(0f, 0f, 0f));
                        center.SetSpeed(normalBulletSpeed * lookingDirection);
                        center.SetDirection(lookingDirection);

                        var right = Instantiate(arrow, transform.position, Quaternion.Euler(0f, 0f, -spreadAngle));
                        right.SetSpeed(normalBulletSpeed * lookingDirection);
                        right.SetDirection(lookingDirection);
                    }
                    else
                    {
                        var center = Instantiate(arrow, transform.position, Quaternion.identity);
                        center.SetSpeed(currentBoost == BoostType.Shoot ? shootBoostedSpeed * lookingDirection : normalBulletSpeed * lookingDirection);
                        center.SetDirection(lookingDirection);
                    }
                }
                SoundList.instance.PlaySound("Attack");
                break;

            case PlayerState.Dead:
                animator.SetTrigger("isDead");
                SoundList.instance.PlaySound("Death");
                break;
        }
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        hasShield = false;
    }
    void Start()
    {
        lookingDirection = 1f;
        health(_health);
        SoundList.instance.PlaySound("BG");
    }

    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        rb.linearVelocity = new Vector2(h * moveSpeed, rb.linearVelocity.y);
        lookingDirection = h != 0 ? Mathf.Sign(h) : lookingDirection;

        if (Input.GetKeyDown(KeyCode.Mouse0) && canAttack)
        {
            UpdateState(PlayerState.Attack);
            float cd = currentBoost == BoostType.Shoot ? shootBoostedCooldown : attackCooldown;
            StartCoroutine(AttackCooldown(cd));
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
        timeManager.FreezeFrame(0, 0.2f);
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
            if (!deathStarted)
            {
                deathStarted = true;
                UpdateState(PlayerState.Dead);
                StartCoroutine(DeathDelay());
            }
            return;
        }
        healthUI.HPUI(_health);
    }

    private IEnumerator DeathDelay()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(2);
    }

    private IEnumerator AttackCooldown(float cooldown)
    {
        canAttack = false;
        yield return new WaitForSeconds(cooldown);
        canAttack = true;
    }

    bool IsGrounded()
    {
        if (groundCheck == null) return false;
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer) != null;
    }

    public void SetBoostToShoot()
    {
        currentBoost = BoostType.Shoot;
    }

    public void SetBoostToBallesta()
    {
        currentBoost = BoostType.Ballesta;
    }
}