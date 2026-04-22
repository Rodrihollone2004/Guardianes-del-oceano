using UnityEngine;

public enum TrashType
{
    plastic,
    glass,
    Dangerous,
    organic
}

public class Trash : MonoBehaviour
{
    [Header("Trash Configuration")]
    [SerializeField] private TrashSO trashSO;

    private Vector2 direction;
    private Transform handTransform;

    private Fish trappedFish;

    public bool IsCaught { get; private set; }
    public bool IsFishTrapped { get; private set; }
    public bool IsContamination { get; private set; }
    public TrashType Type => trashSO.Type;

    private void Awake()
    {
        direction = new Vector2(0, -1);
    }

    private void Update()
    {
        if (IsContamination) return;
        
        if (IsCaught && handTransform != null)
            transform.position = handTransform.position;
        else
            transform.Translate(direction * trashSO.Velocity * Time.deltaTime);
    }

    public void FollowHand(Transform hand)
    {
        IsCaught = true;
        handTransform = hand;
    } 

    public void DropTrash()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, 0.5f, trashSO.RecycleBin);

        if (hit != null && hit.TryGetComponent<RecycleBin>(out RecycleBin recycle))
            CheckTrashRecycle(recycle);
        else
            CheckTrashContamination();

            IsCaught = false;
        handTransform = null;
    }

    private void CheckTrashRecycle(RecycleBin recycle)
    {
        if (trashSO.Type == recycle.RecycleType)
        {
            GameManager.Instance.NotifyTrashRecycled();
            Destroy(gameObject);
        }
        else
        {
            GameManager.Instance.NotifyWrongRecycle();
            Destroy(gameObject);
        }
    }

    private void CheckTrashContamination()
    {
        Collider2D triggerHit = Physics2D.OverlapCircle(transform.position, 0.1f, trashSO.Contamination);

        if (triggerHit != null && triggerHit.TryGetComponent<TrashTrigger>(out TrashTrigger trigger))
        {
            float topOfTrigger = triggerHit.bounds.max.y;
            transform.position = new Vector3(transform.position.x, topOfTrigger, transform.position.z);

            SetToBottom();
            trigger.UpdateContamination();
        }

        IsCaught = false;
        handTransform = null;
    }

    public void SetTrappedFish(Fish fish)
    {
        trappedFish = fish;
        IsFishTrapped = true;
    }

    public bool HasFish() => trappedFish != null;

    public void ReleaseFish()
    {
        if (trappedFish != null)
        {
            trappedFish.Release();
            trappedFish = null;
            IsFishTrapped = false;
        }
    }

    public void SetToBottom()
    {
        IsContamination = true;
        IsCaught = false;
        handTransform = null;

        if (TryGetComponent<SpriteRenderer>(out var renderer))
            renderer.color = Color.gray;
    }
}