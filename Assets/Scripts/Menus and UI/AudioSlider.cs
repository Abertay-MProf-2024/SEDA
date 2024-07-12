using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSlider : MonoBehaviour
{
    [SerializeField]
    Slider volumeSlider;

    [SerializeField]
    AudioMixer mixer;

    [SerializeField]
    string audioGroup;

    private void Start()
    {
        Load();

        if (mixer)
        {
            mixer.SetFloat(audioGroup, GetDecibelLevel(volumeSlider.value));
        }
    }

    public void ChangeVolume()
    {
        mixer.SetFloat(audioGroup, GetDecibelLevel(volumeSlider.value));
        Save();
    }

    /* 
     * Get the decibel level from a float with a range of 0 to 1
     * These equations remap the volume value so that from 0 to 0.5 maps to a decibel level of -80 to 0
     * and a volume from 0.5 to 1 maps to decibel level from 0 to 20
     * This results in a more even change in sound when moving the volume slider
     */
    public float GetDecibelLevel(float volume)
    {
        if (volume >= 0.5)
        {
            return 0 + (20 / (1 - 0.5f)) * (volume - 0.5f);
        }
        else
        {
            return -80 + (80 / 0.5f) * volume;
        }
    }

    void Load()
    {
        volumeSlider.value = PlayerPrefs.GetFloat(audioGroup);
    }

    void Save()
    {
        PlayerPrefs.SetFloat(audioGroup, volumeSlider.value);
    }
}
