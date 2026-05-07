using UnityEngine;

[CreateAssetMenu(fileName = "New Fish", menuName = "Fish Data", order = 0)]
public class FishSO : ScriptableObject
{
    public string fishName;
    [TextArea] public string description;

    public GameObject prefab;
    public Sprite stickerSprite;

    public float minVelocity = 2f;
    public float maxVelocity = 5f;

    public bool isUnlocked;
}
