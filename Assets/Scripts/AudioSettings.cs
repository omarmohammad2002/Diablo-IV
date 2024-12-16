using UnityEngine;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
    public Slider volumeSlider;

    void Start()
    {
        if (AudioManager.Instance != null)
        {
            volumeSlider.value = AudioManager.Instance.musicSource.volume;
            volumeSlider.onValueChanged.AddListener(UpdateVolume);
        }
    }

    public void UpdateVolume(float volume)
    {
        AudioManager.Instance.SetMusicVolume(volume);
    }
}
