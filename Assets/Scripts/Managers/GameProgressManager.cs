using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 游戏进度管理器 - 跨场景保存游戏状态
/// </summary>
public class GameProgressManager : MonoBehaviour
{
    // 单例模式
    public static GameProgressManager Instance { get; private set; }

    // 记录哪些拼图已完成
    private HashSet<string> completedPuzzles = new HashSet<string>();

    // 记录Image颜色状态（场景名 -> Image颜色状态）
    private Dictionary<string, Color> imageColorStates = new Dictionary<string, Color>();

    // 游戏是否全部完成
    private bool isGameCompleted = false;

    void Awake()
    {
        // 单例初始化
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 跨场景保留
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 记录完成的拼图
    /// </summary>
    public void CompletePuzzle(string puzzleSceneName, string puzzleItemId)
    {
        if (!completedPuzzles.Contains(puzzleSceneName))
        {
            completedPuzzles.Add(puzzleSceneName);
            Debug.Log($"完成拼图: {puzzleSceneName}, 已解锁物品: {puzzleItemId}");

            // 可选：保存到PlayerPrefs
            PlayerPrefs.SetInt($"Puzzle_{puzzleSceneName}", 1);
            PlayerPrefs.Save();
        }
    }

    /// <summary>
    /// 检查拼图是否已完成
    /// </summary>
    public bool IsPuzzleCompleted(string puzzleSceneName)
    {
        // 先从内存检查
        if (completedPuzzles.Contains(puzzleSceneName))
            return true;

        // 再从PlayerPrefs检查（用于游戏重启后保持进度）
        if (PlayerPrefs.GetInt($"Puzzle_{puzzleSceneName}", 0) == 1)
        {
            completedPuzzles.Add(puzzleSceneName);
            return true;
        }

        return false;
    }

    /// <summary>
    /// 设置Image颜色状态
    /// </summary>
    public void SetImageColor(string sceneName, string imageName, Color color)
    {
        string key = $"{sceneName}_{imageName}";
        if (imageColorStates.ContainsKey(key))
        {
            imageColorStates[key] = color;
        }
        else
        {
            imageColorStates.Add(key, color);
        }

        // 保存到PlayerPrefs
        PlayerPrefs.SetFloat($"{key}_R", color.r);
        PlayerPrefs.SetFloat($"{key}_G", color.g);
        PlayerPrefs.SetFloat($"{key}_B", color.b);
        PlayerPrefs.SetFloat($"{key}_A", color.a);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// 获取Image颜色状态
    /// </summary>
    public Color GetImageColor(string sceneName, string imageName, Color defaultColor)
    {
        string key = $"{sceneName}_{imageName}";

        // 先从内存检查
        if (imageColorStates.ContainsKey(key))
        {
            return imageColorStates[key];
        }

        // 再从PlayerPrefs检查
        if (PlayerPrefs.HasKey($"{key}_R"))
        {
            Color color = new Color(
                PlayerPrefs.GetFloat($"{key}_R"),
                PlayerPrefs.GetFloat($"{key}_G"),
                PlayerPrefs.GetFloat($"{key}_B"),
                PlayerPrefs.GetFloat($"{key}_A")
            );
            imageColorStates[key] = color;
            return color;
        }

        return defaultColor;
    }

    /// <summary>
    /// 重置所有进度（游戏结束时调用）
    /// </summary>
    public void ResetAllProgress()
    {
        completedPuzzles.Clear();
        imageColorStates.Clear();

        // 清除所有PlayerPrefs
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        isGameCompleted = false;
        Debug.Log("游戏进度已重置");
    }

    /// <summary>
    /// 完成整个游戏
    /// </summary>
    public void CompleteGame()
    {
        isGameCompleted = true;
        Debug.Log("游戏通关！");
        // 这里可以触发游戏结束界面等
    }

    public bool IsGameCompleted()
    {
        return isGameCompleted;
    }
}