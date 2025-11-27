using UnityEngine;

public class Plant : MonoBehaviour
{
    public EnemyBullet projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float shootCooldown = 1.2f;
    [SerializeField] private float shootRange = 8f;

    public int health = 2;


    private float timer;
    private Transform player;

    public bool IsDead => health <= 0;

    void Awake()
    {
   
        var playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj) player = playerObj.transform;
    }

    void Update()
    {

        if (IsDead || !player) return;

        int facing = player.position.x >= transform.position.x ? 1 : -1;
        var s = transform.localScale; s.x = Mathf.Abs(s.x) * facing; transform.localScale = s;

        timer -= Time.deltaTime;
        float dist = Vector2.Distance(player.position, transform.position);
        if (dist <= shootRange && timer <= 0f)
        {
            Shoot(facing);
            timer = shootCooldown;
        }

    }

    private void Shoot(int facing)
    {
        if (!projectilePrefab || !firePoint) return;
        var proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

        Vector2 dir = new Vector2(facing, 0.15f).normalized;
    }
}
