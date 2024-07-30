using UnityEngine;

public class BuildingClickSound : MonoBehaviour
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

    public void PlayClickSound()  
    {
        if (audioSource && clickSound != null)
        {
            audioSource.PlayOneShot(clickSound);
        }
        else
        {
            Debug.LogWarning("ClickSound is null in " + gameObject.name);
        }
    }
}
