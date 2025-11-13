using UnityEngine;
using UnityEngine.Audio;
using DG.Tweening;

public class SoundList : MonoBehaviour
{
   public Sound [] soundList;
    private float minPitch = 0.85f;
    private float maxPitch = 1.2f;

    void Start()
    {
        for (int i = 0; i < soundList.Length; i++)
        {
            AudioSource audioSorce = gameObject.AddComponent<AudioSource>();
            soundList[i].SetAudioSorce(audioSorce);
        }
    }

    public void PlaySound(string audioName)
    {
        Sound sound = FindSound(audioName);
        sound.source.Stop();
    }

   public void PlayRandomPitch(string audioName)
    {
        Sound sound = FindSound(audioName);
        sound.source.pitch = Random.Range(minPitch, maxPitch);
        sound.source.Play();
    }

    public void StopSound(string audioName)
    {
        Sound sound = FindSound(audioName);
        sound.source.Stop();
    }

    public void SoundFadeIn(string audioName, float fadeTime)
    {
        Sound sound = FindSound(audioName);
        sound.volume = 0;
        sound.source.Play();
        sound.source.DOFade(1, fadeTime);
    }
    public Sound FindSound(string soundName)
    {
        for (int i = 0; i < soundList.Length; i++)
        {
            if (soundName == soundList[i].audioName)
            {
                return soundList[i];
            }
        }
        return null;
    }

    void Update()
    {
        
    }
}
[System.Serializable]
public class Sound
{
    public string audioName;
    public AudioClip audioClip;
    public float volume;
    public float pitch;

    public AudioMixerGroup mixer;
    [HideInInspector]
    public AudioSource source;

    public void SetAudioSorce(AudioSource audioSource)
    {
        source = audioSource;
        source.clip = audioClip;
        source.volume = volume;
        source.pitch = pitch;
        source.outputAudioMixerGroup = mixer;
        source.playOnAwake = false;
    }
}