using System;
using UnityEngine;

public class Bonus : MonoBehaviour
{
    public Animator animator;
    [NonSerialized]
    public Action action;

    private void OnMouseDown()
    {
        action?.Invoke();
        gameObject.SetActive(false);
    }
}
