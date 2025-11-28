using System.Collections;
using UnityEngine;

public class Plant : MonoBehaviour
{
    [SerializeField] private float maxHealth = 2f;
    private float currentHealth;

    [SerializeField] private float shootRange = 3f;
    [SerializeField] private float attackDelay = 2f;
    [SerializeField] private EnemyBullet enemyBulletPrefab;
    [SerializeField] private int contactDamage = 1;
    [SerializeField] private Transform firePoint;

    [SerializeField] private Animator animator;

    private Coroutine attackCoroutine;
    private player p;

    private PlantState plantState;

    private enum PlantState
    {
        Idle,
        Attack,
        Dead
    }

    void Start()
    {
        p = GameManager.Instance != null ? GameManager.Instance.p : null;
        currentHealth = maxHealth;
        UpdateState(PlantState.Idle);
    }

    void Update()
    {
        if (plantState == PlantState.Idle)
            CheckPlayerAndMaybeStartAttack();
    }

    private void CheckPlayerAndMaybeStartAttack()
    {
        if (p == null)
            p = GameManager.Instance != null ? GameManager.Instance.p : null;

        if (p == null) return;

        float dist = Vector2.Distance(p.transform.position, transform.position);
        if (dist <= shootRange && attackCoroutine == null)
            attackCoroutine = StartCoroutine(AttackRoutine());
    }

    private IEnumerator AttackRoutine()
    {
        yield return new WaitForSeconds(attackDelay);
        attackCoroutine = null;
        UpdateState(PlantState.Attack);
    }

    private void UpdateState(PlantState state)
    {
        if (state != PlantState.Idle && attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
            attackCoroutine = null;
        }

        plantState = state;

        switch (state)
        {
            case PlantState.Idle:
                if (animator != null)
                    animator.Play("PlantIdle");
                break;

            case PlantState.Attack:
                if (animator != null)
                    animator.Play("PlantAttack");

                if (enemyBulletPrefab != null)
                {
                    Vector3 spawnPos = firePoint != null ? firePoint.position : transform.position;
                    EnemyBullet bullet = Instantiate(enemyBulletPrefab, spawnPos, Quaternion.identity);

                    if (p == null)
                        p = GameManager.Instance != null ? GameManager.Instance.p : null;

                    if (p != null)
                    {
                        Vector3 dir = (p.transform.position - transform.position);
                        if (dir.sqrMagnitude > 0.0001f)
                            bullet.SetDirection(dir.normalized);
                        else
                            bullet.SetDirection(Vector3.right);
                    }
                }
                UpdateState(PlantState.Idle);
                break;

            case PlantState.Dead:
                if (animator != null)
                    animator.Play("PlantDead");

                Destroy(gameObject, 2f);
                break;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (plantState == PlantState.Dead) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            p = collision.gameObject.GetComponent<player>();
            if (p != null)
                p.TakeDamageP(contactDamage);

            UpdateState(PlantState.Attack);
        }
    }

    public void TakeDamagePlant(int damage)
    {
        if (plantState == PlantState.Dead) return;

        currentHealth -= damage;

        if (currentHealth <= 0f)
        {
            UpdateState(PlantState.Dead);
        }
        else
        {
            if (animator != null)
                animator.Play("PlantHurt");
        }
    }
}