using DG.Tweening;
using UnityEngine;

public class PausePanel : UIPanel
{
    [SerializeField] private RectTransform containerBotones;
    [SerializeField] private float duracionAnimacion = 0.5f;

    public override Tween Show()
    {
        gameObject.SetActive(true);
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;

        Sequence seq = DOTween.Sequence().SetUpdate(true);
        seq.Append(canvasGroup.DOFade(1f, duracionAnimacion));
        seq.Join(containerBotones.DOAnchorPosY(0, duracionAnimacion).SetEase(Ease.OutBack));
        seq.OnComplete(() =>
        {
            canvasGroup.blocksRaycasts = true;
            canvasGroup.interactable = true;
            GameManager.Instance.SetTransitioning(false);
        });

        return seq;
    }

    public override Tween Hide()
    {
        GameManager.Instance.SetTransitioning(true);

        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;

        Sequence seq = DOTween.Sequence().SetUpdate(true);
        seq.Append(containerBotones.DOAnchorPosY(500f, duracionAnimacion).SetEase(Ease.InBack));
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