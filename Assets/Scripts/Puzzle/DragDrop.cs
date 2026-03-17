using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Canvas canvas;
    private Transform originalParent;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
        canvas = GetComponentInParent<Canvas>();
        originalParent = transform.parent;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0.7f;
            canvasGroup.blocksRaycasts = false;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.clickCount > 0) // 检测是双击
        {
            // 强制恢复状态，解除锁定
            if (canvasGroup != null)
            {
                canvasGroup.alpha = 1f;
                canvasGroup.blocksRaycasts = true;
            }
            // 把拼图放回原来的父物体（右侧区域）
            transform.SetParent(originalParent);
            rectTransform.anchoredPosition = Vector2.zero;
        }
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        transform.SetParent(canvas.transform); // 拖到最上层
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (rectTransform != null && canvas != null)
        {
            rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 1f;
            canvasGroup.blocksRaycasts = true;
        }

        CanvasManager manager = FindObjectOfType<CanvasManager>();
        if (manager != null)
        {
            manager.CheckAndAttach(gameObject);
        }
    }

}
