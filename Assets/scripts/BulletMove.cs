using UnityEngine;
using UnityEngine.UI;

public class BulletMove : MonoBehaviour
{
    private float speed;

    int direction = 1;
    public float shootBoostTime;
    int damage = 1;

    Crow crow;
    Ghost ghost;

    void Start()
    {
        // ensure sprite/orientation matches direction
        Vector3 ls = transform.localScale;
        ls.x = Mathf.Abs(ls.x) * direction;
        transform.localScale = ls;
    }
   public void SetSpeed(float newSpeed)
    {
                speed = newSpeed;
        Debug.Log("Speed set to: " + newSpeed);
    }
    void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime, Space.Self);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        print("Bullet collided with: " + collision.gameObject.name);
        if (collision.gameObject.CompareTag("Crow"))
        {
            collision.gameObject.TryGetComponent<Crow>(out crow);
            crow.HPCrow(damage);
            Destroy();
        }
       if(collision.gameObject.CompareTag("Ghost"))
          {
            collision.gameObject.TryGetComponent<Ghost>(out ghost);
            ghost.HPGhost(damage);
            Destroy();
        }
       if(collision.gameObject.CompareTag("Boss"))
        {

            Destroy();
        }
       if(collision.gameObject.CompareTag("Plant"))
        {

            Destroy();
        }
       if(collision.gameObject.CompareTag("Snake"))
        {
            zombie p;
            p = collision.gameObject.GetComponent<zombie>();
            p.TakeDamageZ(damage);
            Destroy();
        }
        
    }


    public void SetDirection(float dir)
    {
        direction = dir >= 0 ? 1 : -1;
        Vector3 ls = transform.localScale;
        ls.x = Mathf.Abs(ls.x) * direction;
        transform.localScale = ls;
    }
   
    public void Destroy()
    {
        gameObject.SetActive(false);
    }
}
