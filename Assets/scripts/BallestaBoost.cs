using UnityEngine;

public class BallestaBoost : MonoBehaviour
{
            player playerComponent;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerComponent = collision.gameObject.GetComponent<player>();
            if (playerComponent != null)
            {
                playerComponent.SetBoostToBallesta();
                Destroy(gameObject);
            }
            SoundList.instance.PlaySound("Boosts");
        }
    }
}