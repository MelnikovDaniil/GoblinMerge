using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Crystal : MonoBehaviour
{
    private Animator _animator;
    private SoundGroupManagerComponent _soundGroupManagerComponent;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _soundGroupManagerComponent = GetComponent<SoundGroupManagerComponent>();
    }

    private void OnMouseUp()
    {
        _animator.Play("Crystal_Click", 0, 0);
        _soundGroupManagerComponent.PlaySoundFromGroup("Crystal");
        CrystalManager.Instance.CrystallClick();
    }
}
