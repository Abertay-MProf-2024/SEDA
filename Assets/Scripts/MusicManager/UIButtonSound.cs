using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Audio;

public class UIButtonSound : MonoBehaviour, IPointerEnterHandler
{
    public AudioClip ClickedSound;
    AudioClip HoverSound;
    [SerializeField] AudioMixerGroup mixerGroup;

    //get button component
    private Button button { get { return GetComponent<Button>(); } }

    void Start()
    {
        if (ClickedSound != null)
            button.onClick.AddListener(PlayClickSound);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData == null)
        {
            throw new System.ArgumentNullException(nameof(eventData));
        }

        // Uncomment if you need hover sound functionality
        /*
        source.clip = HoverSound;
        source.PlayOneShot(HoverSound);
        */
    }

    void PlayClickSound()
    {
        if (ClickedSound != null)
        {
            GameObject tempAudioSource = new GameObject("TempAudio");
            AudioSource audioSource = tempAudioSource.AddComponent<AudioSource>();

            audioSource.outputAudioMixerGroup = mixerGroup;
            audioSource.clip = ClickedSound;
            audioSource.playOnAwake = false;
            audioSource.Play();

            // Destroy the temp audio source object after the clip duration
            Destroy(tempAudioSource, ClickedSound.length);
        }
    }
}
