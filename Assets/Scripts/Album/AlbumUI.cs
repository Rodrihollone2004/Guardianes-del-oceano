using UnityEngine;
using TMPro;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.UI;

public class AlbumUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject slotPrefab;      
    [SerializeField] private Transform container;

    [Header("Detail Pop-up References")]
    [SerializeField] private GameObject detailPanelOverlay; 
    [SerializeField] private Transform detailWindow;        
    [SerializeField] private Image detailImage;
    [SerializeField] private TextMeshProUGUI detailNameText;
    [SerializeField] private TextMeshProUGUI detailDescText;
    [SerializeField] private Color lockedColor = Color.black;

    private List<AlbumSlot> spawnedSlots = new List<AlbumSlot>();

    private void OnEnable()
    {
        RefreshAlbum();

        // Nos aseguramos de que el panel de detalles empiece apagado
        if (detailPanelOverlay != null)
            detailPanelOverlay.SetActive(false);
    }

    public void RefreshAlbum()
    {
        List<AnimalsSO> allFish = GameManager.Instance.GetAllFish();

        if (spawnedSlots.Count == 0)
        {
            foreach (AnimalsSO fish in allFish)
            {
                GameObject newSlotObj = Instantiate(slotPrefab, container);
                if (newSlotObj.TryGetComponent<AlbumSlot>(out AlbumSlot slot))
                {
                    slot.Setup(fish, this);
                    spawnedSlots.Add(slot);
                }
            }
        }
        else
        {
            for (int i = 0; i < allFish.Count; i++)
            {
                if (i < spawnedSlots.Count)
                {
                    spawnedSlots[i].Setup(allFish[i], this);
                }
            }
        }
    }

    public void ShowDetailPanel(AnimalsSO fish)
    {
        detailPanelOverlay.SetActive(true);

        if (fish.isUnlocked)
        {
            detailImage.sprite = fish.stickerSprite;
            detailImage.color = Color.white;
            detailNameText.text = fish.fishName;
            detailDescText.text = fish.description;
        }
        else
        {
            detailImage.sprite = fish.stickerSprite;
            detailImage.color = lockedColor;
            detailNameText.text = "???";
            detailDescText.text = "Sigue limpiando el océano para descubrir esta especie.";
        }

        detailWindow.localScale = Vector3.zero; 
        detailWindow.DOScale(1f, 0.4f).SetEase(Ease.OutBack).SetUpdate(true);
    }

    public void CloseDetailPanel()
    {
        detailWindow.DOScale(0f, 0.2f).SetEase(Ease.InBack).SetUpdate(true).OnComplete(() =>
        {
            detailPanelOverlay.SetActive(false);
        });
    }
}
