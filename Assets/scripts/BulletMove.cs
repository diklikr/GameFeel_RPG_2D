using UnityEngine;
using UnityEngine.UI;

public class BulletMove : MonoBehaviour
{
    public int speed;
    public bool usePlayerFacing = true;

    int direction = 1;
    public float shootBoostTime;

    Crow crow;
    Ghost ghost;

    void Start()
    {
        speed = 10;

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
   public void SetSpeed(int newSpeed)
    {
                speed = newSpeed;
    }
    void Update()
    {
        transform.Translate(Vector2.right * direction * speed * Time.deltaTime, Space.Self);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
       if(collision.gameObject.CompareTag("Crow"))
        {
            crow.HPCrow(1);
            Destroy();
        }
       if(collision.gameObject.CompareTag("Ghost"))
          {
                ghost.HPGhost(1);
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
