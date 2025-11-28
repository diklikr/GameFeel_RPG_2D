using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public class Flicker : MonoBehaviour
{
    [SerializeField]
    public SpriteRenderer spriteRenderer;
    [SerializeField]
    public Color targetColor;
    [SerializeField]
    public float tweenTime;
    [SerializeField]
    public int loops;

    private int playerLayer;

    public int immuneLayer;

    private Tween flickerTween;

    [SerializeField]
    public KeyCode debugKey;
   
    public UnityEvent flickerEvent;

    public void FlickerEffect()
    {
        flickerEvent.Invoke();
        flickerTween?.Kill(true);
       flickerTween = spriteRenderer.DOColor(targetColor, tweenTime).SetLoops(loops, LoopType.Yoyo).OnStart(()=> { SetImunity(true); }).OnComplete(()=> { SetImunity(false); }) ;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(debugKey))
        {
            FlickerEffect();
        }
    }

    private void SetImunity(bool immune)
    {
        int targetLayer = immune ? immuneLayer : playerLayer;
    }
}
