using System.Collections;
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

    void OnEnable()
    {
        StartCoroutine(AutoDisableAfterTime(1f));
    }

    void OnDisable()
    {
        StopAllCoroutines();
    }

    void Start()
    {
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
            crow?.HPCrow(damage);
            Deactivate();
            return;
        }
        if (collision.gameObject.CompareTag("Ghost"))
        {
            collision.gameObject.TryGetComponent<Ghost>(out ghost);
            ghost?.HPGhost(damage);
            Deactivate();
            return;
        }
        if (collision.gameObject.CompareTag("Boss"))
        {
            collision.gameObject.TryGetComponent<Boss>(out Boss boss);
            boss?.TakeDamageB(damage);
            Deactivate();
            return;
        }
        if (collision.gameObject.CompareTag("Plant"))
        {
            collision.gameObject.TryGetComponent<Plant>(out Plant plant);
            plant?.TakeDamagePlant(damage);
            Deactivate();
            return;
        }
        if (collision.gameObject.CompareTag("Snake"))
        {
            zombie p;
            p = collision.gameObject.GetComponent<zombie>();
            p?.TakeDamageZ(damage);
            Deactivate();
            return;
        }
    }

    public void SetDirection(float dir)
    {
        direction = dir >= 0 ? 1 : -1;
        Vector3 ls = transform.localScale;
        ls.x = Mathf.Abs(ls.x) * direction;
        transform.localScale = ls;
    }

    private IEnumerator AutoDisableAfterTime(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        if (gameObject.activeInHierarchy)
            Deactivate();
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
