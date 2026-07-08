using UnityEngine;

public class BubbleFloat : MonoBehaviour
{
    [Header("Bubble Settings")]
    [Tooltip("Qué tan rápido sube y baja la burbuja.")]
    [SerializeField] private float speed = 2f;

    [Tooltip("Qué tanta distancia recorre hacia arriba y hacia abajo.")]
    [SerializeField] private float amplitude = 0.5f;

    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        float newY = startPosition.y + Mathf.Sin(Time.unscaledTime * speed) * amplitude;

        transform.position = new Vector3(startPosition.x, newY, startPosition.z);
    }
}
