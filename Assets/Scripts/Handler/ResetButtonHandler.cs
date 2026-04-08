using UnityEngine;
using UnityEngine.UI;

public class ResetButtonHandler : MonoBehaviour
{
    void Start()
    {
        Button btn = GetComponent<Button>();
        if (btn != null)
        {
            // 添加点击事件
            btn.onClick.AddListener(OnResetButtonClick);
        }
    }

    void OnResetButtonClick()
    {
        if (GameProgressManager.Instance != null)
        {
            GameProgressManager.Instance.ResetAllProgress();
            Debug.Log("游戏进度已重置");
            SceneSelectManager sceneSelect = FindObjectOfType<SceneSelectManager>();
            if (sceneSelect != null)
            {
                // 重新加载场景来刷新
                UnityEngine.SceneManagement.SceneManager.LoadScene(
                    UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
                );
            }
        }
        else
        {
            Debug.LogError("GameProgressManager未找到！");
        }
    }
}