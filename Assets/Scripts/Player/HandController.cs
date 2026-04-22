using UnityEngine;

public class HandController : MonoBehaviour
{
    [Header("Movement Configuration")]
    [SerializeField] private float smoothness = 0.1f;

    [Header("Catch Configuration")]
    [SerializeField] private LayerMask trashLayer;

    private Vector3 velocity;
    private Camera mainCamera;
    private Trash currentTrash;

    private bool justCaught;

    private void Start()
    {
        mainCamera = Camera.main;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
    }

    private void Update()
    {
        Vector3 mousePosition = InputManager.Instance.MoveInput;
        mousePosition.z = Mathf.Abs(mainCamera.transform.position.z);

        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mousePosition);
        Vector3 clampedPosition = ClampPositionToScreen(worldPosition);
        transform.position = Vector3.SmoothDamp(transform.position, clampedPosition, ref velocity, smoothness);

        CheckInputs();
    }

    private void CheckInputs()
    {
        if (InputManager.Instance.WasClickPressedThisFrame())
            if (currentTrash == null)
                TryCatch();

        if (!InputManager.Instance.IsDraging && currentTrash != null)
        {
            if (justCaught)
            {
                justCaught = false;
                return;
            }

            currentTrash.DropTrash();
            currentTrash = null;
        }
    }

    private void TryCatch()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, 0.5f, trashLayer);

        if (hit != null && hit.TryGetComponent<Trash>(out Trash trash))
        {
            if (trash.IsContamination)
                return;

            if (trash.HasFish())
            {
                trash.ReleaseFish();
            }
            else
            {
                currentTrash = trash;
                currentTrash.FollowHand(transform);
                justCaught = true;
            }
        }
    }

    private Vector3 ClampPositionToScreen(Vector3 targetPos)
    {
        float zDist = Mathf.Abs(mainCamera.transform.position.z);
        Vector3 minBounds = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, zDist));
        Vector3 maxBounds = mainCamera.ViewportToWorldPoint(new Vector3(1, 1, zDist));

        float margin = 0.2f;

        float clampedX = Mathf.Clamp(targetPos.x, minBounds.x + margin, maxBounds.x - margin);
        float clampedY = Mathf.Clamp(targetPos.y, minBounds.y + margin, maxBounds.y - margin);

        return new Vector3(clampedX, clampedY, targetPos.z);
    }
}
