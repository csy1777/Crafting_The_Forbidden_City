using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneButtonHandler : MonoBehaviour
{
    [Header("场景配置")]
    public string targetSceneName;  // 要跳转的场景名称

    public void LoadScene()
    {
        if (!string.IsNullOrEmpty(targetSceneName))
        {
            Debug.Log($"加载场景: {targetSceneName}");
            SceneManager.LoadScene(targetSceneName);
        }
        else
        {
            Debug.LogError("未设置目标场景名称！");
        }
    }
}