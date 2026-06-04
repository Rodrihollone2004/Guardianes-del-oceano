using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class AlbumUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject slotPrefab;      
    [SerializeField] private Transform container;        

    private List<AlbumSlot> spawnedSlots = new List<AlbumSlot>();

    private void OnEnable()
    {
        RefreshAlbum();
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
                    TextMeshProUGUI infoText = newSlotObj.GetComponentInChildren<TextMeshProUGUI>();
                    slot.Setup(fish, infoText);
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
                    TextMeshProUGUI infoText = spawnedSlots[i].GetComponentInChildren<TextMeshProUGUI>();
                    spawnedSlots[i].Setup(allFish[i], infoText);
                }
            }
        }
    }
}
