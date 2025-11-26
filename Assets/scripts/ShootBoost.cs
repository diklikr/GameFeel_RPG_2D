using UnityEngine;

public class ShootBoost : MonoBehaviour
{
    player playerComponent;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerComponent = collision.gameObject.GetComponent<player>();
            if (playerComponent != null)
            {
                playerComponent.SetBoostToShoot();
                Destroy(gameObject);
            }
            SoundList.instance.PlaySound("Boosts");
        }
    }
}