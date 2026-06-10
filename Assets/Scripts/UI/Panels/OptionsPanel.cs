using DG.Tweening;
using UnityEngine;

public class OptionsPanel : UIPanel
{
    [SerializeField] private RectTransform panelOpciones;
    [SerializeField] private float duracion = 0.5f;

    public override Tween Show()
    {
        gameObject.SetActive(true);
        panelOpciones.anchoredPosition = new Vector2(-1000f, 0);

        Sequence seq = DOTween.Sequence().SetUpdate(true);
        seq.Append(canvasGroup.DOFade(1f, duracion));
        seq.Join(panelOpciones.DOAnchorPosX(0, duracion).SetEase(Ease.OutBack));

        return seq;
    }

    public override Tween Hide()
    {
        GameManager.Instance.SetTransitioning(true);
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;

        Sequence seq = DOTween.Sequence().SetUpdate(true);
        seq.Append(panelOpciones.DOAnchorPosX(-1000f, duracion).SetEase(Ease.InQuad));
        seq.Join(canvasGroup.DOFade(0f, duracion));

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
