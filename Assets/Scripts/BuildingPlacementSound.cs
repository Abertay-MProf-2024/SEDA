using UnityEngine;

public class BuildingPlacementSound : MonoBehaviour
{
    public AudioClip placeBuildingSound;
    private AudioSource audioSource;

    void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    public void PlayPlaceBuildingSound()
    {
        if (placeBuildingSound != null)
        {
            audioSource.PlayOneShot(placeBuildingSound);
        }
        else
        {
            Debug.LogWarning("PlaceBuildingSound is null in " + gameObject.name);
        }
    }
}
