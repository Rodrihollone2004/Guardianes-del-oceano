using UnityEngine;
using DG.Tweening;

public class RecycleBin : MonoBehaviour
{
    [field: SerializeField] public TrashType RecycleType { get; set; }

    public void AnimateSuccess()
    {
        transform.DOKill(true); 
        transform.DOPunchScale(Vector3.one * 0.2f, 0.3f, 5, 1f); // Se infla un poquito
    }

    public void AnimateError()
    {
        transform.DOKill(true);
        transform.DOShakePosition(0.4f, new Vector3(0.3f, 0, 0), 20, 90, false, true);
    }
}
