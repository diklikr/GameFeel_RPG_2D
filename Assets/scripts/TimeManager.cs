using DG.Tweening;
using System.Collections;
using System.Data;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public float targetTimescale = 1;
    public bool gamePaused;
    public CanvasGroup pauseMenu;
    private Tween pauseTween;
    private float pauseTweenTime = 0.5f;

    public static TimeManager instance;

    private Coroutine freezeFrameCoroutine;
    [SerializeField] private KeyCode pauseKey;


    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        targetTimescale = 1;
        gamePaused = false;
        pauseMenu.alpha = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(pauseKey))
        {
            OnPause(!gamePaused);
            
        }
    }

    public void SetTime(float timescale)
    {
        targetTimescale = timescale;
        Time.timeScale = targetTimescale;
    }
    public void OnPause(bool pause)
    {
        if(freezeFrameCoroutine != null)
        {
            StopCoroutine(freezeFrameCoroutine);
        }
        gamePaused = pause;
        pauseTween?.Kill();
        if(pause)
        {
            SetTime(0);
            pauseTween = pauseMenu.DOFade(1, pauseTweenTime).SetUpdate(true);
            //pauseMenu.alpha = 1;
        }
        else
        {
            //pauseMenu.alpha = 0;
            pauseTween = pauseMenu.DOFade(0, pauseTweenTime).OnComplete(()=> { SetTime(1); Debug.Log("WAZA"); }).SetUpdate(true);
        }
    }
    public void FreezeFrame(float timeScale, float duration)
    {
        if(freezeFrameCoroutine != null)
        {
            StopCoroutine(FreezeFrameRoutine(timeScale, duration));
        }
        freezeFrameCoroutine = StartCoroutine(FreezeFrameRoutine(timeScale, duration));
    }

    IEnumerator FreezeFrameRoutine(float timeScale,float duration)
    {
        SetTime(timeScale);
        yield return new WaitForSecondsRealtime(duration);
        SetTime(1);

    }
}
