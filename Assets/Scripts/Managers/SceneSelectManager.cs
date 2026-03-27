using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneSelectManager : MonoBehaviour
{
    [Header("场景按钮Image配置")]
    public Image taiheGateImage;      // 太和门Image
    public Image taiheHallImage;      // 太和殿Image
    public Image zhongheHallImage;    // 中和殿Image
    public Image baoheHallImage;      // 保和殿Image

    [Header("按钮配置")]
    public Button taiheGateButton;     // 太和门按钮
    public Button taiheHallButton;     // 太和殿按钮
    public Button zhongheHallButton;   // 中和殿按钮
    public Button baoheHallButton;     // 保和殿按钮

    [Header("颜色配置")]
    public Color completedColor = Color.green;   // 完成后颜色
    public Color lockedColor = Color.gray;       // 未解锁颜色
    public Color unlockedColor = Color.white;    // 已解锁但未完成颜色

    void Start()
    {
        // 初始化所有Image颜色
        LoadProgressAndUpdateImages();
    }

    void LoadProgressAndUpdateImages()
    {
        if (GameProgressManager.Instance == null)
        {
            Debug.LogError("GameProgressManager未找到！");
            return;
        }

        // 更新太和门Image
        if (taiheGateImage != null)
        {
            bool isCompleted = GameProgressManager.Instance.IsPuzzleCompleted("TaiheMenPuzzleScene");
            taiheGateImage.color = isCompleted ? completedColor : unlockedColor;

            if (taiheGateButton != null)
                taiheGateButton.interactable = true; // 太和门总是可点击
        }

        // 更新太和殿Image（需要先完成太和门）
        if (taiheHallImage != null)
        {
            bool isCompleted = GameProgressManager.Instance.IsPuzzleCompleted("TaihePuzzleScene");
            bool isTaiheGateCompleted = GameProgressManager.Instance.IsPuzzleCompleted("TaiheMenPuzzleScene");

            taiheHallImage.color = isCompleted ? completedColor : (isTaiheGateCompleted ? unlockedColor : lockedColor);

            if (taiheHallButton != null)
                taiheHallButton.interactable = isTaiheGateCompleted || isCompleted;
        }

        // 更新中和殿Image（需要先完成太和殿）
        if (zhongheHallImage != null)
        {
            bool isCompleted = GameProgressManager.Instance.IsPuzzleCompleted("ZhonghePuzzleScene");
            bool isTaiheHallCompleted = GameProgressManager.Instance.IsPuzzleCompleted("TaihePuzzleScene");

            zhongheHallImage.color = isCompleted ? completedColor : (isTaiheHallCompleted ? unlockedColor : lockedColor);

            if (zhongheHallButton != null)
                zhongheHallButton.interactable = isTaiheHallCompleted || isCompleted;
        }

        // 更新保和殿Image（需要先完成中和殿）
        if (baoheHallImage != null)
        {
            bool isCompleted = GameProgressManager.Instance.IsPuzzleCompleted("BaohePuzzleScene");
            bool isZhongheCompleted = GameProgressManager.Instance.IsPuzzleCompleted("ZhonghePuzzleScene");

            baoheHallImage.color = isCompleted ? completedColor : (isZhongheCompleted ? unlockedColor : lockedColor);

            if (baoheHallButton != null)
                baoheHallButton.interactable = isZhongheCompleted || isCompleted;
        }
    }

    // 重置所有进度（可选，用于测试）
    public void ResetAllProgress()
    {
        if (GameProgressManager.Instance != null)
        {
            GameProgressManager.Instance.ResetAllProgress();
            LoadProgressAndUpdateImages();
            Debug.Log("所有进度已重置");
        }
    }
}