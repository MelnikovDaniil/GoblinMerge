using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Goblin : Unit
{
    public float speed;
    public float hittingRate = 1;
    public Vector3 infoPoint = Vector2.up;

    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private SpriteRenderer _spriteRenderer;

    private bool isHitting;
    private Vector3 movingVector;
    private float hittingTime;

    private Coroutine hittingRoutine;
    protected new void Awake()
    {
        base.Awake();
        _dragableObject.OnDrop += Drop;
        _dragableObject.OnDrag += Drag;
        hittingTime = 1.0f / hittingRate;
    }

    private void Start()
    {
        _animator.SetFloat("hittingSpeed", hittingTime);
        SetMovement();
    }

    private void Update()
    {
        if (!isHitting)
        {
            transform.position += movingVector * speed * Time.deltaTime;
        }
    }

    private IEnumerator HittingRoutine()
    {
        while (isHitting)
        {
            yield return new WaitForSeconds(hittingTime);
            var hitCost = currentEfficiency / hittingRate;
            CrystalManager.Instance.HitCrystal(hitCost, transform.position + infoPoint);
        }
        hittingRoutine = null;
    }

    protected override void Upgrade()
    {
        _spriteRenderer.sprite = ClickAndDragWithRigidbody.Instance.unitSprites[level];
        Debug.Log("Upgraded level - " + level);
    }

    private void StartHitting()
    {
        if (isHitting == false)
        {
            _animator.SetTrigger("hit");
            isHitting = true;
            hittingRoutine = StartCoroutine(HittingRoutine());
        }
    }

    private void Drag()
    {
        isHitting = false;
        _animator.SetBool("isDragging", true);
    }

    private void Drop()
    {
        _animator.SetBool("isDragging", false);
        if (Physics2D.OverlapCircleAll(transform.position, 0.5f)
            .Any(x => x.CompareTag("Crystal")))
        {
            StartHitting();
        }
        else
        {
            SetMovement();
        }
    }

    private void SetMovement()
    {
        movingVector = Vector3.zero - transform.position;
        movingVector = movingVector.normalized;
        transform.localScale = new Vector2(Mathf.Sign(movingVector.x), 1);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_dragableObject.isDragging && collision.CompareTag("Crystal"))
        {
            StartHitting();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(transform.position + infoPoint, Vector2.one);
    }
}
