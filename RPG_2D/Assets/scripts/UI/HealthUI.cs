using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private Sprite fullHeart;
    [SerializeField] private Sprite halfHeart;
    [SerializeField] private Sprite emptyHeart;
    [SerializeField] private Sprite shieldHeart;

    [SerializeField] private Image heart1;

    public void HPUI(int healthPoints)
    {
             switch(healthPoints)
            {
                case 2:
                    heart1.sprite = shieldHeart;
                    break;
                case 1:
                    heart1.sprite = fullHeart;
                    break;
                case 0:
                    heart1.sprite = emptyHeart;
                    break;

        }
    }
}