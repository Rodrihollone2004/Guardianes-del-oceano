using UnityEngine;

[CreateAssetMenu(fileName = "Trash SO", menuName = "Trash Settings", order = 0)]
public class TrashSO : ScriptableObject
{
    public TrashType Type;
    public float Velocity;
    public LayerMask RecycleBin;
    public LayerMask Contamination;
}