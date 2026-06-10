using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class AlbumNotification : MonoBehaviour
{
    [Header("Sprite Settings")]
    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Sprite goldSprite;

    private Image albumIcon;
    private Tweener pulseTween;
    private bool isAnimation;

    private void Awake()
    {
        albumIcon = GetComponent<Image>();
    }

    public void NotifyNewFish()
    {
        albumIcon.sprite = goldSprite;

        pulseTween?.Kill();
        pulseTween = transform.DOScale(1.15f, 0.5f).SetLoops(-1, LoopType.Yoyo).SetUpdate(true);

        isAnimation = true;
    }

    public void ClearNotification()
    {
        if (isAnimation)
        {
            pulseTween?.Kill();
            albumIcon.sprite = normalSprite;
            isAnimation = false;
        } 

        transform.DOScale(1f, 0.2f).SetUpdate(true);
    }
}
