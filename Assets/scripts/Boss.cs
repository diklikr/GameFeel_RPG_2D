using System.Collections;
using UnityEngine;
public class Boss : MonoBehaviour
{
    [SerializeField] private float maxHealth = 8f;
    private float currentHealth;

    [SerializeField] private float shootRange = 6f;
    [SerializeField] private float attackDelay = 2f;
    [SerializeField] private EnemyBullet enemyBulletPrefab;
    [SerializeField] private int contactDamage = 1;
    [SerializeField] private GameObject firePoint;

    [SerializeField] private GameObject pointA;
    [SerializeField] private GameObject pointB;
    [SerializeField] private float patrolSpeed = 2f;
    [SerializeField] private float reachThreshold = 0.1f;
    [SerializeField] private bool startAtA = true;

    [SerializeField] private Animator animator;

    private Coroutine attackCoroutine;
    private player p;

    private Vector3 aPos;
    private Vector3 bPos;
    private Vector3 targetPos;
    private bool goingToB;

    private BossState bState;

    private enum BossState
    {
        Walk,
        Attack,
        Dead
    }

    void Start()
    {
        p = GameManager.Instance != null ? GameManager.Instance.p : p;
        currentHealth = maxHealth;

        if (pointA != null && pointB != null)
        {
            aPos = pointA.transform.position;
            bPos = pointB.transform.position;
            goingToB = startAtA ? true : false;
            targetPos = goingToB ? bPos : aPos;
        }

        UpdateState(BossState.Walk);
    }

    private void Update()
    {
        switch (bState)
        {
            case BossState.Walk:
                Patrol();
                CheckPlayerDistanceAndStartAttack();
                break;
            case BossState.Attack:
                break;
            case BossState.Dead:
                break;
        }
    }

    private void Patrol()
    {
        if (pointA == null || pointB == null) return;
        if (bState != BossState.Walk) return;

        transform.position = Vector3.MoveTowards(transform.position, targetPos, patrolSpeed * Time.deltaTime);

        float dist = Vector3.Distance(transform.position, targetPos);
        if (dist <= reachThreshold)
        {
            goingToB = !goingToB;
            targetPos = goingToB ? bPos : aPos;
        }
    }

    private void CheckPlayerDistanceAndStartAttack()
    {
        if (p == null)
            p = GameManager.Instance != null ? GameManager.Instance.p : p;

        if (p == null) return;

        float dist = Vector3.Distance(p.transform.position, transform.position);
        if (dist <= shootRange && attackCoroutine == null)
            attackCoroutine = StartCoroutine(AttackRoutine());
    }

    private IEnumerator AttackRoutine()
    {
        yield return new WaitForSeconds(attackDelay);
        attackCoroutine = null;
        UpdateState(BossState.Attack);
    }

    private void UpdateState(BossState state)
    {
        if (state != BossState.Walk && attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
            attackCoroutine = null;
        }

        bState = state;

        switch (state)
        {
            case BossState.Walk:
                if (animator != null)
                    animator.Play("BossWalk");
                break;

            case BossState.Attack:
                if (animator != null)
                    animator.Play("BossAttack");

                if (enemyBulletPrefab != null && p != null)
                {
                    Vector3 spawnPos = firePoint != null ? firePoint.transform.position : transform.position;
                    EnemyBullet bullet = Instantiate(enemyBulletPrefab, spawnPos, Quaternion.identity);

                    Vector3 dir = (p.transform.position - transform.position);
                    if (dir.sqrMagnitude > 0.0001f)
                        bullet.SetDirection(dir.normalized);
                    else
                        bullet.SetDirection(Vector3.right);
                }

                UpdateState(BossState.Walk);
                break;

            case BossState.Dead:
                if (animator != null)
                    animator.Play("BossDeath");

                Destroy(gameObject, 2.0f);
                break;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (bState == BossState.Dead) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            p = collision.gameObject.GetComponent<player>();
            if (p != null)
                p.TakeDamageP(contactDamage);

            UpdateState(BossState.Attack);
        }
    }

    public void TakeDamageB(int damage)
    {
        if (bState == BossState.Dead) return;

        currentHealth -= damage;

        if (currentHealth <= 0f && bState != BossState.Dead)
            UpdateState(BossState.Dead);
        else
        {
            if (animator != null)
                animator.Play("BossHurt");
        }
    }
}