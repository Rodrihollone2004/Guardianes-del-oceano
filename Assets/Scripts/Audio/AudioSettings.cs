using UnityEngine;
using UnityEngine.Audio;

public class AudioSettings : MonoBehaviour
{
    public static AudioSettings Instance { get; private set; }

    [Header("References")]
    [SerializeField] private AudioMixer mainMixer;

    private const string MASTER_KEY = "MasterVol";
    private const string MUSIC_KEY = "MusicVol";
    private const string SFX_KEY = "SFXVol";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        LoadVolumeSettings();
    }

    public void SetMasterVolume(float value)
    {
        ApplyVolume(MASTER_KEY, value);
        PlayerPrefs.SetFloat(MASTER_KEY, value);
        PlayerPrefs.Save();
    }

    public void SetMusicVolume(float value)
    {
        ApplyVolume(MUSIC_KEY, value);
        PlayerPrefs.SetFloat(MUSIC_KEY, value);
        PlayerPrefs.Save();
    }

    public void SetSFXVolume(float value)
    {
        ApplyVolume(SFX_KEY, value);
        PlayerPrefs.SetFloat(SFX_KEY, value);
        PlayerPrefs.Save();
    }

    private void ApplyVolume(string parameterName, float value)
    {
        float dbValue = Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20;
        mainMixer.SetFloat(parameterName, dbValue);
    }

    private void LoadVolumeSettings()
    {
        ApplyVolume(MASTER_KEY, PlayerPrefs.GetFloat(MASTER_KEY, 0.75f));
        ApplyVolume(MUSIC_KEY, PlayerPrefs.GetFloat(MUSIC_KEY, 0.75f));
        ApplyVolume(SFX_KEY, PlayerPrefs.GetFloat(SFX_KEY, 0.75f));
    }

    public float GetMasterVolume() => PlayerPrefs.GetFloat(MASTER_KEY, 0.75f);
    public float GetMusicVolume() => PlayerPrefs.GetFloat(MUSIC_KEY, 0.75f);
    public float GetSFXVolume() => PlayerPrefs.GetFloat(SFX_KEY, 0.75f);
}
