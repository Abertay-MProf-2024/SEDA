using UnityEngine;
using System.Collections;

public class AudioZoomController : MonoBehaviour
{
    public AudioClip musicClip1;
    public AudioClip musicClip2;
    public Camera orthoCam;
    public float fadeDuration = 1.0f;

    private AudioSource audioSource1;
    private AudioSource audioSource2;

    private float minZoom;
    private float maxZoom;
    private bool isMusic2AtMaxVolume = false;
    private Coroutine music2FadeCoroutine;
    private float music2OriginalVolume;

    void Start()
    {
        // Initialize AudioSources
        audioSource1 = gameObject.AddComponent<AudioSource>();

        audioSource2 = gameObject.AddComponent<AudioSource>();

        if (orthoCam != null)
        {
            CameraPan cameraPan = orthoCam.GetComponent<CameraPan>();
            minZoom = cameraPan.minZoomDistance;
            maxZoom = cameraPan.maxZoomDistance;
        }
    }

    void Update()
    {
        if (orthoCam != null)
        {
            float zoomLevel = orthoCam.orthographicSize;

            float volume1 = Mathf.InverseLerp(maxZoom - 1, minZoom, zoomLevel);  // »º³åÇø¼ä
            float volume2 = Mathf.InverseLerp(minZoom + 3, maxZoom, zoomLevel);

            StartCoroutine(FadeAudioSource.StartFade(audioSource1, fadeDuration, volume1));

            if (volume2 >= 0.95f && !isMusic2AtMaxVolume)
            {
                isMusic2AtMaxVolume = true;
                music2OriginalVolume = volume2;
                if (music2FadeCoroutine != null)
                {
                    StopCoroutine(music2FadeCoroutine);
                }
                music2FadeCoroutine = StartCoroutine(WaitAndLowerVolume(audioSource2, 5.0f, volume2 * 0.4f));
            }
            else if (volume2 < 0.95f)
            {
                isMusic2AtMaxVolume = false;
                if (music2FadeCoroutine != null)
                {
                    StopCoroutine(music2FadeCoroutine);
                }
                StartCoroutine(FadeAudioSource.StartFade(audioSource2, fadeDuration, volume2));
            }
        }
    }

    private IEnumerator WaitAndLowerVolume(AudioSource audioSource, float waitTime, float targetVolume)
    {
        yield return new WaitForSeconds(waitTime);
        StartCoroutine(FadeAudioSource.StartFade(audioSource, fadeDuration, targetVolume));
    }
}

public static class FadeAudioSource
{
    public static IEnumerator StartFade(AudioSource audioSource, float duration, float targetVolume)
    {
        float currentTime = 0;
        float startVolume = audioSource.volume;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, targetVolume, currentTime / duration);
            yield return null;
        }
        yield break;
    }
}