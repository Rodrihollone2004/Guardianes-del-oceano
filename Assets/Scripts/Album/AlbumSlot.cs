using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using DG.Tweening;

public class AlbumSlot : MonoBehaviour, /*IPointerEnterHandler, IPointerExitHandler,*/ IPointerClickHandler
{
    [Header("UI References")]
    [SerializeField] private Color lockedColor = Color.black;

    private Image stickerImage;
    private TextMeshProUGUI nameText;

    private AnimalsSO fishData;
    private AlbumUI parentUI;
    
    private void Awake()
    {
        stickerImage = GetComponent<Image>();
        nameText = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void Setup(AnimalsSO data, AlbumUI ui)
    {
        fishData = data;
        parentUI = ui;

        if (fishData.isUnlocked)
        {
            stickerImage.sprite = fishData.stickerSprite;
            stickerImage.color = Color.white;
            nameText.text = fishData.fishName;
        }
        else
        {
            stickerImage.sprite = fishData.stickerSprite;
            stickerImage.color = lockedColor;
            nameText.text = "???";
        }
    }

    //public void OnPointerEnter(PointerEventData eventData)
    //{
    //    transform.DOScale(1.1f, 0.2f).SetEase(Ease.OutBack).SetUpdate(true);
    //}

    //public void OnPointerExit(PointerEventData eventData)
    //{
    //    transform.DOScale(1f, 0.2f).SetEase(Ease.OutQuad).SetUpdate(true);
    //}

    public void OnPointerClick(PointerEventData eventData)
    {
        parentUI.ShowDetailPanel(fishData);
    }
}
