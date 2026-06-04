using UnityEngine;

public class Sponge : MonoBehaviour, IInteractable
{
    private Transform handTransform;
    private int contaminationLayerIndex;

    public bool IsCaught { get; private set; }

    private void Awake()
    {
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
            if (collision.TryGetComponent<OilSlick>(out OilSlick oil))
                oil.CleanOil();
    }

    //private void OnTriggerStay2D(Collider2D collision)
    //{
    //    if (collision.gameObject.layer == contaminationLayerIndex)
    //        if (collision.TryGetComponent<OilSlick>(out OilSlick oil))
    //            oil.CleanOil();
    //}

    public void DropSponge()
    {
        IsCaught = false;
        handTransform = null;
    }
}
