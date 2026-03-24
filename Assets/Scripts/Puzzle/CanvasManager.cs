using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CanvasManager : MonoBehaviour
{
    [Header("拼图基础配置")]
    public GameObject puzzlePiecePrefab;
    public Transform rightAreaTransform; // 右侧初始生成区域
    public float scaleOnAttach = 1.5f;   // 吸附后放大倍数
    public Sprite[] puzzleSprites;       // 长度为3的数组，依次对应Puzzle_0、Puzzle_1、Puzzle_2

    [Header("完成弹窗配置")]
    public GameObject completePanel;     // 拖拽绑定你创建的弹窗Panel
    public Text completeText;            // 弹窗里的文字（手动在编辑器填）

    [Header("音效配置")]
    public AudioClip successSound;       // 全部完成音效
    public AudioClip clickPieceSound;    // 点击拼图音效（新增）
    public AudioClip attachPieceSound;   // 单块拼图吸附音效（新增）
    private AudioSource audioSource;     // 音频播放组件

    // 三个拼图的目标坐标区间（按你提供的数值）
    private Dictionary<string, (Vector2 min, Vector2 max, Vector2 center)> targetAreas = new Dictionary<string, (Vector2, Vector2, Vector2)>()
    {
        { "Puzzle_0", (new Vector2(-573, 308), new Vector2(-473, 408), new Vector2(-523, 358)) },
        { "Puzzle_1", (new Vector2(-573, 58), new Vector2(-473, 158), new Vector2(-523, 108)) },
        { "Puzzle_2", (new Vector2(-573, -192), new Vector2(-473, -92), new Vector2(-523, -142)) }
    };

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

        // 生成三个拼图到右侧（带不同素材）
        SpawnPuzzlePieces();
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

    // 核心：坐标区间判定 + 吸附/弹回
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
            pieceRect.anchoredPosition = target.center;
            pieceRect.localScale = new Vector3(scaleOnAttach, scaleOnAttach, 1);
            Destroy(piece.GetComponent<DragDrop>());

            // 新增：播放单块吸附音效
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

    // 新增：播放单块拼图吸附音效
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

    // 新增：播放点击拼图音效（提供给DragDrop调用）
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

    // 关闭完成弹窗
    public void HideCompletePanel()
    {
        if (completePanel != null) completePanel.SetActive(false);
    }
}