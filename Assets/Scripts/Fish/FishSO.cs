using UnityEngine;

[CreateAssetMenu(fileName = "New Fish", menuName = "Fish Data", order = 0)]
public class FishSO : ScriptableObject
{
    public GameObject prefab;
    public float minVelocity = 2f;
    public float maxVelocity = 5f;
}
