using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioSource currentMusicSource;
    private List<AudioSource> soundEffectSources = new List<AudioSource>();

    private float musicVolume = 1f;
    private float effectsVolume = 1f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        effectsVolume = PlayerPrefs.GetFloat("EffectsVolume", 1f);
    }

    public void RegisterMusicSource(AudioSource source)
    {
        if (currentMusicSource != null && currentMusicSource != source && currentMusicSource.isPlaying)
        {
            currentMusicSource.Stop();
        }

        currentMusicSource = source;
        currentMusicSource.volume = musicVolume;

        if (!currentMusicSource.isPlaying)
        {
            currentMusicSource.Play();
        }
    }


    public void RegisterSoundEffect(AudioSource source)
    {
        if (!soundEffectSources.Contains(source))
        {
            soundEffectSources.Add(source);
            source.volume = effectsVolume;
        }
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;
        if (currentMusicSource != null)
        {
            currentMusicSource.volume = volume;
        }
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }

    public void SetEffectsVolume(float volume)
    {
        effectsVolume = volume;
        foreach (AudioSource source in soundEffectSources)
        {
            if (source != null)
            {
                source.volume = volume;
            }
        }
        PlayerPrefs.SetFloat("EffectsVolume", volume);
    }

    public float GetMusicVolume() => musicVolume;
    public float GetEffectsVolume() => effectsVolume;
}
