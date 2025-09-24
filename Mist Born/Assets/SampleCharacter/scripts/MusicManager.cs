using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    [Header("Music Clips")]
    public AudioClip mainTheme;
    public AudioClip battleTheme;
    public AudioClip victoryTheme;

    public AudioClip monsterWalkingInDistance;
    public AudioClip MonsterJumping;
    public AudioClip MonsterFalling;
    public AudioClip MonsterSlam;
    public AudioClip MonsterFireThrower;
    public AudioClip MonsterDeath;

    public AudioSource audioSource;
    public AudioSource sfxSource;
    private Coroutine fadeOutCoroutine;

    private void Awake()
    {
        // Singleton pattern to keep one MusicManager across scenes
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Setup AudioSource
        //audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.loop = true;
    }

    private void Start()
    {
        PlayMusic(MusicManager.Instance.battleTheme);
        audioSource.volume = 0;
        PlayMusic(mainTheme);
    }

    public void PlayMusic(AudioClip clip)
    {
        if (clip == null) return;

        // Cancel fade out if already running
        if (fadeOutCoroutine != null)
        {
            StopCoroutine(fadeOutCoroutine);
            fadeOutCoroutine = null;
        }

        if (audioSource.clip == clip && audioSource.isPlaying)
            return; // Already playing this music

        audioSource.volume = 1f;
        audioSource.clip = clip;
        audioSource.Play();
    }

    public void StopMusic()
    {
        if (fadeOutCoroutine != null)
        {
            StopCoroutine(fadeOutCoroutine);
            fadeOutCoroutine = null;
        }

        audioSource.Stop();
        audioSource.clip = null;
    }

    public void StopMusic(float duration)
    {
        if (fadeOutCoroutine != null)
            StopCoroutine(fadeOutCoroutine);

        fadeOutCoroutine = StartCoroutine(FadeOutAndStop(duration));
    }

    private IEnumerator FadeOutAndStop(float duration)
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0.01f)
        {
            audioSource.volume -= startVolume * (Time.deltaTime / duration);
            yield return null;
        }

        // Make sure volume hits 0 exactly
        audioSource.volume = 0f;

        // Now stop playback
        audioSource.Stop();
        audioSource.clip = null;

        // Reset for next time
        audioSource.volume = 1f;
        fadeOutCoroutine = null;
    }


    public void PlaySFX(AudioClip clip, float volume = 1f)
    {
        if (clip == null) return;
        sfxSource.PlayOneShot(clip, volume);
    }
}
