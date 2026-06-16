using UnityEngine;

public class Sponge : MonoBehaviour, IInteractable
{
    [Header("Layer Recycle Bin")]
    public TrashType TrashType;
    public LayerMask RecycleBinLayer;

    private int contaminationLayerIndex;
    private Transform handTransform;

    public int ContaminationCount { get; set; }
    public bool IsCaught { get; private set; }

    private void Awake()
    {
        ContaminationCount = 0;
        contaminationLayerIndex = LayerMask.NameToLayer("Contamination");
    }

    private void Update()
    {
        if (IsCaught && handTransform != null)
            transform.position = handTransform.position;
    }

    public void Interact(HandController hand)
    {
        if (hand.CurrentTrash == null && hand.CurrentSponge == null)
        {
            hand.CurrentSponge = this;
            hand.JustCaught = true;
            IsCaught = true;
            handTransform = hand.transform;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == contaminationLayerIndex)
            if (collision.TryGetComponent<OilSlick>(out OilSlick oil) && ContaminationCount < 3)
                oil.CleanOil(this);
    }

    public void DropSponge()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, 0.5f, RecycleBinLayer);

        if (ContaminationCount >= 3 && hit != null && hit.TryGetComponent<RecycleBin>(out RecycleBin recycle))
            CheckSpongeRecycle(recycle);

        IsCaught = false;
        handTransform = null;
    }

    private void CheckSpongeRecycle(RecycleBin recycle)
    {
        if (TrashType == recycle.RecycleType)
        {
            recycle.AnimateSuccess();
            SpongeManager.Instance.SpawnSponge();
            Destroy(gameObject);
        }
        else
        {
            recycle.AnimateError();
            SpongeManager.Instance.SpawnSponge();
            GameManager.Instance.NotifyWrongRecycle();
            Destroy(gameObject);
        }
    }
}
