using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AudioUIController : MonoBehaviour
{
    [Header("UI Sliders")]
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    [Header("UI Value Texts")]
    [SerializeField] private TextMeshProUGUI masterText;
    [SerializeField] private TextMeshProUGUI musicText;
    [SerializeField] private TextMeshProUGUI sfxText;

    private void Start()
    {
        float currentMaster = 0f;
        float currentMusic = 0f;
        float currentSFX = 0f;
        if (AudioSettings.Instance != null)
        {
            currentMaster = AudioSettings.Instance.GetMasterVolume();
            currentMusic = AudioSettings.Instance.GetMusicVolume();
            currentSFX = AudioSettings.Instance.GetSFXVolume();
        }

        masterSlider.value = currentMaster;
        musicSlider.value = currentMusic;
        sfxSlider.value = currentSFX;

        UpdateText(masterText, currentMaster);
        UpdateText(musicText, currentMusic);
        UpdateText(sfxText, currentSFX);

        masterSlider.onValueChanged.AddListener(OnMasterSliderChanged);
        musicSlider.onValueChanged.AddListener(OnMusicSliderChanged);
        sfxSlider.onValueChanged.AddListener(OnSFXSliderChanged);
    }

    private void OnMasterSliderChanged(float value)
    {
        AudioSettings.Instance.SetMasterVolume(value);
        UpdateText(masterText, value);
    }

    private void OnMusicSliderChanged(float value)
    {
        AudioSettings.Instance.SetMusicVolume(value);
        UpdateText(musicText, value);
    }

    private void OnSFXSliderChanged(float value)
    {
        AudioSettings.Instance.SetSFXVolume(value);
        UpdateText(sfxText, value);
    }

    private void UpdateText(TextMeshProUGUI textElement, float value)
    {
        if (textElement != null)
        {
            int percentage = Mathf.RoundToInt(value * 100);
            textElement.text = percentage.ToString();
        }
    }
}
