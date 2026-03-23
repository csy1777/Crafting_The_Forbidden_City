using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanelController : MonoBehaviour
{
    public GameObject settingsPanel;      // 设置面板
    public Button openButton;              // 打开设置的按钮
    public Button closeButton;             // 关闭设置的按钮

    void Start()
    {
        // 确保初始状态是隐藏的
        if (settingsPanel != null)
            settingsPanel.SetActive(false);

        // 绑定打开按钮事件
        if (openButton != null)
            openButton.onClick.AddListener(OpenSettings);

        // 绑定关闭按钮事件
        if (closeButton != null)
            closeButton.onClick.AddListener(CloseSettings);
    }

    // 打开设置面板
    public void OpenSettings()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(true);
            Debug.Log("设置面板已打开");
        }
    }

    // 关闭设置面板
    public void CloseSettings()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
            Debug.Log("设置面板已关闭");
        }
    }

    // 切换设置面板（同一个按钮开关）
    public void ToggleSettings()
    {
        if (settingsPanel != null)
        {
            bool isActive = settingsPanel.activeSelf;
            settingsPanel.SetActive(!isActive);
            Debug.Log("设置面板 " + (!isActive ? "打开" : "关闭"));
        }
    }
}
