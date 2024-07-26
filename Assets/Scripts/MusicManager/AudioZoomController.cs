using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class AudioZoomController : MonoBehaviour
{
    [SerializeField] AudioClip windSound;
    [SerializeField] AudioMixerGroup mixerGroupSFX;

    public Camera orthoCam;
    public float fadeDuration = 1.0f;

    [SerializeField] AudioSource musicSource;
    private AudioSource sfxSource;


    private void Awake()
    {
        // Initialize AudioSources
        sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.clip = windSound;
        sfxSource.playOnAwake = false;
        sfxSource.loop = true;
        sfxSource.volume = 0;
        sfxSource.outputAudioMixerGroup = mixerGroupSFX;
    }

    public void PlayWindSound()
    {
        StopAllCoroutines();
        StartCoroutine(FadeIn(sfxSource));
    }

    public void StopWindSound()
    {
        StopAllCoroutines();
        StartCoroutine(FadeOut(sfxSource));
    }

    IEnumerator FadeIn(AudioSource audioSource)
    {
        audioSource.Play();

        while (audioSource.volume < 1)
        {
            audioSource.volume += 0.1f;
            yield return new WaitForSeconds(0.05f);
        }
    }    
    
    IEnumerator FadeOut(AudioSource audioSource)
    {
        while (audioSource.volume > 0)
        {
            audioSource.volume -= 0.1f;
            yield return new WaitForSeconds(0.05f);
        }

        audioSource.Stop();
    }
}