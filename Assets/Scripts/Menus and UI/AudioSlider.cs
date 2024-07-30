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
     * This equation remaps the volume value so that from 0 to 1 maps to a decibel level of -40 to 0
     * Equation: output = output_start + ((output_end - output_start) / (input_end - input_start)) * (input - input_start)
     */
    public float GetDecibelLevel(float volume)
    {
        if (volume < 0.1f)
            return -80;

        return -40 + (10 + 40) * volume;
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
