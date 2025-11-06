using UnityEngine;

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
        UpdateState(CrowState.Idle);
        health = 2;
    }

    private void Update()
    {
        if (!isFound)
        {
            if (found())
            {
                isFound = true;
                UpdateState(CrowState.Walk);
            }
        }

        if (isFound && cState == CrowState.Walk && player != null)
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
    }

    public void HPCrow(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            UpdateState(CrowState.Dead);
        }
    }
    public bool found()
    {
        if (player == null) return false;
        float dist = Vector2.Distance(new Vector2(transform.position.x, transform.position.y),
                                      new Vector2(player.transform.position.x, player.transform.position.y));
        return dist <= detectRadius;
    }

    private void UpdateState(CrowState state)
    {
        cState = state;

        switch (state)
        {
            case CrowState.Idle:
                animator.SetBool("CrowWalk", false);
                break;

            case CrowState.Walk:
                animator.SetBool("CrowWalk", true);

                break;

            case CrowState.Attack:
                animator.SetTrigger("CrowAttack");
                break;

            case CrowState.Dead:
                animator.SetTrigger("CrowDead");
                shake.ShakeCamera();
                gameObject.SetActive(false);
                break;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            UpdateState(CrowState.Attack);
            player.TakeDamageP(1);
        }
    }
}
