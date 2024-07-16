using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingClickSound : MonoBehaviour, IPointerClickHandler
{
    public AudioClip clickSound;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;

        if (clickSound == null)
        {
            Debug.LogWarning("ClickSound is not assigned in " + gameObject.name);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        PlayClickSound();
    }

    public void PlayClickSound()  
    {
        if (clickSound != null)
        {
            audioSource.PlayOneShot(clickSound);
        }
        else
        {
            Debug.LogWarning("ClickSound is null in " + gameObject.name);
        }
    }
}
