using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public class CineCamera : MonoBehaviour
{
    public CinemachineVirtualCameraBase vcam;
    public Transform player;

    private List<CinemachineBasicMultiChannelPerlin> perlins = new List<CinemachineBasicMultiChannelPerlin>();
    private List<float> originalAmps = new List<float>();
    private List<float> originalFreqs = new List<float>();
    private Coroutine shakeCoroutine;

    void Start()
    {
        if (vcam != null && player != null) vcam.Follow = player;
        CollectPerlins();
    }

    void CollectPerlins()
    {
        perlins.Clear();
        originalAmps.Clear();
        originalFreqs.Clear();
        if (vcam == null) return;

        var direct = vcam.GetCinemachineComponent(CinemachineCore.Stage.Noise);
        var perlin = direct as CinemachineBasicMultiChannelPerlin;
        if (perlin != null) perlins.Add(perlin);

        var childPerlins = vcam.GetComponentsInChildren<CinemachineBasicMultiChannelPerlin>(true);
        foreach (var p in childPerlins)
        {
            if (!perlins.Contains(p)) perlins.Add(p);
        }

        foreach (var p in perlins)
        {
            originalAmps.Add(p.AmplitudeGain);
            originalFreqs.Add(p.FrequencyGain);
        }
    }

    public void Shake(float intensity = 1f, float duration = 0.3f, float frequency = 0f)
    {
        if (vcam == null)
        {
            Debug.LogWarning("CineCamera: vcam not assigned.");
            return;
        }

        if (perlins.Count == 0) CollectPerlins();
        if (perlins.Count == 0)
        {
            Debug.LogWarning("CineCamera: No Perlin noise components found on vcam. Add a Noise (CinemachineBasicMultiChannelPerlin) to the virtual camera.");
            return;
        }

        if (intensity <= 0f || duration <= 0f) return;

        if (shakeCoroutine != null) StopCoroutine(shakeCoroutine);
        shakeCoroutine = StartCoroutine(DoShake(intensity, duration, frequency));
    }

    private IEnumerator DoShake(float intensity, float duration, float frequency)
    {
        int n = perlins.Count;
        float[] startAmps = new float[n];
        float[] startFreqs = new float[n];

        for (int i = 0; i < n; i++)
        {
            var p = perlins[i];
            if (p == null) continue;
            startAmps[i] = p.AmplitudeGain;
            startFreqs[i] = p.FrequencyGain;
            if (frequency > 0f) p.FrequencyGain = frequency;
        }

        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            float lerp = Mathf.Lerp(1f, 0f, t); // from peak -> original
            for (int i = 0; i < n; i++)
            {
                var p = perlins[i];
                if (p == null) continue;
                p.AmplitudeGain = startAmps[i] + intensity * lerp;
            }
            yield return null;
        }

        for (int i = 0; i < n; i++)
        {
            var p = perlins[i];
            if (p == null) continue;
            p.AmplitudeGain = startAmps[i];
            p.FrequencyGain = startFreqs[i];
        }

        shakeCoroutine = null;
    }
}