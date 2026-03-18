using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening; // 必须引入DoTween命名空间

public class ButtonHoverTween : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("缩放设置")]
    public float hoverScale = 1.2f;        // 悬停时的缩放倍数
    public float duration = 0.3f;           // 动画持续时间（秒）

    [Header("动画曲线")]
    public Ease enterEase = Ease.OutBack;    // 进入时的缓动类型
    public Ease exitEase = Ease.OutQuad;      // 退出时的缓动类型

    [Header("可选效果")]
    public bool usePunchEffect = false;       // 是否使用弹性效果
    public float punchAmount = 0.2f;           // 弹性强度

    private Vector3 originalScale;              // 原始大小
    private Tween currentTween;                  // 当前动画引用

    void Start()
    {
        // 保存原始大小
        originalScale = transform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // 停止当前正在播放的动画
        currentTween?.Kill();

        if (usePunchEffect)
        {
            // 使用弹性效果：先放大再回弹
            currentTween = transform.DOPunchScale(
                Vector3.one * punchAmount,
                duration,
                5,     // 弹性次数
                0.5f   // 弹性强度
            );
        }
        else
        {
            // 标准放大效果
            currentTween = transform.DOScale(
                originalScale * hoverScale,
                duration
            ).SetEase(enterEase); // 设置缓动曲线
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // 停止当前正在播放的动画
        currentTween?.Kill();

        // 恢复原始大小
        currentTween = transform.DOScale(originalScale, duration)
            .SetEase(exitEase);
    }

    // 当对象被销毁时，确保清理DoTween动画，防止内存泄漏
    void OnDestroy()
    {
        currentTween?.Kill();
    }
}