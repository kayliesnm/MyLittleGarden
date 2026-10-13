using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
public class MusicVolumeControl : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider volumeSlider;

    void Start()
    {
        float volume;
        if (audioMixer.GetFloat("MusicVolume", out volume))
        {
            volumeSlider.value = Mathf.Pow(10f, volume / 20f);
        }
        else
        {
            volumeSlider.value = 0.5f;
            audioMixer.SetFloat("MusicVolume", Mathf.Log10(0.5f) * 20f);
        }
    }

    public void SetVolume(float value)
    {
        float volume = Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20f; 
        audioMixer.SetFloat("MusicVolume", volume);
        Debug.Log("Slider changed! Volume set to: " + volume);
    }
}
