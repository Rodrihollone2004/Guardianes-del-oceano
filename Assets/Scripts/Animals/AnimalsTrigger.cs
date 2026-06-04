using UnityEngine;

public class AnimalsTrigger : MonoBehaviour
{
    private int fishLayerIndex;

    private void Awake()
    {
        fishLayerIndex = LayerMask.NameToLayer("Fishes");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == fishLayerIndex)
            Destroy(collision.gameObject);
    }
}
