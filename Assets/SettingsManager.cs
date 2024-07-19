using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider soundSlider;

    void Start()
    {
        if (AudioManager.Instance != null)
        {
            // Set initial slider values based on AudioManager settings
            musicSlider.value = AudioManager.Instance.backgroundMusic.volume;
            soundSlider.value = AudioManager.Instance.footballKicked.volume;

            // Add listeners to sliders
            musicSlider.onValueChanged.AddListener(OnMusicSliderChanged);
            soundSlider.onValueChanged.AddListener(OnSoundSliderChanged);
        }
    }

    private void OnMusicSliderChanged(float value)
    {
        AudioManager.SetMusicVolume(value);
        AudioManager.isMusicOn = value > 0f;
        AudioManager.Save();
    }

    private void OnSoundSliderChanged(float value)
    {
        AudioManager.SetSFXVolume(value);
        AudioManager.isSFXOn = value > 0f;
        AudioManager.Save();
    }
}
