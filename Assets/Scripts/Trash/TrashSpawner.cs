using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashSpawner : MonoBehaviour
{
    [Header("Spawn Configuration")]
    [SerializeField] private GameObject[] trashPrefabs;
    [SerializeField] private Transform trashContainer;
    [SerializeField] private float spawnInterval = 1.5f;
    [SerializeField] private float spawnHeightOffset = 2f;
    [SerializeField] private float horizontalMargin = 1f;
    [SerializeField] private Camera mainCamera;

    [Header("Finish Spawn Configuration")]
    [SerializeField] private int spawnLimit;

    [Header("Items Contamination")]
    [SerializeField] private List<SpriteRenderer> cleanBackGround;
    [SerializeField] private List<SpriteRenderer> contaminationBackground;

    private int currentSpawn;

    private float screenLeft;
    private float screenRight;
    private float spawnY;

    public int SpawnLimit { get => spawnLimit; set => spawnLimit = value; }
    public List<SpriteRenderer> CleanBackground { get => cleanBackGround; set => cleanBackGround = value; }
    public List<SpriteRenderer> ContaminationBackground { get => contaminationBackground; set => contaminationBackground = value; }

    private void Awake()
    {
        CalculateSpawnBoundaries();
    }

    private void Start()
    {
        ResetItemsAlpha();
        GameManager.Instance.TrashSpawner = this;
        StartCoroutine(SpawnRoutine());
    }

    private void CalculateSpawnBoundaries()
    {
        float zDistance = Mathf.Abs(mainCamera.transform.position.z);

        Vector3 bottomLeft = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, zDistance));
        Vector3 topRight = mainCamera.ViewportToWorldPoint(new Vector3(1, 1, zDistance));

        screenLeft = bottomLeft.x + horizontalMargin;
        screenRight = topRight.x - horizontalMargin;

        spawnY = topRight.y + spawnHeightOffset;
    }

    private IEnumerator SpawnRoutine()
    {
        while (currentSpawn < spawnLimit)
        {
            yield return new WaitForSeconds(spawnInterval);
            SpawnTrash();
        }
    }

    private void SpawnTrash()
    {
        if (trashPrefabs.Length == 0) return;

        float randomX = Random.Range(screenLeft, screenRight);
        Vector3 spawnPos = new Vector3(randomX, spawnY, 0f);

        GameObject selectedPrefab = trashPrefabs[Random.Range(0, trashPrefabs.Length)];

        Instantiate(selectedPrefab, spawnPos, Quaternion.identity, trashContainer);

        currentSpawn++;
    }

    public void UpdateBackgroundOpacity(float currentContamination, float maxContamination)
    {
        float dirtyAlpha = Mathf.Clamp01(currentContamination / maxContamination);

        float cleanAlpha = 1f - dirtyAlpha;

        if (cleanBackGround.Count > 0)
        {
            foreach (SpriteRenderer item in cleanBackGround)
            {
                Color color = item.color;
                color.a = cleanAlpha;
                item.color = color;
            }
        }

        if (contaminationBackground.Count > 0)
        {
            foreach (SpriteRenderer item in contaminationBackground)
            {
                Color color = item.color;
                color.a = dirtyAlpha;
                item.color = color;
            }
        }
    }

    //public void ReturnContamination()
    //{
    //    if (contaminationItems.Count > 0)
    //        foreach (SpriteRenderer item in contaminationItems)
    //        {
    //            Color color = item.color;
    //            if (color.a > 1f)
    //                continue;
    //            color.a += 0.02f;
    //            item.color = color;
    //        }

    //    if (backContamination.Count > 0)
    //        foreach (SpriteRenderer item in backContamination)
    //        {
    //            Color color = item.color;
    //            if (color.a < 0f)
    //                continue;
    //            color.a -= 0.02f;
    //            item.color = color;
    //        }
    //}

    private void ResetItemsAlpha()
    {
        if (cleanBackGround.Count > 0)
            foreach (SpriteRenderer item in cleanBackGround)
            {
                Color color = item.color;
                color.a = 1f;
                item.color = color;
            }

        if (contaminationBackground.Count > 0)
            foreach (SpriteRenderer item in contaminationBackground)
            {
                Color color = item.color;
                color.a = 0f;
                item.color = color;
            }
    }

    private void OnDrawGizmosSelected()
    {
        if (mainCamera == null) mainCamera = Camera.main;
        CalculateSpawnBoundaries();

        Gizmos.color = Color.green;
        Gizmos.DrawLine(new Vector3(screenLeft, spawnY, 0), new Vector3(screenRight, spawnY, 0));
    }
}