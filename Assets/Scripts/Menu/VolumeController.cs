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
        // 加载保存的音量设置
        float savedVolume = PlayerPrefs.GetFloat(playerPrefsKey, 0.8f);

        // 设置AudioListener音量
        AudioListener.volume = savedVolume;

        // 设置Slider的值
        if (volumeSlider != null)
        {
            volumeSlider.value = savedVolume;

            // 添加监听事件
            volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
        }

        // 更新显示文本
        UpdateVolumeText(savedVolume);
    }

    // 音量改变时的回调
    void OnVolumeChanged(float value)
    {
        // 设置主音量
        AudioListener.volume = value;

        // 保存设置
        PlayerPrefs.SetFloat(playerPrefsKey, value);
        PlayerPrefs.Save();

        // 更新显示文本
        UpdateVolumeText(value);
    }

    void UpdateVolumeText(float volume)
    {
        if (volumeText != null)
        {
            // 显示为百分比
            volumeText.text = Mathf.RoundToInt(volume * 100) + "%";
        }
    }

    // 静音切换功能
    private bool isMuted = false;
    private float lastVolume;

    public void ToggleMute()
    {
        if (isMuted)
        {
            // 取消静音，恢复之前的音量
            AudioListener.volume = lastVolume;
            if (volumeSlider != null)
                volumeSlider.value = lastVolume;
            isMuted = false;
        }
        else
        {
            // 静音，保存当前音量
            lastVolume = AudioListener.volume;
            AudioListener.volume = 0f;
            if (volumeSlider != null)
                volumeSlider.value = 0f;
            isMuted = true;
        }
    }
}

