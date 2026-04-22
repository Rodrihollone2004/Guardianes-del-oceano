using System.Collections;
using UnityEngine;

public class FishSpawner : MonoBehaviour
{
    [Header("Spawn Areas Configurations")]
    [SerializeField] private float verticalMargin = 2f;
    [SerializeField] private float verticalOffset = 0f;
    [SerializeField] private float horizontalOffset = 1.5f;

    [Header("Spawn Fish Configuration")]
    [SerializeField] private FishSO[] fishTypes;
    [SerializeField] private float spawnInterval = 3f;
    [SerializeField] private Transform fishContainer;
    [SerializeField] private Camera mainCamera;

    [Header("Finish Spawn Configuration")]
    [SerializeField] private int spawnLimit;

    private int currentSpawn;
    private float screenLeft;
    private float screenRight;
    private float screenTop;
    private float screenBottom;

    private void Awake()
    {
        CalculateBoundaries();
    }

    private void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    private void CalculateBoundaries()
    {
        float zDistance = Mathf.Abs(mainCamera.transform.position.z);
        Vector3 centerWorld = mainCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, zDistance));
        Vector3 bottomLeft = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, zDistance));
        Vector3 topRight = mainCamera.ViewportToWorldPoint(new Vector3(1, 1, zDistance));

        screenLeft = bottomLeft.x - horizontalOffset;
        screenRight = topRight.x + horizontalOffset;

        float centerY = centerWorld.y + verticalOffset;

        screenTop = centerY + verticalMargin;
        screenBottom = centerY - verticalMargin;
    }

    private IEnumerator SpawnRoutine()
    {
        while (currentSpawn < spawnLimit)
        {
            yield return new WaitForSeconds(spawnInterval);
            SpawnFish();
        }
    }

    //private void SpawnFish()
    //{
    //    if (fishTypes.Length == 0) return;

    //    bool spawnFromLeft = Random.Range(0, 2) == 0;

    //    float spawnX = spawnFromLeft ? screenLeft : screenRight;
    //    float spawnY = Random.Range(screenBottom, screenTop);
    //    Vector3 spawnPos = new Vector3(spawnX, spawnY, 0f);

    //    Vector2 direction = spawnFromLeft ? Vector2.right : Vector2.left;

    //    FishSO selectedSO = fishTypes[Random.Range(0, fishTypes.Length)];
    //    GameObject fishObj = Instantiate(selectedSO.prefab, spawnPos, Quaternion.identity, fishContainer);
    //    currentSpawn++;

    //    if (fishObj.TryGetComponent<Fish>(out Fish fishScript))
    //    {
    //        float randomSpeed = Random.Range(selectedSO.minVelocity, selectedSO.maxVelocity);
    //        fishScript.Initialize(direction, randomSpeed);
    //    }
    //}
    private void SpawnFish()
    {
        if (fishTypes.Length == 0) return;

        // Determinamos el lado de aparición
        bool spawnFromLeft = Random.Range(0, 2) == 0;

        float spawnX = spawnFromLeft ? screenLeft : screenRight;
        float spawnY = Random.Range(screenBottom, screenTop);
        Vector3 spawnPos = new Vector3(spawnX, spawnY, 0f);

        // Definimos la dirección del movimiento
        Vector2 direction = spawnFromLeft ? Vector2.right : Vector2.left;

        FishSO selectedSO = fishTypes[Random.Range(0, fishTypes.Length)];

        // Instanciamos el pez
        GameObject fishObj = Instantiate(selectedSO.prefab, spawnPos, Quaternion.identity, fishContainer);
        currentSpawn++;

        // Lógica Visual: Voltear el sprite si viene de la derecha
        // Asumimos que tu pez en el prefab mira hacia la DERECHA por defecto
        if (fishObj.TryGetComponent<SpriteRenderer>(out SpriteRenderer sr))
        {
            // Si no sale de la izquierda (osea, sale de la derecha), lo volteamos
            sr.flipX = !spawnFromLeft;
        }

        // Lógica de Comportamiento: Inicializar velocidad y dirección
        if (fishObj.TryGetComponent<Fish>(out Fish fishScript))
        {
            float randomSpeed = Random.Range(selectedSO.minVelocity, selectedSO.maxVelocity);
            fishScript.Initialize(direction, randomSpeed);
        }
    }
    private void OnDrawGizmos()
    {
        // Calculamos los bordes para verlos en el editor aunque no estemos en Play
        CalculateBoundaries();

        Gizmos.color = Color.cyan;

        // Línea izquierda (Punto de salida A)
        Gizmos.DrawLine(new Vector3(screenLeft, screenBottom, 0), new Vector3(screenLeft, screenTop, 0));

        // Línea derecha (Punto de salida B)
        Gizmos.DrawLine(new Vector3(screenRight, screenBottom, 0), new Vector3(screenRight, screenTop, 0));

        // Dibujar un pequeño rombo o caja en los extremos para marcar los límites
        Gizmos.DrawWireCube(new Vector3(screenLeft, (screenTop + screenBottom) / 2, 0), new Vector3(0.2f, screenTop - screenBottom, 0));
        Gizmos.DrawWireCube(new Vector3(screenRight, (screenTop + screenBottom) / 2, 0), new Vector3(0.2f, screenTop - screenBottom, 0));

        // Texto opcional (solo informativo en consola o si usas Handles)
    }
}

