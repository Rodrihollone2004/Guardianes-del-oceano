using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Mixer Groups")]
    [SerializeField] private AudioMixerGroup musicGroup;
    [SerializeField] private AudioMixerGroup sfxGroup;

    [Header("Music & Sounds Clips")]
    [SerializeField] private AudioClip musicClip;
    [SerializeField] private AudioClip buttonHover;

    private AudioSource _musicSource;
    private AudioSource _sfxSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeSystem();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeSystem()
    {
        _musicSource = gameObject.AddComponent<AudioSource>();
        _musicSource.outputAudioMixerGroup = musicGroup;
        _musicSource.loop = true;
        _musicSource.playOnAwake = false;

        _sfxSource = gameObject.AddComponent<AudioSource>();
        _sfxSource.outputAudioMixerGroup = sfxGroup;
        _sfxSource.playOnAwake = false;

        if (musicClip != null)
            PlayMusic(musicClip);
    }

    public void PlaySFX2D(AudioClip clip)
    {
        if (clip == null) return;

        _sfxSource.PlayOneShot(clip);
    }

    public void PlayButtonHover()
    {
        if (buttonHover == null) return;

        _sfxSource.PlayOneShot(buttonHover);
    }

    public void PlayMusic(AudioClip clip)
    {
        if (clip == null || _musicSource.clip == clip) return;

        _musicSource.clip = clip;
        _musicSource.Play();
    }

    public void StopMusic()
    {
        _musicSource.Stop();
        _musicSource.clip = null;
    }
}
