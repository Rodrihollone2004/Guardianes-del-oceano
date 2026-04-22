using UnityEngine;

public class FishTrigger : MonoBehaviour
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
