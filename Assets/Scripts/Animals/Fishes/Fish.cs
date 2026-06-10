using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour, IFisheable
{
    [Header("Rescue Settings")]
    [SerializeField] private float rescueHoldTime = 0f;
    public float RescueHoldTime => rescueHoldTime;

    [Header("Trash Contacts")]
    [SerializeField] private List<TrashType> contactTypes;

    private float velocity;
    private Vector2 moveDirection;
    private bool isTrapped;
    private Transform trapTransform;
    private int trashLayerIndex;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Initialize(Vector2 dir, float speed)
    {
        moveDirection = dir;
        velocity = speed;
        trashLayerIndex = LayerMask.NameToLayer("Trash");

        if (spriteRenderer != null)
            spriteRenderer.flipX = (moveDirection.x < 0);
    }

    private void Update()
    {
        if (isTrapped && trapTransform != null)
            transform.position = trapTransform.position;
        else
            transform.Translate(moveDirection * velocity * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isTrapped && collision.gameObject.layer == trashLayerIndex)
        {
            if (collision.TryGetComponent<Trash>(out Trash trashScript))
                if (contactTypes.Contains(trashScript.Type))
                    TrapInTrash(collision.transform, trashScript);
        }
    }

    private void TrapInTrash(Transform trash, Trash trashScript)
    {
        if (trashScript.IsCaught || trashScript.IsFishTrapped)
            return;

        isTrapped = true;
        trapTransform = trash;
        trashScript.SetTrappedFish(this);
    }

    public void Release()
    {
        isTrapped = false;
        trapTransform = null;
    }
}
