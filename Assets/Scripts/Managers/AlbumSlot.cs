using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class AlbumSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Color lockedColor = Color.black;

    private Image stickerImage;
    private AnimalsSO fishData;
    private TextMeshProUGUI infoText; // Referencia al texto de la derecha

    private void Awake()
    {
        stickerImage = GetComponent<Image>();
    }

    public void Setup(AnimalsSO data, TextMeshProUGUI displayTextField)
    {
        fishData = data;
        infoText = displayTextField;

        if (fishData.isUnlocked)
        {
            stickerImage.sprite = fishData.stickerSprite;
            stickerImage.color = Color.white;
        }
        else
        {
            stickerImage.sprite = fishData.stickerSprite;
            stickerImage.color = lockedColor; 
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (fishData.isUnlocked && infoText != null)
        {
            infoText.text = $"<b>{fishData.fishName}</b>\n\n{fishData.description}";
        }
        else if (infoText != null)
        {
            infoText.text = "???\n\nSigue limpiando el océano para descubrir esta especie.";
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (infoText != null) infoText.text = "";
    }
}
