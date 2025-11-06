using UnityEngine;

public class BulletMove : MonoBehaviour
{
    public float speed = 8f;
    public float lifeTime = 5f;
    public bool usePlayerFacing = true;

    int direction = 1;
    float lifeTimer;

    Crow crow;
    Ghost ghost;

    void Start()
    {
        lifeTimer = lifeTime;

        if (usePlayerFacing)
        {
            var playerGO = GameObject.FindGameObjectWithTag("Player");
            if (playerGO != null)
            {
                float sx = playerGO.transform.localScale.x;
                direction = sx >= 0f ? 1 : -1;
            }
        }

        // ensure sprite/orientation matches direction
        Vector3 ls = transform.localScale;
        ls.x = Mathf.Abs(ls.x) * direction;
        transform.localScale = ls;
    }

    void Update()
    {
        transform.Translate(Vector2.right * direction * speed * Time.deltaTime, Space.Self);

        lifeTimer -= Time.deltaTime;
        if (lifeTimer <= 0f) Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
       if(collision.gameObject.CompareTag("Crow"))
        {
            crow.HPCrow(1);
        }
       if(collision.gameObject.CompareTag("Ghost"))
          {
                ghost.HPGhost(1);
        }
    }


    public void SetDirection(float dir)
    {
        direction = dir >= 0 ? 1 : -1;
        Vector3 ls = transform.localScale;
        ls.x = Mathf.Abs(ls.x) * direction;
        transform.localScale = ls;
    }
}
