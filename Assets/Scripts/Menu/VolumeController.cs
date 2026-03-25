using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    public Slider volumeSlider;
    public Text volumeText;
    public string playerPrefsKey = "MasterVolume";

    void Start()
    {

        float savedVolume = PlayerPrefs.GetFloat(playerPrefsKey, 0.8f);


        AudioListener.volume = savedVolume;


        if (volumeSlider != null)
        {
            volumeSlider.value = savedVolume;

            volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
        }


        UpdateVolumeText(savedVolume);
    }


    void OnVolumeChanged(float value)
    {

        AudioListener.volume = value;

        PlayerPrefs.SetFloat(playerPrefsKey, value);
        PlayerPrefs.Save();

        UpdateVolumeText(value);
    }

    void UpdateVolumeText(float volume)
    {
        if (volumeText != null)
        {
            volumeText.text = Mathf.RoundToInt(volume * 100) + "%";
        }
    }

    private bool isMuted = false;
    private float lastVolume;

    public void ToggleMute()
    {
        if (isMuted)
        {
            AudioListener.volume = lastVolume;
            if (volumeSlider != null)
                volumeSlider.value = lastVolume;
            isMuted = false;
        }
        else
        {
            lastVolume = AudioListener.volume;
            AudioListener.volume = 0f;
            if (volumeSlider != null)
                volumeSlider.value = 0f;
            isMuted = true;
        }
    }
}
