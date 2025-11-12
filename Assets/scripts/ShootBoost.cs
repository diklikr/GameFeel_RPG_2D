using UnityEngine;
using UnityEngine.UI;

public class ShootBoost : MonoBehaviour
{
BulletMove bulletMove;
    float shootBoostTime;
    [SerializeField] Text boostTimeText;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        shootBoostTime = 10;  // 1.5 times the normal speed
        GameObject.Destroy(this);
    }

    private void Update()
    {
        shootBoostTime -= Time.deltaTime;
        if (shootBoostTime > 0f)
        {
            bulletMove.SetSpeed(10);
        boostTimeText.text = shootBoostTime.ToString();
        }
        else if (shootBoostTime <= 0f)
        {
            bulletMove.SetSpeed(5);
            shootBoostTime = 0f;
            boostTimeText.text = "0";
        }
    }
}
