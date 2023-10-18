using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Goblin : Unit
{
    public float speed;
    public float hittingRate = 1;
    public float standupTime = 1.5f;
    public Vector3 infoPoint = Vector2.up;

    public override UnitType Type => UnitType.Goblin;

    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private SpriteRenderer _spriteRenderer;

    private bool isHitting;
    private Vector3 movingVector;
    private float hittingSpeed;
    private float standupSpeed;

    protected new void Awake()
    {
        base.Awake();
        _dragableObject.OnDrop += Drop;
        _dragableObject.OnDrag += Drag;
        hittingSpeed = 1.0f / hittingRate;
        standupSpeed = 1.0f / standupTime;
    }

    private void Start()
    {
        _animator.SetFloat("hittingSpeed", hittingSpeed);
        _animator.SetFloat("standupSpeed", standupSpeed);
    }

    public override void SetUp(int level = 0)
    {
        base.SetUp(level);
        isHitting = false;
        SetMovement();
        Upgrade();
    }

    public override void Disable()
    {
        base.Disable();
        isHitting = false;
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
            yield return new WaitForSeconds(hittingSpeed);
            var hitCost = currentEfficiency / hittingRate;
            CrystalManager.Instance.HitCrystal(hitCost, transform.position + infoPoint);
        }
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
            isHitting = true;
            _animator.SetTrigger("hit");
            StartCoroutine(HittingRoutine());
        }
    }

    private void Drag()
    {
        movingVector = Vector3.zero;
        isHitting = false;
        _animator.SetBool("isDragging", true);
    }

    private void Drop()
    {
        _animator.SetBool("isDragging", false);
        StartCoroutine(StandupRouting());
    }

    private IEnumerator StandupRouting()
    {
        yield return new WaitForSeconds(standupTime);
        SetMovement();

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
