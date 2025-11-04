using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private Sprite fullHeart;
    [SerializeField] private Sprite halfHeart;
    [SerializeField] private Sprite emptyHeart;
    [SerializeField] private Sprite shieldHeart;

    [SerializeField] private Image heart1;
    [SerializeField] private Image heart2;
    [SerializeField] private Image heart3;

    public void HPUI(int healthPoints)
    {
             switch(healthPoints)
            {
                case 7:
                    heart1.sprite = shieldHeart;
                    heart2.sprite = fullHeart;
                    heart3.sprite = fullHeart;
                    break;
                case 6:
                    heart1.sprite = fullHeart;
                    heart2.sprite = fullHeart;
                    heart3.sprite = fullHeart;
                    break;
                case 5:
                    heart1.sprite = fullHeart;
                    heart2.sprite = fullHeart;
                    heart3.sprite = halfHeart;
                    break;
                case 4:
                    heart1.sprite = fullHeart;
                    heart2.sprite = fullHeart;
                    heart3.sprite = emptyHeart;
                    break;
                case 3:
                    heart1.sprite = fullHeart;
                    heart2.sprite = halfHeart;
                    heart3.sprite = emptyHeart;
                    break;
                case 2:
                    heart1.sprite = fullHeart;
                    heart2.sprite = emptyHeart;
                    heart3.sprite = emptyHeart;
                    break;
                case 1:
                    heart1.sprite = halfHeart;
                    heart2.sprite = emptyHeart;
                    heart3.sprite = emptyHeart;
                    break;
                case 0:
                    heart1.sprite = emptyHeart;
                    heart2.sprite = emptyHeart;
                    heart3.sprite = emptyHeart;
                    break;
            }
    }
}