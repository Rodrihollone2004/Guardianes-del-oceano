using UnityEngine;

public class HandController : MonoBehaviour
{
    [Header("Movement Configuration")]
    [SerializeField] private float smoothness = 0.1f;

    private Vector3 velocity;
    private Camera mainCamera;

    private Trash currentTrash;

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
        if (InputManager.Instance.IsDraging)
            CatchTrash();

        if (!InputManager.Instance.IsDraging && currentTrash != null)
        {
            currentTrash.DropTrash();
            currentTrash = null;
        }
    }

    private void CatchTrash()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, 0.5f);

        if (hit != null && hit.CompareTag("Trash"))
        {
            currentTrash = hit.GetComponent<Trash>();
            currentTrash.FollowHand(transform);
        }
    }
}
