using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Fader : MonoBehaviour
{
    public Image image;
    public float tweenTime = 1f;
    private bool startFaded = true;

    void Awake()
    {
        DOTween.Init(false, true);

        if (image != null)
        {
            var c = image.color;
            c.a = startFaded ? 1f : 0f;
            image.color = c;
        }
    }

    void Start()
    {
        if (startFaded && image != null)
        {
            Fade(false);
        }
    }

    public void Fade(bool fadeToBlack)
    {
        if (image == null)
        {
            Debug.LogWarning("Fader: No Image assigned.");

            SceneManager.LoadScene(1);

        }

        float targetValue = fadeToBlack ? 1f : 0f;
        image.DOFade(targetValue, tweenTime).OnComplete(() =>
        {
            SceneManager.LoadScene(1);
        });
    }
}
