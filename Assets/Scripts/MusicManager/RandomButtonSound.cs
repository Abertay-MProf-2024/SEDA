using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Audio;

public class RandomButtonSound : MonoBehaviour, IPointerClickHandler
{
    public AudioClip[] clickSounds; // Array of audio clips for click sounds
    [SerializeField] AudioMixerGroup mixerGroup;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.outputAudioMixerGroup = mixerGroup;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (clickSounds != null && clickSounds.Length > 0)
        {
            int randomIndex = Random.Range(0, clickSounds.Length); // Get a random index
            AudioClip randomClip = clickSounds[randomIndex]; // Get a random clip

            PlayClickSound(randomClip);
        }
    }

    void PlayClickSound(AudioClip clip)
    {
        if (clip)
        {
            GameObject tempAudioSource = new GameObject("TempAudio");
            AudioSource tempSource = tempAudioSource.AddComponent<AudioSource>();

            tempSource.outputAudioMixerGroup = mixerGroup;
            tempSource.clip = clip;
            tempSource.playOnAwake = false;
            tempSource.Play();

            // Destroy the temp audio source object after the clip duration
            Destroy(tempAudioSource, clip.length);
        }
    }
}
