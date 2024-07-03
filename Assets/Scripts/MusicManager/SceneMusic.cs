using UnityEngine;

public class SceneMusic : MonoBehaviour
{
    public static SceneMusic instance;

    [SerializeField] AudioSource audioSource;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ChangeMusicTrack(AudioClip newMusic)
    {
        audioSource.Stop();
        audioSource.clip = newMusic;
        audioSource.Play();
    }
}