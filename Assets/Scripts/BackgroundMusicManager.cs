using System.Collections;
using UnityEngine;

// Persistent manager for background music playback.
[RequireComponent(typeof(AudioSource))]
public class BackgroundMusicManager : MonoBehaviour
{
    public static BackgroundMusicManager Instance { get; private set; }

    [Header("Audio Config")]
    public AudioClip musicClip;
    public float maxVolume = 0.5f;
    public float fadeDuration = 2f;

    private AudioSource _audioSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeAudio();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (Instance == this && !_audioSource.isPlaying)
        {
            BeginPlayback();
        }
    }

    private void InitializeAudio()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.clip = musicClip;
        _audioSource.loop = true;
        _audioSource.volume = 0;
    }

    public void BeginPlayback()
    {
        if (_audioSource.clip != null)
        {
            _audioSource.Play();
            StartCoroutine(FadeInVolume());
        }
    }

    private IEnumerator FadeInVolume()
    {
        float timer = 0;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            _audioSource.volume = Mathf.Lerp(0, maxVolume, timer / fadeDuration);
            yield return null;
        }
        _audioSource.volume = maxVolume;
    }
}