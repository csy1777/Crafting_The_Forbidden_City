using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CanvasManager : MonoBehaviour
{
    [Header("拼图基础配置")]
    public GameObject puzzlePiecePrefab;
    public Transform rightAreaTransform; // 右侧初始生成区域
    public float scaleOnAttach = 1.5f;   // 吸附后放大倍数

    [Header("完成弹窗配置")]
    public GameObject completePanel;     // 拖拽绑定你创建的弹窗Panel
    public Text completeText;            // 弹窗里的文字（手动在编辑器填）

    // 👇 删掉了 completeContent 变量行

    // 三个拼图的目标坐标区间（按你提供的数值）
    private Dictionary<string, (Vector2 min, Vector2 max, Vector2 center)> targetAreas = new Dictionary<string, (Vector2, Vector2, Vector2)>()
    {
        // Puzzle_0: Pos X=-523, Pos Y=358
        { "Puzzle_0", (new Vector2(-573, 308), new Vector2(-473, 408), new Vector2(-523, 358)) },
        // Puzzle_1: Pos X=-523, Pos Y=108
        { "Puzzle_1", (new Vector2(-573, 58), new Vector2(-473, 158), new Vector2(-523, 108)) },
        // Puzzle_2: Pos X=-523, Pos Y=-142
        { "Puzzle_2", (new Vector2(-573, -192), new Vector2(-473, -92), new Vector2(-523, -142)) }
    };

    private int completedCount = 0;      // 已完成拼图数
    private bool isAllCompleted = false; // 是否全部完成

    void Start()
    {
        // 初始化弹窗：默认隐藏 + 添加点击关闭事件
        if (completePanel != null)
        {
            completePanel.SetActive(false);
            // 👇 删掉了 completeText.text = completeContent; 这行

            // 给弹窗添加点击关闭按钮（自动创建，无需手动加）
            Button closeBtn = completePanel.GetComponent<Button>();
            if (closeBtn == null) closeBtn = completePanel.AddComponent<Button>();
            closeBtn.onClick.AddListener(HideCompletePanel);
        }

        // 生成三个拼图到右侧
        SpawnPuzzlePieces();
    }

    // 生成拼图到右侧初始位置
    void SpawnPuzzlePieces()
    {
        for (int i = 0; i < 3; i++)
        {
            GameObject piece = Instantiate(puzzlePiecePrefab, rightAreaTransform);
            piece.name = $"Puzzle_{i}";

            RectTransform rect = piece.GetComponent<RectTransform>();
            // 调整后的初始坐标：垂直间距拉大，避免重叠
            rect.anchoredPosition = new Vector2(0, 300 - 300 * i);
            // 保持250×250大小
            rect.sizeDelta = new Vector2(250, 250);
        }
    }

    // 核心：坐标区间判定 + 吸附/弹回
    public void CheckAndAttach(GameObject piece)
    {
        if (isAllCompleted) return; // 全部完成后不再处理

        RectTransform pieceRect = piece.GetComponent<RectTransform>();
        string pieceName = piece.name;

        // 找不到对应拼图 → 弹回初始位置
        if (!targetAreas.ContainsKey(pieceName))
        {
            ResetPieceToRightArea(piece);
            return;
        }

        var target = targetAreas[pieceName];
        Vector2 piecePos = pieceRect.anchoredPosition;

        // 判断是否在目标坐标区间内
        bool isInArea = (piecePos.x >= target.min.x && piecePos.x <= target.max.x) &&
                        (piecePos.y >= target.min.y && piecePos.y <= target.max.y);

        if (isInArea)
        {
            // 吸附到目标中心 + 放大 + 禁用拖拽
            piece.transform.SetParent(rightAreaTransform.parent);
            pieceRect.anchoredPosition = target.center;
            pieceRect.localScale = new Vector3(scaleOnAttach, scaleOnAttach, 1);
            Destroy(piece.GetComponent<DragDrop>());

            // 计数+1，检测是否全部完成
            completedCount++;
            if (completedCount >= 3)
            {
                isAllCompleted = true;
                ShowCompletePanel(); // 弹出完成界面
            }
        }
        else
        {
            // 不在区间内 → 弹回右侧初始位置
            ResetPieceToRightArea(piece);
        }
    }

    // 拼图弹回右侧初始位置
    private void ResetPieceToRightArea(GameObject piece)
    {
        int index = int.Parse(piece.name.Split('_')[1]);
        piece.transform.SetParent(rightAreaTransform);

        RectTransform pieceRect = piece.GetComponent<RectTransform>();
        // 关键：弹回坐标和初始生成坐标完全一致，避免聚集
        pieceRect.anchoredPosition = new Vector2(0, 300 - 300 * index);
        pieceRect.sizeDelta = new Vector2(250, 250); // 确保弹回后大小不变
        pieceRect.localScale = Vector3.one;
    }

    // 显示完成弹窗
    private void ShowCompletePanel()
    {
        if (completePanel != null)
        {
            completePanel.SetActive(true);
            completePanel.transform.SetAsLastSibling(); // 弹窗置顶
        }
    }

    // 关闭完成弹窗
    public void HideCompletePanel()
    {
        if (completePanel != null) completePanel.SetActive(false);
    }
}