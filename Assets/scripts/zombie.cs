using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class zombie : MonoBehaviour
{

    private float _health = 1;
    public float health;
    private int damage = 1;

    public player p;

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
                if (p != null)
                {
                    float dx = p.transform.position.x - transform.position.x;
                    dir = dx > 0 ? 1 : -1;
                }

                if (rb != null)
                {
                    var v = rb.linearVelocity;
                    v.x = dir * walkSpeed;
                    rb.linearVelocity = v;
                }

                var s = transform.localScale;
                s.x = Mathf.Abs(s.x) * dir;
                transform.localScale = s;
                break;

            case EnemyState.Attack:
                break;

            case EnemyState.Dead:
                gameObject.SetActive(false);
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
                gameObject.SetActive(false);
                break;
        }

    }
    void Start()
    {
        health = _health;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
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
            zState = EnemyState.Dead;
        }
    }
}