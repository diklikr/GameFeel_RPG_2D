using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class zombie : MonoBehaviour
{

    private float _health = 10;
    public float health;
    public float damage = 5f;

    player p;

    public EnemyState zState;
    public NavMeshAgent agent;
    public SphereCollider visionTrigger;

    private float currentSpeed;
    private Vector3 currentTarget;
    public float patrolSpeed, attackSpeed, patrolRadius, arriveDistance;

    private void UpdateState(EnemyState state)
    {
        zState = state;

        switch (state)
        {
            case EnemyState.Idle:
                currentSpeed = 0;
                currentTarget = GetRandomPoint();
                break;

            case EnemyState.Patrol:
                currentSpeed = patrolSpeed;
                break;

            case EnemyState.Aggro:
                currentSpeed = attackSpeed;
                currentTarget = p.transform.position;
                break;

            case EnemyState.Attack:
                DealDamage();
                break;

            case EnemyState.Dead:
                gameObject.SetActive(false);
                break;
        }
    }

    private Vector3 GetRandomPoint()
    {
        return Random.insideUnitSphere * patrolRadius;
    }

    private bool HasReachedTarget()
    {
        return Vector3.Distance(transform.position, currentTarget) > arriveDistance;
    }

    void Start()
    {
        health = _health;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            zState = EnemyState.Attack;
        }
    }
    void DealDamage()
    {
        p.TakeDamage(damage);
    }

}
