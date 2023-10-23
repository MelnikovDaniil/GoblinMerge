using System.Collections;
using UnityEngine;

public class MergeHand : MonoBehaviour
{
    public Unit firstUnit;
    public Unit secondUnit;

    public float movingTime;
    public float dragDropDelay;

    private float currentMovingTime;
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (firstUnit != null && secondUnit != null && currentMovingTime < movingTime)
        {
            currentMovingTime += Time.deltaTime;
            transform.position = Vector2.Lerp(firstUnit.transform.position, secondUnit.transform.position, currentMovingTime / movingTime);
        
            if (currentMovingTime >= movingTime)
            {
                StopMovement();
            }
        }
    }

    public void SetMovement(Unit firstUnit, Unit secondUnit)
    {
        _animator.SetFloat("delaySpeed", 1.0f / dragDropDelay);
        currentMovingTime = movingTime;
        this.firstUnit = firstUnit;
        this.secondUnit = secondUnit;
        StartCoroutine(MovementRoutine());
    }

    public IEnumerator MovementRoutine()
    {
        _animator.Play("Drag");
        transform.position = firstUnit.transform.position;
        yield return new WaitForSeconds(dragDropDelay);
        currentMovingTime = 0;
    }

    public IEnumerator ResetRoutine()
    {
        yield return new WaitForSeconds(dragDropDelay);
        StartCoroutine(MovementRoutine());
    }

    private void StopMovement()
    {
        _animator.Play("Drop");
        StartCoroutine(ResetRoutine());
    }

}
