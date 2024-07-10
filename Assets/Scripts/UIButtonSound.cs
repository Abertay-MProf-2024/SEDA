using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Audio;
using UnityEditor.SceneManagement;

public class UIButtonSound : MonoBehaviour, IPointerEnterHandler
{

    public AudioClip ClickedSound;
    AudioClip HoverSound;
    [SerializeField] AudioMixerGroup mixerGroup;

    //get button component
    private Button button { get { return GetComponent<Button>(); } }
    // get audiosource
    private AudioSource source { get { return GetComponent<AudioSource>(); } }


    void Start()
    {
        if(ClickedSound != null)
            button.onClick.AddListener(() => PlayClickSoud());
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData == null)
        {
            throw new System.ArgumentNullException(nameof(eventData));
        }

        /*source.clip = HoverSound;
        source.PlayOneShot(HoverSound);*/
    }

    void PlayClickSoud()
    {
        //bind an AudioSource on its
        gameObject.AddComponent<AudioSource>();

        source.outputAudioMixerGroup = mixerGroup;

        source.playOnAwake = false;

        source.clip = ClickedSound;
        
        if (source.enabled)
        {
            source.PlayOneShot(ClickedSound);
            Destroy(source, ClickedSound.length);
        }
    }
}