using System.Collections.Generic;
using UnityEngine;

public class Otter : MonoBehaviour, IFisheable, IInteractable
{
    //[Header("Trash Contacts")]
    //[SerializeField] private List<TrashType> contactTypes;

    [Header("Animal Configuration")]
    [SerializeField] private Sprite changeSprite;
    [SerializeField] private int contaminationPorcentage;

    private float velocity;
    private Vector2 moveDirection;
    private bool isContaminated;
    private int contaminationLayerIndex;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Initialize(Vector2 dir, float speed)
    {
        moveDirection = dir;
        velocity = speed;
        contaminationLayerIndex = LayerMask.NameToLayer("Contamination");

        if (spriteRenderer != null)
            spriteRenderer.flipX = (moveDirection.x < 0);
    }

    private void Update()
    {
        //if (isTrapped && trapTransform != null)
        //    transform.position = trapTransform.position;
        //else
        transform.Translate(moveDirection * velocity * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (/*!isTrapped && */collision.gameObject.layer == contaminationLayerIndex)
        {
            //if (collision.TryGetComponent<Trash>(out Trash trashScript))
            //    if (contactTypes.Contains(trashScript.Type))
            OtterContaminated();
        }
    }

    private void OtterContaminated()
    {
        if (!isContaminated)
        {
            GameManager.Instance.AddRawContamination(contaminationPorcentage);
            spriteRenderer.sprite = changeSprite;
            spriteRenderer.color = Color.gray;
            isContaminated = true;
        }
    }

    public void Interact(HandController hand = null)
    {
        if (!isContaminated)
        {
            moveDirection.x *= -1;
            spriteRenderer.flipX = (moveDirection.x < 0);
        }
    }
    //private void TrapInTrash(Transform trash, Trash trashScript)
    //{
    //    if (trashScript.IsCaught || trashScript.IsFishTrapped)
    //        return;

    //    isTrapped = true;
    //    trapTransform = trash;
    //    trashScript.SetTrappedFish(this);
    //}

    //public void Release()
    //{
    //    isTrapped = false;
    //    trapTransform = null;
    //}
}