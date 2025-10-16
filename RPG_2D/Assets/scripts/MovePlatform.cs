using UnityEngine;
using DG.Tweening;

public class MovePlatform : MonoBehaviour
{
    public Transform targetPlatform, destination;
    public float tweenTime;
    public LoopType loopType;
    public Ease ease;
    private Tween platformTween;

    private void Start()
    {
        //DOMoveY, DOMoveX
        platformTween = targetPlatform.DOMove(destination.position, tweenTime).SetLoops(-1, loopType);
    }
}
