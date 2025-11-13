using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour
{
    public Slider masterSlider, musicSlider, sfxSlider;
    public AudioMixerGroup master, music, sfx;

    private string masterVolume = "MasterVolume";
    private string musicVolume = "MusicVolume";
    private string sfxVolume = "SfxVolume";

    void Start()
    {
        float _masterVolume = 0;
        float _sfxVolume = 0;
        float _musicVolume = 0;
        master.audioMixer.GetFloat(masterVolume, out _masterVolume);
        music.audioMixer.GetFloat(musicVolume, out _musicVolume);
        sfx.audioMixer.GetFloat(sfxVolume, out _sfxVolume);
        _masterVolume = masterSlider.value;
        _musicVolume = musicSlider.value;
        _sfxVolume = sfxSlider.value;

    }

    public void SetMasterVolume(float volume)
    {
        master.audioMixer.SetFloat(masterVolume, volume);
    }
    public void SetMusicVolume(float volume)
    {
        music.audioMixer.SetFloat(musicVolume, volume);
    }
    public void SetSfxVolume(float volume)
    {
        sfx.audioMixer.SetFloat(sfxVolume, volume);
    }
}
