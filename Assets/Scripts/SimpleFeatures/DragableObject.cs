using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class DragableObject : MonoBehaviour
{
    public event Action OnDrag;
    public event Action OnDrop;

    public bool isDragging;

    private Rigidbody2D _rigidbody;
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    public void Drag()
    {
        isDragging = true;
        OnDrag?.Invoke();
    }

    public void Drop()
    {
        isDragging = false;
        OnDrop?.Invoke();
    }

    public void Move(Vector3 position)
    {
        //_rigidbody.MovePosition(position);
        transform.position = position;
    }
}
