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

    private float minZoom;
    private float maxZoom;
    private bool isMusic2AtMaxVolume = false;
    private Coroutine music2FadeCoroutine;
    private float music2OriginalVolume;

    private void Awake()
    {
        // Initialize AudioSources
        sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.clip = windSound;
        sfxSource.playOnAwake = false;
        sfxSource.loop = true;
        sfxSource.outputAudioMixerGroup = mixerGroupSFX;
    }

    public void PlayWindSound()
    {
        sfxSource.Play();
    }

    public void StopWindSound()
    {
        sfxSource.Stop();
    }
}