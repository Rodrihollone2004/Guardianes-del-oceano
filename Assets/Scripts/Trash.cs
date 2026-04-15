using UnityEngine;

public enum TrashType
{
    plastic,
    glass,
    paper,
    organic
}

public class Trash : MonoBehaviour
{
    [Header("Trash Configuration")]
    [SerializeField] private TrashType type;
    [SerializeField] private float velocity;
    [SerializeField] private Vector2 direction;

    private bool isCaught;
    private Transform handTransform;

    private void Update()
    {
        if(isCaught && handTransform != null)
            transform.position = handTransform.position;
        else
            transform.Translate(direction * velocity * Time.deltaTime);
    }

    public void FollowHand(Transform hand)
    {
        isCaught = true;
        handTransform = hand;
    } 

    public void DropTrash()
    {
        isCaught = false;
        handTransform = null;
    }

}

public class Fish : MonoBehaviour
{

}


