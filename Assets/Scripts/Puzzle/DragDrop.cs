using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Canvas canvas;
    private Vector2 startAnchoredPos; // 记录按下时的位置，用于判断是否真的在拖动

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        canvas = GetComponentInParent<Canvas>();
    }

    // 按下时：只记录初始位置，不改变视觉状态
    public void OnPointerDown(PointerEventData eventData)
    {
        startAnchoredPos = rectTransform.anchoredPosition;
    }

    // 开始拖拽：才改变视觉状态
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0.7f;
            canvasGroup.blocksRaycasts = false;
        }
        transform.SetParent(canvas.transform); // 提升层级
    }

    // 拖拽中：更新位置
    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    // 结束拖拽：强制恢复状态，无论是否真的拖动了
    public void OnEndDrag(PointerEventData eventData)
    {
        // 强制恢复视觉和交互状态
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 1f;
            canvasGroup.blocksRaycasts = true;
        }

        // 如果只是单击（位置几乎没变），不做位置判定
        float moveDistance = Vector2.Distance(rectTransform.anchoredPosition, startAnchoredPos);
        if (moveDistance < 5f) // 极小阈值，判断为单击
        {
            // 单击：位置不变，直接恢复，不调用CheckAndAttach
            return;
        }

        // 真正拖动了：才去判定位置
        CanvasManager manager = FindObjectOfType<CanvasManager>();
        if (manager != null)
            manager.CheckAndAttach(gameObject);
    }

    // 完全删除OnPointerClick，避免任何点击干扰
}