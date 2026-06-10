using DG.Tweening;
using UnityEngine;

public class MainMenuPanel : UIPanel
{
    [SerializeField] private RectTransform containerBotones;
    [SerializeField] private float duracionAnimacion = 0.5f;

    public override Tween Show()
    {
        gameObject.SetActive(true);

        Sequence seq = DOTween.Sequence();
        seq.Append(canvasGroup.DOFade(1f, duracionAnimacion));
        seq.Join(containerBotones.DOAnchorPosX(0, duracionAnimacion).SetEase(Ease.OutBack));

        return seq;
    }

    public override Tween Hide()
    {
        GameManager.Instance.SetTransitioning(true);
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;

        Sequence seq = DOTween.Sequence();
        seq.Append(containerBotones.DOAnchorPosX(500f, duracionAnimacion).SetEase(Ease.InBack));
        seq.Join(canvasGroup.DOFade(0f, duracionAnimacion));

        seq.OnComplete(() =>
        {
            gameObject.SetActive(false);
            canvasGroup.blocksRaycasts = true;
            canvasGroup.interactable = true;
            GameManager.Instance.SetTransitioning(false);
        });

        return seq;
    }
}