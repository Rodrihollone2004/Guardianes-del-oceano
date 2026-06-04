using System;
using UnityEngine;

public class TrashTrigger : MonoBehaviour
{
    public Action OnContamination;

    private int trashLayerIndex;

    private void Awake()
    {
        trashLayerIndex = LayerMask.NameToLayer("Trash");
    }

    private void Start()
    {
        GameManager.Instance.TrashTrigger = this;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == trashLayerIndex)
        {
            Trash trash = collision.gameObject.GetComponent<Trash>();

            if (trash != null && !trash.IsCaught)
            {
                trash.SetToBottom();

                OnContamination?.Invoke();
            }
        }
    }

    public void UpdateContamination()
    {
        OnContamination?.Invoke();
    }
}