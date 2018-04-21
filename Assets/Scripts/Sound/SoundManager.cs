using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
using Random = UnityEngine.Random;

public class SoundManager : MonoSingleton<SoundManager>
{
    [Header("References")]
    [SerializeField] private SoundContext _soundContext;
    [SerializeField] private AudioMixerGroup _sfxMixer;

    [Header("Settings")]
    [Tooltip("The amount of audio sources the SoundManager creates on start")]
    [SerializeField] private int _sfxAudioSourceCount;
    [SerializeField] private Vector2 _pitchDomain = Vector2.one;

    public bool Muted
    {
        get { return PlayerPrefs.GetInt(PlayerPreferenceLiterals.MuteSound, 0) != 0; }
        set
        {
            MutateSfxAudioSources(audioSource => audioSource.mute = value);
            PlayerPrefs.SetInt(PlayerPreferenceLiterals.MuteSound, value ? 1 : 0);
        }
    }

    private readonly List<AudioSource> _sfxAudioSources = new List<AudioSource>();

    private void Awake()
    {
        DefineSingleton(this, true);
    }

    private void Start()
    {
        InitializeSfxAudioSources();
    }

    public void Play(Sound sound)
    {
        StartCoroutine(PlaySfx(sound));
    }

    public void Play(Sound sound, float pitch)
    {
        StartCoroutine(PlaySfx(sound, pitch));
    }

    private void InitializeSfxAudioSources()
    {
        for (var i = 0; i < _sfxAudioSourceCount; i++)
        {
            CreateNewSfxAudioSource();
        }
    }

    private void MutateSfxAudioSources(Action<AudioSource> action)
    {
        _sfxAudioSources.ForEach(action.Invoke);
    }

    private IEnumerator PlaySfx(Sound sound)
    {
        var availableAudioSource = _sfxAudioSources.FirstOrDefault(audioSource => audioSource.clip == null) ??
                                   CreateNewSfxAudioSource();

        var availableClips = _soundContext.Map.First(item => item.Sound == sound).Clips;
        availableAudioSource.clip = availableClips[Random.Range(0, availableClips.Length)];
        availableAudioSource.pitch = Random.Range(_pitchDomain.x, _pitchDomain.y);
        availableAudioSource.Play();

        yield return new WaitUntil(() => !availableAudioSource.isPlaying);

        availableAudioSource.clip = null;
    }

    private IEnumerator PlaySfx(Sound sound, float pitch)
    {
        var availableAudioSource = _sfxAudioSources.FirstOrDefault(audioSource => audioSource.clip == null) ??
                                   CreateNewSfxAudioSource();

        var availableClips = _soundContext.Map.First(item => item.Sound == sound).Clips;
        availableAudioSource.clip = availableClips[Random.Range(0, availableClips.Length)];
        availableAudioSource.pitch = pitch;
        availableAudioSource.Play();

        yield return new WaitUntil(() => !availableAudioSource.isPlaying);

        availableAudioSource.clip = null;
    }

    private AudioSource CreateNewSfxAudioSource()
    {
        var sfxAudioSource = gameObject.AddComponent<AudioSource>();
        sfxAudioSource.playOnAwake = false;
        sfxAudioSource.outputAudioMixerGroup = _sfxMixer;
        sfxAudioSource.mute = Muted;

        _sfxAudioSources.Add(sfxAudioSource);

        return sfxAudioSource;
    }
}
