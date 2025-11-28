using UnityEngine;
using UnityEngine.SceneManagement;

public class zombie : MonoBehaviour
{
    [SerializeField] private float detectionRange = 5f;

    private float _health = 1;
    public float health;
    private int damage = 1;

    player p;

    public EnemyState zState;

    public Animator animator;
    public float walkSpeed;
    public Rigidbody2D rb;
    private int dir = 1;

    void Update()
    {
        switch (zState)
        {
            case EnemyState.Walk:
                if (p == null && GameManager.Instance != null)
                    p = GameManager.Instance.p;

                if (p != null)
                {
                    float distToPlayer = Vector2.Distance(p.transform.position, transform.position);
                    if (distToPlayer <= detectionRange)
                    {
                        float dx = p.transform.position.x - transform.position.x;
                        dir = dx > 0 ? 1 : -1;

                        if (rb != null)
                        {
                            var v = rb.linearVelocity;
                            v.x = dir * walkSpeed;
                            rb.linearVelocity = v;
                        }

                        var s = transform.localScale;
                        s.x = Mathf.Abs(s.x) * dir;
                        transform.localScale = s;
                    }
                    else
                    {
                        if (rb != null)
                        {
                            var v = rb.linearVelocity;
                            v.x = 0f;
                            rb.linearVelocity = v;
                        }
                    }
                }
                break;

            case EnemyState.Attack:
                break;

            case EnemyState.Dead:
                break;
        }
    }

    private void UpdateState(EnemyState state)
    {
        zState = state;
        switch (state)
        {
            case EnemyState.Walk:
                if (animator != null) animator.Play("SnakeWalk");
                break;

            case EnemyState.Attack:
                DealDamageZ();
                break;

            case EnemyState.Dead:
                if (animator != null) animator.Play("SnakeDead");
                Destroy(gameObject, 2f);
                break;
        }
    }

    void Start()
    {
        health = _health;
        if (p == null && GameManager.Instance != null)
            p = GameManager.Instance.p;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            p = collision.gameObject.GetComponent<player>();
            UpdateState(EnemyState.Attack);
        }
    }

    void DealDamageZ()
    {
        if (p != null) p.TakeDamageP(damage);
    }

    public void TakeDamageZ(int damage)
    {
        _health -= damage;

        if (_health <= 0f)
        {
            UpdateState(EnemyState.Dead);
        }
    }
}