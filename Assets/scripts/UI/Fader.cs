using DG.Tweening;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Fader : MonoBehaviour
{
    public Image image;
    public float tweenTime = 1f;
    [SerializeField]
    private bool startFaded = true;

    public int sceneToLoad;

    void Awake()
    {
        Debug.Log("Awake");
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
        Debug.Log("Start");
        if (startFaded && image != null)
        {
            Fade(false);
            startFaded = false;
        }
    }

    public void Fade(bool fadeToBlack)
    {
        Debug.Log("Fading to " + (fadeToBlack ? "black" : "clear"));
        if (image == null)
        {
            Debug.LogWarning("Fader: No Image assigned.");

            SceneManager.LoadScene(sceneToLoad);

        }
        float targetValue = fadeToBlack ? 1f : 0f;
        if (fadeToBlack)
        {
            image.DOFade(targetValue, tweenTime).OnComplete(() =>
             {
                 SceneManager.LoadScene(sceneToLoad);
             });
        }
        else
        {
            Debug.Log("Time" + tweenTime + " Target" + targetValue);
            image.DOFade(targetValue, tweenTime).OnComplete(() =>
            {
                Debug.Log("Fade to clear complete.");
            });
        }
    }
}
