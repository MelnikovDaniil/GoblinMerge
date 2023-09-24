using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Crystal : MonoBehaviour
{
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnMouseUp()
    {
        _animator.Play("Crystal_Click", 0, 0);
        CrystalManager.Instance.CrystallClick();
    }
}
