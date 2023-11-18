using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Bonus : MonoBehaviour
{
    public Animator animator;
    [NonSerialized]
    public Action action;
    public ParticleSystem activatoinParticles;

    private void Start()
    {
        if (Random.Range(0, 5) == 0)
        {
            StartCoroutine(RandomJumpRoutine());
        }
    }

    private void OnMouseDown()
    {
        action?.Invoke();
        SoundManager.PlaySound("Bonus");
        gameObject.SetActive(false);
        activatoinParticles.Play();
    }

    private IEnumerator RandomJumpRoutine()
    {
        yield return new WaitForSeconds(2);
        animator.SetTrigger("jump");
    }
}
