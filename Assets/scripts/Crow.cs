using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Crow : MonoBehaviour
{
    public CineCamara shake;
    public Animator animator;
    public player player;
    CrowState cState;

    public float detectRadius = 8f;
    public float flySpeed = 3f;
    public float stopDistance = 0.5f;
    public int health;
    bool isFound = false;

    private void Start()
    {
        SetState(CrowState.Idle);
    }

    private void Update()
    {
        UpdateState();
    }

    public void SetState(CrowState state)
    {
                cState = state;

        switch (state)
        {
            case CrowState.Idle:
                break;

            case CrowState.Walk:
                animator.Play("CrowWalk");
                break;

            case CrowState.Attack:
                animator.Play("CrowAttack");
                player.TakeDamageP(1);
                break;

            case CrowState.Dead:
                animator.Play("CrowDead");
                shake.ShakeCamera();
                gameObject.GetComponent<Collider2D>().enabled = false;
                Destroy(gameObject, 1f);
                break;
            case CrowState.Hurt:
                animator.Play("CrowHurt");

                break;
        }
    }

    public void HPCrow(int damage)
    {
        health -= damage;
        SetState(CrowState.Dead);

        if (health <= 0)
        {
            SetState(CrowState.Dead);
        }
    }
    public bool found()
    {
        if (player == null) return false;
        float dist = Vector2.Distance(new Vector2(transform.position.x, transform.position.y),
                                      new Vector2(player.transform.position.x, player.transform.position.y));
        return dist <= detectRadius;
    }

    private void UpdateState()
    {
        switch (cState)
        {
            case CrowState.Idle:
                if (found())
                {
                    isFound = true;
                    SetState(CrowState.Walk);
                }
                break;

            case CrowState.Walk:
                if (found())
                {
                    Vector3 dir = (player.transform.position - transform.position);
                    float dist = dir.magnitude;
                    if (dist > stopDistance)
                    {
                        dir.Normalize();
                        transform.position += dir * flySpeed * Time.deltaTime;
                        if (dir.x != 0f)
                            transform.localScale = new Vector3(Mathf.Sign(dir.x) * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                    }
                }
            break;

            case CrowState.Attack:
                animator.Play("CrowAttack");
                if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
                {
                    SetState(CrowState.Walk);
                }
                break;

            case CrowState.Dead:   
                
                break;

            case CrowState.Hurt:

                if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
                {
                    SetState(CrowState.Walk);
                }
                break;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            SetState(CrowState.Attack);
        }
    }
}
