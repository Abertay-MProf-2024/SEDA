using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class AudioZoomController : MonoBehaviour
{
    [SerializeField] AudioClip windSound;
    [SerializeField] AudioMixerGroup mixerGroupSFX;

    public Camera orthoCam;

    [SerializeField] AudioSource musicSource;
    private AudioSource sfxSource;

    private float minZoom;
    private float maxZoom;

    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

        // Initialize AudioSources
        sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.clip = windSound;
        sfxSource.playOnAwake = false;
        sfxSource.loop = true;
        sfxSource.outputAudioMixerGroup = mixerGroupSFX;
    }

    void Update()
    {
        if (orthoCam != null && orthoCam.orthographic)
        {
            float zoomLevel = orthoCam.orthographicSize;

            float volume1 = Mathf.InverseLerp(maxZoom - 1, minZoom, zoomLevel);  // »º³åÇø¼ä
            float volume2 = Mathf.InverseLerp(minZoom + 3, maxZoom, zoomLevel);

            musicSource.volume = volume1;
            sfxSource.volume = volume2;

            if (volume2 >= 0.95f && !sfxSource.isPlaying)
            {
                sfxSource.volume = volume2 * 0.4f;
                sfxSource.Play();
            }
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        sfxSource.Play();

        orthoCam = FindAnyObjectByType<Camera>();

        InputManager cameraPan;
        if (orthoCam != null && (cameraPan = orthoCam.GetComponent<InputManager>()))
        {
            minZoom = cameraPan.minZoomDistance;
            maxZoom = cameraPan.maxZoomDistance;
        }
    }
}
