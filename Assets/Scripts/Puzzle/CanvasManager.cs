using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CanvasManager : MonoBehaviour
{
    [Header("拼图预设")]
    public GameObject puzzlePiecePrefab;

    [Header("区域引用")]
    public Transform rightAreaTransform;
    public List<Transform> puzzleSlots; // 拖入 Slot1、Slot2、Slot3

    void Start()
    {
        SpawnPuzzlePieces();
    }

        void SpawnPuzzlePieces()
        {
            for (int i = 0; i < 3; i++)
            {
                GameObject piece = Instantiate(puzzlePiecePrefab, rightAreaTransform);
                piece.name = "Puzzle_" + i;

                RectTransform rect = piece.GetComponent<RectTransform>();

                // ✔️ 强制固定在右侧面板中间，绝不乱跑！
                rect.anchoredPosition = new Vector2(0, 150 - 120 * i);
            }
        }


    public void CheckAndAttach(GameObject piece)
    {
        RectTransform pieceRect = piece.GetComponent<RectTransform>();
        float closestDistance = float.MaxValue;
        Transform targetSlot = null;

        foreach (var slot in puzzleSlots)
        {
            // 卡槽里有拼图 = 跳过，不能放
            if (slot.childCount > 0)
                continue;

            RectTransform slotRect = slot.GetComponent<RectTransform>();

            // 👇 进阶优化核心：判断拼图是否进入卡槽范围内！
            float xDifference = Mathf.Abs(pieceRect.anchoredPosition.x - slotRect.anchoredPosition.x);
            float yDifference = Mathf.Abs(pieceRect.anchoredPosition.y - slotRect.anchoredPosition.y);

            float slotHalfWidth = slotRect.rect.width / 2f + 20f; // +20 让范围更大一点
            float slotHalfHeight = slotRect.rect.height / 2f + 20f;

            // 拼图在卡槽范围内 = 直接判定放对！
            if (xDifference < slotHalfWidth && yDifference < slotHalfHeight)
            {
                targetSlot = slot;
                break; // 找到就停
            }
        }

        // ==============================
        // 放对了：直接吸附 + 放大 + 固定
        // ==============================
        if (targetSlot != null)
        {
            piece.transform.SetParent(targetSlot);
            pieceRect.anchoredPosition = Vector2.zero;
            pieceRect.localScale = new Vector3(1.5f, 1.5f, 1);
            Destroy(piece.GetComponent<DragDrop>());
        }
        // ==============================
        // 放错了：弹回右侧原来位置
        // ==============================
        else
        {
            int index = 0;
            string[] parts = piece.name.Split('_');
            if (parts.Length >= 2)
                int.TryParse(parts[1], out index);

            piece.transform.SetParent(rightAreaTransform);
            pieceRect.anchoredPosition = new Vector2(0, 120 - 110 * index);
        }
    }
    }