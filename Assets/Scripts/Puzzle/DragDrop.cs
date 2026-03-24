using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Canvas canvas;
    private Vector2 startAnchoredPos;
    // 提前缓存CanvasManager，避免重复查找
    private CanvasManager canvasManager;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        canvas = GetComponentInParent<Canvas>();

        // 找到全局的CanvasManager
        canvasManager = FindObjectOfType<CanvasManager>();
        if (canvasManager == null)
            Debug.LogError("场景中没有找到CanvasManager！");
    }

    // 点击拼图时立刻播放音效
    public void OnPointerDown(PointerEventData eventData)
    {
        startAnchoredPos = rectTransform.anchoredPosition;
        // 只要点下去就响！
        if (canvasManager != null)
            canvasManager.PlayClickPieceSound();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0.7f;
            canvasGroup.blocksRaycasts = false;
        }
        // 把拼图提升到最上层，避免被遮挡
        transform.SetParent(canvas.transform);
    }

    public void OnDrag(PointerEventData eventData)
    {
        // 处理拖拽位置（适配Canvas缩放）
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // 恢复透明度和射线阻挡
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 1f;
            canvasGroup.blocksRaycasts = true;
        }
        // 拖拽结束后，交给CanvasManager判断是否吸附
        if (canvasManager != null)
            canvasManager.CheckAndAttach(gameObject);
    }
}