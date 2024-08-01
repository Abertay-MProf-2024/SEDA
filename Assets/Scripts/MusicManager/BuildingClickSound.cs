using System.Collections;
using UnityEngine;

public class BuildingClickSound : MonoBehaviour
{
    public AudioClip clickSound;
    [SerializeField] Transform vfxLocation;
    [SerializeField] GameObject vfxForSound;
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

            if (vfxForSound)
            {
                GameObject vfx = Instantiate(vfxForSound, vfxLocation.position, Quaternion.Euler(0, 0, 0));
                vfx.transform.parent = vfxLocation.transform;
                StartCoroutine(DestroyVFX(vfx));
            }
        }
        else
        {
            Debug.LogWarning("ClickSound is null in " + gameObject.name);
        }
    }

    IEnumerator DestroyVFX(GameObject objectToDestroy)
    {
        yield return new WaitForSeconds(clickSound.length);

        Destroy(objectToDestroy);
    }
}
