using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PurchaseButton : MonoBehaviour
{
    public Button button;
    public TextMeshProUGUI costText;
    public float cost;

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void Click()
    {
        _animator.Play("Click", 0, 0);
    }

    public void Disable()
    {
        _animator.Play("Disable", 0, 0);
    }

    public void Enable()
    {
        _animator.Play("Enable", 0, 0);
    }
}
