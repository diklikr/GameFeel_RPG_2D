using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using UnityEngine.UI;
public class Fader : MonoBehaviour
{
    public Image image;
    public float tweenTime;
    public UnityEvent onEndfade;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void Fade(bool fadeIn)
    {
        float targetValue = fadeIn ? 1f : 0f;
        image.DOFade(targetValue, tweenTime).OnComplete(onEndfade.Invoke);
    }
}
