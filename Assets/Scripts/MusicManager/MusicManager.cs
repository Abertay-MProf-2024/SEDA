using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance = null; // Singleton instance
    private AudioSource audioSource;

    public float fadeDuration = 1.0f; // Fade in/out duration

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Don't destroy on load new scene
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.loop = true; // Loop the background music
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }

    public void PlayMusic(AudioClip musicClip)
    {
        StartCoroutine(FadeInMusic(musicClip));
    }

    private IEnumerator FadeInMusic(AudioClip newClip)
    {
        if (audioSource.isPlaying)
        {
            yield return StartCoroutine(FadeOut());
        }
        audioSource.clip = newClip;
        audioSource.Play();
        yield return StartCoroutine(FadeIn());
    }

    private IEnumerator FadeOut()
    {
        float startVolume = audioSource.volume;
        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }
        audioSource.Stop();
        audioSource.volume = startVolume;
    }

    private IEnumerator FadeIn()
    {
        float targetVolume = audioSource.volume;
        audioSource.volume = 0;
        while (audioSource.volume < targetVolume)
        {
            audioSource.volume += targetVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }
        audioSource.volume = targetVolume;
    }
}