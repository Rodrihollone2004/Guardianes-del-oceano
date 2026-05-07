using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class AlbumUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject slotPrefab;      
    [SerializeField] private Transform container;        
    [SerializeField] private TextMeshProUGUI infoText;   

    private List<AlbumSlot> spawnedSlots = new List<AlbumSlot>();

    private void OnEnable()
    {
        RefreshAlbum();
    }

    public void RefreshAlbum()
    {
        List<FishSO> allFish = GameManager.Instance.GetAllFish();

        if (spawnedSlots.Count == 0)
        {
            foreach (FishSO fish in allFish)
            {
                GameObject newSlotObj = Instantiate(slotPrefab, container);
                if (newSlotObj.TryGetComponent<AlbumSlot>(out AlbumSlot slot))
                {
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
                    spawnedSlots[i].Setup(allFish[i], infoText);
            }
        }
    }
}
