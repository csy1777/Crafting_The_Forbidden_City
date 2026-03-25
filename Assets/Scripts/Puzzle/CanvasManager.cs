using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement; // 场景切换命名空间

[System.Serializable] // 序列化类，使其可在编辑器显示
public class PuzzleTargetArea
{
    public string pieceName; // 拼图名称（如Puzzle_0）
    public Vector2 minPos;   // 最小坐标
    public Vector2 maxPos;   // 最大坐标
    public Vector2 centerPos;// 吸附中心坐标
    public Vector2 sizeDelta;// 吸附后的尺寸（自定义）
}

public class CanvasManager : MonoBehaviour
{
    [Header("拼图基础配置")]
    public GameObject puzzlePiecePrefab;
    public Transform rightAreaTransform; // 右侧初始生成区域
    public float scaleOnAttach = 1.5f;   // 吸附后放大倍数（可选保留/移除）
    public Sprite[] puzzleSprites;       // 长度为3的数组，依次对应Puzzle_0、Puzzle_1、Puzzle_2

    [Header("拼图目标区域配置【可在外部编辑】")]
    public List<PuzzleTargetArea> puzzleTargetAreas; // 可在编辑器配置的目标区域列表

    [Header("完成弹窗配置")]
    public GameObject completePanel;     // 拖拽绑定你创建的弹窗Panel
    public Text completeText;            // 弹窗里的文字（手动在编辑器填）

    [Header("音效配置")]
    public AudioClip successSound;       // 全部完成音效
    public AudioClip clickPieceSound;    // 点击拼图音效（新增）
    public AudioClip attachPieceSound;   // 单块拼图吸附音效（新增）
    private AudioSource audioSource;     // 音频播放组件

    // 运行时使用的目标区域字典（新增size字段）
    private Dictionary<string, (Vector2 min, Vector2 max, Vector2 center, Vector2 size)> targetAreas = new Dictionary<string, (Vector2, Vector2, Vector2, Vector2)>();

    private int completedCount = 0;      // 已完成拼图数
    private bool isAllCompleted = false; // 是否全部完成

    void Start()
    {
        // 初始化音频组件
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        // 配置音频参数（可选）
        audioSource.loop = false;         // 不循环播放
        audioSource.playOnAwake = false;  // 不自动播放

        // 初始化弹窗：默认隐藏 + 添加点击关闭事件
        if (completePanel != null)
        {
            completePanel.SetActive(false);
            Button closeBtn = completePanel.GetComponent<Button>();
            if (closeBtn == null) closeBtn = completePanel.AddComponent<Button>();
            closeBtn.onClick.AddListener(HideCompletePanel);
        }

        // 初始化目标区域字典（从编辑器配置的列表转换）
        InitTargetAreas();

        // 生成三个拼图到右侧（带不同素材）
        SpawnPuzzlePieces();
    }

    // 初始化目标区域字典（从编辑器配置的列表转换）
    private void InitTargetAreas()
    {
        targetAreas.Clear();

        if (puzzleTargetAreas == null || puzzleTargetAreas.Count == 0)
        {
            Debug.LogWarning("未配置拼图目标区域，使用默认值！");
            // 添加默认值（兼容旧版本，新增尺寸）
            puzzleTargetAreas = new List<PuzzleTargetArea>()
            {
                new PuzzleTargetArea(){
                    pieceName = "Puzzle_0",
                    minPos = new Vector2(-573, 308),
                    maxPos = new Vector2(-473, 408),
                    centerPos = new Vector2(-523, 358),
                    sizeDelta = new Vector2(300, 300)
                },
                new PuzzleTargetArea(){
                    pieceName = "Puzzle_1",
                    minPos = new Vector2(-573, 58),
                    maxPos = new Vector2(-473, 158),
                    centerPos = new Vector2(-523, 108),
                    sizeDelta = new Vector2(300, 300)
                },
                new PuzzleTargetArea(){
                    pieceName = "Puzzle_2",
                    minPos = new Vector2(-573, -192),
                    maxPos = new Vector2(-473, -92),
                    centerPos = new Vector2(-523, -142),
                    sizeDelta = new Vector2(300, 300)
                }
            };
        }

        // 将列表转换为字典，方便快速查找（新增size）
        foreach (var area in puzzleTargetAreas)
        {
            if (string.IsNullOrEmpty(area.pieceName))
            {
                Debug.LogError("存在未设置名称的拼图目标区域！");
                continue;
            }
            targetAreas[area.pieceName] = (area.minPos, area.maxPos, area.centerPos, area.sizeDelta);
        }
    }

    // 生成拼图到右侧初始位置（新增素材赋值逻辑）
    void SpawnPuzzlePieces()
    {
        if (puzzleSprites == null || puzzleSprites.Length < 3)
        {
            Debug.LogError("请在CanvasManager中配置3个拼图素材！");
            return;
        }

        for (int i = 0; i < 3; i++)
        {
            GameObject piece = Instantiate(puzzlePiecePrefab, rightAreaTransform);
            piece.name = $"Puzzle_{i}";

            RectTransform rect = piece.GetComponent<RectTransform>();
            rect.anchoredPosition = new Vector2(0, 300 - 300 * i);
            rect.sizeDelta = new Vector2(250, 250);

            Image pieceImage = piece.GetComponent<Image>();
            if (pieceImage != null)
            {
                pieceImage.sprite = puzzleSprites[i];
                pieceImage.preserveAspect = true;
            }
            else
            {
                Debug.LogWarning($"Puzzle_{i}预制体缺少Image组件，请添加！");
            }
        }
    }

    // 核心：坐标区间判定 + 吸附/弹回（应用自定义位置+尺寸）
    public void CheckAndAttach(GameObject piece)
    {
        if (isAllCompleted) return;

        RectTransform pieceRect = piece.GetComponent<RectTransform>();
        string pieceName = piece.name;

        if (!targetAreas.ContainsKey(pieceName))
        {
            ResetPieceToRightArea(piece);
            return;
        }

        var target = targetAreas[pieceName];
        Vector2 piecePos = pieceRect.anchoredPosition;

        bool isInArea = (piecePos.x >= target.min.x && piecePos.x <= target.max.x) &&
                        (piecePos.y >= target.min.y && piecePos.y <= target.max.y);

        if (isInArea)
        {
            piece.transform.SetParent(rightAreaTransform.parent);
            pieceRect.anchoredPosition = target.center; // 自定义位置
            pieceRect.sizeDelta = target.size;          // 自定义尺寸
            pieceRect.localScale = new Vector3(scaleOnAttach, scaleOnAttach, 1); // 可选：改为Vector3.one移除缩放
            Destroy(piece.GetComponent<DragDrop>());

            // 播放单块吸附音效
            PlayAttachPieceSound();

            completedCount++;
            if (completedCount >= 3)
            {
                isAllCompleted = true;
                ShowCompletePanel(); // 弹出完成界面
            }
        }
        else
        {
            ResetPieceToRightArea(piece);
        }
    }

    // 拼图弹回右侧初始位置
    private void ResetPieceToRightArea(GameObject piece)
    {
        int index = int.Parse(piece.name.Split('_')[1]);
        piece.transform.SetParent(rightAreaTransform);

        RectTransform pieceRect = piece.GetComponent<RectTransform>();
        pieceRect.anchoredPosition = new Vector2(0, 300 - 300 * index);
        pieceRect.sizeDelta = new Vector2(250, 250);
        pieceRect.localScale = Vector3.one;
    }

    // 显示完成弹窗（新增播放音效）
    private void ShowCompletePanel()
    {
        if (completePanel != null)
        {
            completePanel.SetActive(true);
            completePanel.transform.SetAsLastSibling(); // 弹窗置顶

            // 播放成功音效
            PlaySuccessSound();
        }
    }

    // 播放成功音效
    private void PlaySuccessSound()
    {
        if (successSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(successSound); // 单次播放音效（不打断其他音频）
        }
        else
        {
            Debug.LogWarning("成功音效未配置或音频组件初始化失败！");
        }
    }

    // 播放单块拼图吸附音效
    private void PlayAttachPieceSound()
    {
        if (attachPieceSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(attachPieceSound);
        }
        else
        {
            Debug.LogWarning("单块吸附音效未配置或音频组件初始化失败！");
        }
    }

    // 播放点击拼图音效（提供给DragDrop调用）
    public void PlayClickPieceSound()
    {
        if (clickPieceSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(clickPieceSound);
        }
        else
        {
            Debug.LogWarning("点击拼图音效未配置或音频组件初始化失败！");
        }
    }

    private bool isSceneLoading = false; // 防止重复加载

    // 关闭完成弹窗并跳转场景
    public void HideCompletePanel()
    {
        if (isSceneLoading) return;
        isSceneLoading = true;

        if (completePanel != null) completePanel.SetActive(false);

        // 跳转至SelectLevel场景（根据Build Settings中的场景名调整）
        SceneManager.LoadScene("Scenes/SelectLevel");
    }
}