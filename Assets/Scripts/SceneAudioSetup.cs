using UnityEngine;

public class SceneAudioSetup : MonoBehaviour
{
    public AudioSource musicSource;
    public AudioSource[] soundEffectSources;

    void Start()
    {
        if (AudioManager.Instance != null)
        {
            if (musicSource != null)
            {
                AudioManager.Instance.RegisterMusicSource(musicSource);
                Debug.Log("Scene Music Registered: " + musicSource.clip.name);
            }

            foreach (var source in soundEffectSources)
            {
                AudioManager.Instance.RegisterSoundEffect(source);
            }
        }
    }
}
