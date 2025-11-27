using System.Collections;
using UnityEngine;

public class Plant : MonoBehaviour
{
    private float _health = 2;
    public float health;
    private int damage = 1;

    player p;

    private float currentSpeed;

    public Animator animator;
    private PlantState plantState;

    public EnemyBullet enemyBulletPrefab;

    private enum PlantState
    {
        Idle,
        Attack,
        Dead
    }

    void Update()
    {
        UpdateState(plantState);
    }

    private void UpdateState(PlantState state)
    {
        plantState = state;

        switch (state)
        {
            case PlantState.Idle:
                animator.Play("PlantIdle");
                break;

            case PlantState.Attack:
                animator.Play("PlantAttack");
                EnemyBullet bullet = Instantiate(enemyBulletPrefab, transform.position, Quaternion.identity);
                bullet.SetDirection(p.transform.position);
                plantState = PlantState.Idle;
                break;

            case PlantState.Dead:
                animator.Play("PlantDead");
                gameObject.SetActive(false);
                break;
        }
    }
    void Start()
    {
        p = GameManager.Instance.p;
        plantState = PlantState.Idle;
        health = _health;
    }
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            plantState = PlantState.Attack;
            p = collision.gameObject.GetComponent<player>();
            p.TakeDamageP(damage);
        }
    }

    public void TakeDamagePlant(int damage)
    {

        _health -= damage;

        if (_health <= 0f)
        {
            plantState = PlantState.Dead;
        }
    }
}
