using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class Boss : MonoBehaviour
{

    private float _health = 8;
    public float health;
    private int damage = 1;

    player p;

    Coroutine attackCoroutine;

    private float currentSpeed;

    public Animator animator;
    private BossState bState;

    public EnemyBullet enemyBulletPrefab;

    private enum BossState
    {
        Walk,
        Attack,
        Dead
    }

    IEnumerator Attack()
    {
        yield return new WaitForSeconds(2f);
        bState = BossState.Attack;
    }
    private void UpdateState(BossState state)
    {
        bState = state;

        switch (state)
        {
            case BossState.Walk:
                //walk yoyo movement
                attackCoroutine = StartCoroutine(Attack());
                break;

            case BossState.Attack:
                EnemyBullet bullet = Instantiate(enemyBulletPrefab, transform.position, Quaternion.identity);
                bullet.SetDirection(p.transform.position);
                bState = BossState.Walk;
                break;

            case BossState.Dead:
                gameObject.SetActive(false);
                break;
        }
    }
    void Start()
    {
        p = GameManager.Instance.p;
        bState = BossState.Walk;
        health = _health;
    }
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            bState = BossState.Attack;
            p = collision.gameObject.GetComponent<player>();
            p.TakeDamageP(damage);
        }
    }

    public void TakeDamageB(int damage)
    {

        _health -= damage;

        if (_health <= 0f)
        {
            bState = BossState.Dead;
        }
    }
}