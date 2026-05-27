using UnityEngine;

public class SewagePipe : MonoBehaviour, ILevelEvent
{
    [Header("Settings")]
    [SerializeField] private int health = 3;
    [SerializeField] private float contaminationPerSecond = 1f;

    private float tickTimer;
    private SpriteRenderer spriteRenderer;
    private System.Action onDestroyCallback;
    private TrashSpawner trashSpawner;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        trashSpawner = GameManager.Instance.TrashSpawner;
    }

    public void Execute(System.Action onComplete)
    {
        health = 3;
        tickTimer = 0f;
        onDestroyCallback = onComplete;

        SetPositionAndRotation();
    }

    void Update()
    {
        tickTimer += Time.deltaTime;
        if (tickTimer >= 1f)
        {
            GameManager.Instance.AddRawContamination(contaminationPerSecond);
            ContaminationSewagePipe();
            GeneralContamination();
            tickTimer = 0f;
        }
    }

    public void Hit()
    {
        health--;

        if (health <= 0)
        {
            onDestroyCallback?.Invoke();
            Destroy(gameObject);
        }
    }

    private void SetPositionAndRotation()
    {
        Camera cam = Camera.main;
        float z = Mathf.Abs(cam.transform.position.z);

        Vector3[] spots = {
            new Vector3(0.5f, 1f, z), 
            new Vector3(-0f, 0.5f, z), 
            new Vector3(1f, 0.5f, z)   
        };

        int randomIndex = Random.Range(0, spots.Length);
        transform.position = cam.ViewportToWorldPoint(spots[randomIndex]);

        transform.rotation = Quaternion.identity;
        transform.localScale = Vector3.one;

        switch (randomIndex)
        {
            case 0:
                spriteRenderer.flipX = false;
                transform.eulerAngles = new Vector3(0, 0, -90f);
                break;

            case 1:
                spriteRenderer.flipX = false;
                transform.eulerAngles = Vector3.zero;
                break;

            case 2:
                spriteRenderer.flipX = true;
                transform.eulerAngles = Vector3.zero;
                break;
        }
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