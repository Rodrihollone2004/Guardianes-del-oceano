using UnityEngine;

public class SpongeManager : MonoBehaviour
{
    public static SpongeManager Instance;

    [Header("Spawn Settings")]
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private GameObject spongePrefab;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void SpawnSponge()
    {
        Instantiate(spongePrefab, spawnPoint.position, Quaternion.identity, transform);
    }
}
