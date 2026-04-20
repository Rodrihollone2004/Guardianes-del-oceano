using UnityEngine;

public class Fish : MonoBehaviour
{
    [Header("Fish Configuration")]
    [SerializeField] private float velocity;
    [SerializeField] private Vector2 direction;


    private bool isTrapped;
    private Transform trapTransform;
    private int trashLayerIndex;

    private void Awake()
    {
        trashLayerIndex = LayerMask.NameToLayer("Trash");
    }

    private void Update()
    {
        if (isTrapped && trapTransform != null)
            transform.position = trapTransform.position;
        else
            transform.Translate(direction * velocity * Time.deltaTime);
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
        if (trashScript.IsCaught) return;

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


