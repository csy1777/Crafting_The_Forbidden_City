using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableTile : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("通用设置")]
    public GameObject ghostOutline;         // 场景中的 GhostOutline
    public RectTransform leftTemplateArea;  // 左侧模板区

    [Header("类型标志")]
    public bool isPlacedTile = false;       // true = 左侧已放置的瓦片，false = 右侧库瓦片

    [Header("放置预制体（仅右侧库瓦片使用）")]
    public GameObject placedTilePrefab;     // 左侧放置后的瓦片预制体（即 PlacedTile）

    private PolygonCollider2D polygonCollider;
    private LineRenderer lineRenderer;
    private CanvasGroup canvasGroup;
    private ScrollRect parentScrollRect;    // 右侧工具栏的 ScrollRect（仅右侧瓦片需要禁用）
    private Vector2[] originalPoints;       // 原始顶点（缓存）

    void Start()
    {
        // 获取必要组件
        polygonCollider = GetComponent<PolygonCollider2D>();
        canvasGroup = GetComponent<CanvasGroup>();

        if (polygonCollider != null)
            originalPoints = polygonCollider.points;
        else
            Debug.LogError($"物体 {name} 缺少 PolygonCollider2D 组件！");

        // 自动查找场景对象
        if (ghostOutline == null)
            ghostOutline = GameObject.Find("GhostOutline");
        if (leftTemplateArea == null)
            leftTemplateArea = GameObject.Find("LeftTemplateArea")?.GetComponent<RectTransform>();

        if (ghostOutline != null)
            lineRenderer = ghostOutline.GetComponent<LineRenderer>();

        // 如果是右侧库瓦片，获取父级 ScrollRect
        if (!isPlacedTile)
            parentScrollRect = GetComponentInParent<ScrollRect>();

        // 警告提示
        if (ghostOutline == null)
            Debug.LogWarning("未找到 GhostOutline 物体，请检查场景！");
        if (leftTemplateArea == null)
            Debug.LogWarning("未找到 LeftTemplateArea，请检查场景！");
    }

    // 开始拖拽
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (ghostOutline == null || lineRenderer == null || polygonCollider == null)
            return;

        // 1. 根据类型处理
        if (!isPlacedTile) // 右侧库瓦片
        {
            // 禁用右侧工具栏滚动
            if (parentScrollRect != null)
                parentScrollRect.enabled = false;

            // 自身半透明
            if (canvasGroup != null)
                canvasGroup.alpha = 0.5f;
        }
        else // 左侧已放置瓦片
        {
            // 自身隐藏（或半透明），用轮廓代替
            if (canvasGroup != null)
                canvasGroup.alpha = 0f; // 完全隐藏（也可以设为 0.5f）
        }

        // 2. 更新轮廓顶点（根据瓦片形状）
        UpdateOutlineVertices();

        // 3. 显示轮廓
        ghostOutline.SetActive(true);

        // 4. 立即将轮廓移动到鼠标位置
        OnDrag(eventData);
    }

    // 拖拽中
    public void OnDrag(PointerEventData eventData)
    {
        if (ghostOutline == null || leftTemplateArea == null) return;

        // 将鼠标屏幕坐标转换为左侧区域的局部坐标
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            leftTemplateArea,
            eventData.position,
            eventData.pressEventCamera,
            out Vector2 localPoint
        );

        // 获取左侧区域的尺寸边界（以中心为原点）
        Rect rect = leftTemplateArea.rect;
        float halfWidth = rect.width / 2;
        float halfHeight = rect.height / 2;

        // 限制坐标不超出左侧区域
        localPoint.x = Mathf.Clamp(localPoint.x, -halfWidth, halfWidth);
        localPoint.y = Mathf.Clamp(localPoint.y, -halfHeight, halfHeight);

        // 移动轮廓
        ghostOutline.GetComponent<RectTransform>().anchoredPosition = localPoint;
    }

    // 结束拖拽
    public void OnEndDrag(PointerEventData eventData)
    {
        if (ghostOutline == null) return;

        if (!isPlacedTile) // 右侧库瓦片
        {
            // 恢复工具栏
            if (parentScrollRect != null)
                parentScrollRect.enabled = true;

            // 自身恢复不透明
            if (canvasGroup != null)
                canvasGroup.alpha = 1f;

            // 在轮廓位置生成新的左侧瓦片
            if (placedTilePrefab != null)
            {
                // 获取轮廓当前的位置（局部坐标）
                Vector2 ghostPos = ghostOutline.GetComponent<RectTransform>().anchoredPosition;

                // 实例化新的瓦片，作为左侧模板区域的子物体
                GameObject newTile = Instantiate(placedTilePrefab, leftTemplateArea);
                RectTransform newTileRT = newTile.GetComponent<RectTransform>();
                newTileRT.anchoredPosition = ghostPos;

                // 确保新瓦片的缩放正确
                newTileRT.localScale = Vector3.one;
                ScaleForLeftArea(newTile);
                // 新瓦片应该设置为已放置类型（isPlacedTile 已在预制体中设置好，无需更改）
            }

            // 销毁当前右侧瓦片
            Destroy(gameObject);
        }
        else // 左侧已放置瓦片
        {
            // 将自身移动到轮廓的位置
            Vector2 ghostPos = ghostOutline.GetComponent<RectTransform>().anchoredPosition;
            GetComponent<RectTransform>().anchoredPosition = ghostPos;

            // 恢复自身显示
            if (canvasGroup != null)
                canvasGroup.alpha = 1f;
        }

        // 隐藏轮廓
        ghostOutline.SetActive(false);
    }
    // 将瓦片放大10倍（用于左侧区域）
    private void ScaleForLeftArea(GameObject tile)
    {
        if (tile != null)
        {
            tile.transform.localScale = Vector3.one * 10f;
        }
    }

    // 更新轮廓顶点（根据瓦片形状）
    private void UpdateOutlineVertices()
    {
        if (originalPoints == null || originalPoints.Length < 2) return;

        // 计算中心点
        Vector2 center = Vector2.zero;
        foreach (Vector2 p in originalPoints)
            center += p;
        center /= originalPoints.Length;

        // 构建顶点数组（相对于中心）
        Vector3[] points = new Vector3[originalPoints.Length + 1];
        for (int i = 0; i < originalPoints.Length; i++)
        {
            Vector2 offset = originalPoints[i] - center;
            points[i] = new Vector3(offset.x, offset.y, 0);
        }
        points[originalPoints.Length] = points[0]; // 闭合

        lineRenderer.positionCount = points.Length;
        lineRenderer.SetPositions(points);
    }
}