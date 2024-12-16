using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public AudioSource musicSource;

    void Start()
    {
        if (musicSource != null)
        {
            musicSource.volume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        }
    }

    public void SetMusicVolume(float volume)
    {
        if (musicSource != null)
        {
            musicSource.volume = volume;
            PlayerPrefs.SetFloat("MusicVolume", volume);
        }
    }

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
    }
}
