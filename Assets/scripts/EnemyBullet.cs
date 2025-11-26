using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    private float speed = 5;
    private int damage = 1;

    Vector2 target;
    

    void Start()
    {
        Destroy(gameObject, 5f);
    }

    private void Update()
    {
        transform.Translate(target * speed * Time.deltaTime);
    }

    public void SetDirection(Vector3 direction)
    {

        target = (direction - transform.position).normalized;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("player"))
        {
            player p;
            p = collision.gameObject.GetComponent<player>();
            p.TakeDamageP(damage);
            Destroy(gameObject);
        }
    }
}
