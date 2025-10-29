using UnityEngine;

public class zombie : MonoBehaviour
{

    private float _health = 10;
    public float health;
    public float speed = 2f;
    public float damage = 5f;

    player p;

    void Start()
    {
        health = _health;
    }

 
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //play animation
            DealDamage();
        }
    }
    void DealDamage()
    {
        p.TakeDamage(damage);
    }

}
