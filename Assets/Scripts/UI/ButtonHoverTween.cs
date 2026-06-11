using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHoverTween : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Vector3 hoverScale;
    private float tweenDuration = 0.2f;

    private Vector3 originalScale;

    private void Awake()
    {
        originalScale = transform.localScale;
        hoverScale = new Vector3(1.2f, 1.2f, 1.2f);
        tweenDuration = 0.2f;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayButtonHover();

        transform.DOScale(hoverScale, tweenDuration).SetEase(Ease.OutBack).SetUpdate(true); ;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOScale(originalScale, tweenDuration).SetEase(Ease.OutQuad).SetUpdate(true); ;
    }
}
