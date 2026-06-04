using UnityEngine;

public class HandController : MonoBehaviour
{
    [Header("Movement Configuration")]
    [SerializeField] private float smoothness = 0.1f;

    [Header("Catch Configuration")]
    [SerializeField] private LayerMask interactionLayer;

    private Vector3 velocity;
    private Camera mainCamera;
    private Trash currentTrash;
    private Sponge currentSponge;

    private bool justCaught;

    public Trash CurrentTrash { get => currentTrash; set => currentTrash = value; }
    public bool JustCaught { get => justCaught; set => justCaught = value; }
    public Sponge CurrentSponge { get => currentSponge; set => currentSponge = value; }

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

        HandleInteraction();
    }

    private void HandleInteraction()
    {
        if (InputManager.Instance.WasClickPressedThisFrame())
        {
            Collider2D hit = Physics2D.OverlapCircle(transform.position, 0.4f, interactionLayer);

            if (hit != null)
            {
                if (hit.TryGetComponent<IInteractable>(out var interactable))
                {
                    interactable.Interact(this);
                    return;
                }
            }
        }

        if (!InputManager.Instance.IsDraging && currentTrash != null ||
            !InputManager.Instance.IsDraging && currentSponge != null)
        {
            if (justCaught) { justCaught = false; return; }
            
            if (currentTrash != null)
            {
                currentTrash.DropTrash();
                currentTrash = null;
            }
            else if (currentSponge != null)
            {
                currentSponge.DropSponge();
                currentSponge = null;
            }
        }
    }

    //private void TryCatch()
    //{
    //    Collider2D hit = Physics2D.OverlapCircle(transform.position, 0.5f, interactionLayer);

    //    if (hit != null && hit.TryGetComponent<Trash>(out Trash trash))
    //    {
    //        if (trash.IsContamination)
    //            return;

    //        if (trash.HasFish())
    //        {
    //            trash.ReleaseFish();
    //        }
    //        else
    //        {
    //            currentTrash = trash;
    //            currentTrash.FollowHand(transform);
    //            justCaught = true;
    //        }
    //    }
    //}

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
