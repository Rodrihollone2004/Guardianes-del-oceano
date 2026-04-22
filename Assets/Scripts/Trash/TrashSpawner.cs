using System.Collections;
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

    private int currentSpawn;

    private float screenLeft;
    private float screenRight;
    private float spawnY;

    public int SpawnLimit { get => spawnLimit; set => spawnLimit = value; }

    private void Awake()
    {
        CalculateSpawnBoundaries();
    }

    private void Start()
    {
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

    private void OnDrawGizmosSelected()
    {
        if (mainCamera == null) mainCamera = Camera.main;
        CalculateSpawnBoundaries();

        Gizmos.color = Color.green;
        Gizmos.DrawLine(new Vector3(screenLeft, spawnY, 0), new Vector3(screenRight, spawnY, 0));
    }
}