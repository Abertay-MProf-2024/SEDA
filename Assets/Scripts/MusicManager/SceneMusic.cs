using UnityEngine;

public class SceneMusic : MonoBehaviour
{
    public AudioClip backgroundMusic;
    [SerializeField] AudioSource audioSource;

    void Start()
    {
        audioSource.loop = true;
    }
}