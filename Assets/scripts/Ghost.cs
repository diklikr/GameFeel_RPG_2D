using System.Collections;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    public Animator animator;
    public CineCamara shake;
    GhostState gstate;
    int health;

    public float speed = 1.5f;
    public float floatAmplitude = 0.25f;
    public float floatFrequency = 1f;
    public float attackDelay = 0.2f;
    public int damage = 1;

    float baseY;
    bool isAttacking = false;
    public player player;
    private void Start()
    {
        baseY = transform.position.y;
        UpdateState(GhostState.Walk);
        health = 1;
    }

    private void Update()
    {
        switch (gstate)
        {
            case GhostState.Idle:
                break;

            case GhostState.Walk:
                if (!isAttacking)
                {
                    Vector3 pos = transform.position;
                    pos += transform.right * speed * Time.deltaTime;
                    pos.y = baseY + Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
                    transform.position = pos;
                }
                animator.SetBool("GhostWalk", true);
                break;

            case GhostState.Attack:
                animator.SetTrigger("GhostAttack");
                player.TakeDamageP(1);
                UpdateState(GhostState.Walk);
                break;

            case GhostState.Dead:
                animator.SetTrigger("GhostDead");
                shake.ShakeCamera();
                gameObject.SetActive(false);
                break;
        }
    }

    public void HPGhost(int damageTaken)
    {
        health -= damageTaken;

        if (health <= 0)
        {
            UpdateState(GhostState.Dead);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isAttacking)
        {
            UpdateState(GhostState.Attack);
            
        }
    }

    private void UpdateState(GhostState state)
    {
        gstate = state;

        switch (state)
        {
            case GhostState.Idle:
                animator.SetBool("GhostWalk", false);
                break;

            case GhostState.Walk:
                animator.SetBool("GhostWalk", true);
                break;

            case GhostState.Attack:
                animator.SetTrigger("GhostAttack");
                break;

            case GhostState.Dead:
                animator.SetTrigger("GhostDead");
                gameObject.SetActive(false);
                break;
        }
    }
}
