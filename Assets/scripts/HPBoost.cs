using UnityEngine;

public class HPBoost : MonoBehaviour
{
    public player player;
    [SerializeField] int healAmount;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player.Heal(healAmount);
            gameObject.SetActive(false);
        }
    }
}
