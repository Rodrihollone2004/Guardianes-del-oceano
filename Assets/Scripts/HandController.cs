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
    }

    private void Update()
    {
        Vector3 mousePosition = InputManager.Instance.MoveInput;
        mousePosition.z = Mathf.Abs(mainCamera.transform.position.z);
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mousePosition);
        transform.position = Vector3.SmoothDamp(transform.position, worldPosition, ref velocity, smoothness);

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
            if (trash.HasFish())
            {
                trash.ReleaseFish();
                Debug.Log("Fish Released! Click again to grab the trash.");
            }
            else
            {
                currentTrash = trash;
                currentTrash.FollowHand(transform);
                justCaught = true;
                Debug.Log("Trash Caught!");
            }
        }
    }
}
