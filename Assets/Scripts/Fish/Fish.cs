using UnityEngine;

public class Fish : MonoBehaviour
{
    private float velocity;
    private Vector2 moveDirection;
    private bool isTrapped;
    private Transform trapTransform;
    private int trashLayerIndex;

    public void Initialize(Vector2 dir, float speed)
    {
        moveDirection = dir;
        velocity = speed;
        trashLayerIndex = LayerMask.NameToLayer("Trash");

        if (moveDirection.x > 0)
            transform.Rotate(0, 0, -90);
        else
            transform.Rotate(0, 0, 90);
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
            {
                TrapInTrash(collision.transform, trashScript);
            }
        }
    }

    public void TrapInTrash(Transform trash, Trash trashScript)
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