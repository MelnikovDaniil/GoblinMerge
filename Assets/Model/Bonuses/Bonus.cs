using System;
using UnityEngine;

public class Bonus : MonoBehaviour
{
    public Animator animator;
    [NonSerialized]
    public Action action;
    public ParticleSystem activatoinParticles;

    private void OnMouseDown()
    {
        action?.Invoke();
        SoundManager.PlaySound("Bonus");
        gameObject.SetActive(false);
        activatoinParticles.Play();
    }
}
