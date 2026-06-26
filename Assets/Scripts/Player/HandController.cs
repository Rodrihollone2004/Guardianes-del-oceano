using UnityEngine;

public class HandController : MonoBehaviour
{
    [Header("Movement Configuration")]
    [SerializeField] private float smoothness = 0.1f;

    [Header("Catch Configuration")]
    [SerializeField] private LayerMask interactionLayer;

    [Header("Feedback Hand")]
    [SerializeField] private Sprite normalHand;
    [SerializeField] private Sprite closeHand;
    private SpriteRenderer spriteRenderer;

    private Vector3 velocity;
    private Camera mainCamera;
    private Trash currentTrash;
    private Sponge currentSponge;

    private bool justCaught;

    private Trash rescuingTrash;

    public Trash CurrentTrash { get => currentTrash; set => currentTrash = value; }
    public bool JustCaught { get => justCaught; set => justCaught = value; }
    public Sponge CurrentSponge { get => currentSponge; set => currentSponge = value; }

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (GameManager.Instance.IsTutorialOpen)
            return;

        Vector3 mousePosition = InputManager.Instance.MoveInput;
        mousePosition.z = Mathf.Abs(mainCamera.transform.position.z);

        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mousePosition);
        Vector3 clampedPosition = ClampPositionToScreen(worldPosition);
        transform.position = Vector3.SmoothDamp(transform.position, clampedPosition, ref velocity, smoothness);

        spriteRenderer.sprite = InputManager.Instance.IsDraging ? closeHand : normalHand;

        HandleInteraction();
    }

    private void HandleInteraction()
    {
        if (InputManager.Instance.WasClickPressedThisFrame())
        {
            Collider2D hit = Physics2D.OverlapCircle(transform.position, 0.4f, interactionLayer);

            if (hit != null)
            {
                if (hit.TryGetComponent<Trash>(out var trash))
                    rescuingTrash = trash;

                if (hit.TryGetComponent<IInteractable>(out var interactable))
                {
                    interactable.Interact(this);
                    return;
                }
            }
        }

        if (!InputManager.Instance.IsDraging)
        {
            if (rescuingTrash != null)
            {
                rescuingTrash.CancelInteract();
                rescuingTrash = null;
            }

            if (currentTrash != null || currentSponge != null)
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
        else
        {
            if (rescuingTrash != null && !rescuingTrash.HasFish())
                rescuingTrash = null;
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
