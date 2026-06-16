using UnityEngine;

public class OilSlick : MonoBehaviour, ILevelEvent
{
    [Header("Settings")]
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private float contaminationPerSecond = 1f;

    private int health;

    private float tickTimer;
    private SpriteRenderer spriteRenderer;
    private System.Action onDestroyCallback;
    private TrashSpawner trashSpawner;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        trashSpawner = GameManager.Instance.TrashSpawner;

        SetPositionAndRandomized();
    }

    public void Execute(System.Action onComplete)
    {
        health = maxHealth;
        tickTimer = 0f;
        onDestroyCallback = onComplete;

        SetAlpha(1.0f);
    }

    void Update()
    {
        tickTimer += Time.deltaTime;
        if (tickTimer >= 1f)
        {
            GameManager.Instance.AddRawContamination(contaminationPerSecond);
            tickTimer = 0f;
        }
    }

    public void CleanOil(Sponge sponge)
    {
        health--;

        sponge.ContaminationCount++;

        float newAlpha = (float)health / maxHealth;
        SetAlpha(newAlpha);

        if (health <= 0)
        {
            onDestroyCallback?.Invoke();
            Destroy(gameObject);
        }
    }

    private void SetAlpha(float alpha)
    {
        if (spriteRenderer != null)
        {
            Color color = spriteRenderer.color;
            color.a = alpha;
            spriteRenderer.color = color;
        }
    }

    private void SetPositionAndRandomized()
    {
        Camera cam = Camera.main;
        float z = Mathf.Abs(cam.transform.position.z);

        Vector3 randomViewport;
        bool isInsideForbiddenZone = true;

        do
        {
            float x = Random.Range(0.15f, 0.85f);
            float y = Random.Range(0.15f, 0.85f);
            randomViewport = new Vector3(x, y, z);

            bool isMiddleX = x > 0.35f && x < 0.65f;
            bool isMiddleY = y > 0.35f && y < 0.65f;

            if (!(isMiddleX && isMiddleY))
            {
                isInsideForbiddenZone = false;
            }
        } while (isInsideForbiddenZone);

        transform.position = cam.ViewportToWorldPoint(randomViewport);

        transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360f));
    }

    public void ContaminationSewagePipe()
    {
        if (trashSpawner.ContaminationItems.Count > 0)
            for (int i = 0; i < trashSpawner.ContaminationItems.Count; i++)
            {
                Color contaminationColor = trashSpawner.ContaminationItems[i].color;
                contaminationColor.a -= 0.02f;
                trashSpawner.ContaminationItems[i].color = contaminationColor;
            }
    }

    public void GeneralContamination()
    {
        if (trashSpawner.BackContamination.Count > 0)
            for (int i = 0; i < trashSpawner.BackContamination.Count; i++)
            {
                Color contaminationColor = trashSpawner.BackContamination[i].color;
                contaminationColor.a += 0.02f;
                trashSpawner.BackContamination[i].color = contaminationColor;
            }
    }
}