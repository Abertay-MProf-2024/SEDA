using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BuildingPlacementSound : MonoBehaviour, IPointerDownHandler
{
    public AudioClip placeBuildingSound;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // Play the placement sound
        PlayPlaceBuildingSound();

        // Call the method to place the building
        PlaceBuilding();
    }

    void PlayPlaceBuildingSound()
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

    void PlaceBuilding()
    {
        // Assume you have a reference to your Building script
        Building buildingScript = GetComponent<Building>();

        if (buildingScript != null)
        {
            // Your logic to place the building, e.g.:
            // buildingScript.PlaceOnTile();
            Debug.Log("Building placed on tile."); // Placeholder for actual placement logic
        }
        else
        {
            Debug.LogWarning("Building script not found on " + gameObject.name);
        }
    }
}
