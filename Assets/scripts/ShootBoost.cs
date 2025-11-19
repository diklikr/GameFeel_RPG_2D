using UnityEngine;

public class ShootBoost : MonoBehaviour
{
    player playerComponent;
    SoundList soundList;
    private void OnCollisionEnter2D(Collision2D collision)
    {
            playerComponent.SetBoostToShoot();
            SoundList.instance.PlaySound("Boosts");
            Destroy(gameObject);
    }
}