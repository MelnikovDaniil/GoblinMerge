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
    public Vector3 hitPosition;

    public override UnitType Type => UnitType.Goblin;

    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private SpriteRenderer _spriteRenderer;

    private SoundGroupManagerComponent _soundGroupManagerComponent;

    private bool isHitting;
    private Vector3 movingVector;
    private float hittingSpeed;
    private float standupSpeed;

    protected new void Awake()
    {
        base.Awake();
        _soundGroupManagerComponent = GetComponent<SoundGroupManagerComponent>();
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
        _animator.SetFloat("hittingSpeed", hittingSpeed);
        _animator.SetFloat("standupSpeed", standupSpeed);
        _soundGroupManagerComponent.PlaySoundFromGroup("Spawn");
        isHitting = false;
        SetMovement();
        _spriteRenderer.sprite = ClickAndDragWithRigidbody.Instance.unitSprites[level];
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
        _animator.SetTrigger("hit");
        _animator.speed = 0;
        yield return new WaitForSeconds(Random.value);
        _animator.speed = 1;
        while (isHitting)
        {
            yield return new WaitForSeconds(hittingSpeed);
            var hitCost = currentEfficiency / hittingRate;
            _soundGroupManagerComponent.PlaySoundFromGroup("Hit");

            var scaledHitPos = transform.position + new Vector3(Mathf.Sign(transform.localScale.x) * hitPosition.x, hitPosition.y);
            CrystalManager.Instance.HitCrystal(hitCost, transform.position + infoPoint, scaledHitPos);
        }
    }

    protected override void Upgrade()
    {
        _soundGroupManagerComponent.PlaySoundFromGroup("Merge");
        _spriteRenderer.sprite = ClickAndDragWithRigidbody.Instance.unitSprites[level];
    }

    private void StartHitting()
    {
        if (isHitting == false)
        {
            isHitting = true;
            StartCoroutine(HittingRoutine());
        }
    }

    private void Drag()
    {
        movingVector = Vector3.zero;
        isHitting = false;
        _animator.SetBool("isDragging", true);
        _soundGroupManagerComponent.PlaySoundFromGroup("Drag");
    }

    private void Drop()
    {
        _animator.SetBool("isDragging", false);
        StartCoroutine(StandupRouting());
    }

    private IEnumerator StandupRouting()
    {
        _soundGroupManagerComponent.PlaySoundFromGroup("Fall");
        yield return new WaitForSeconds(standupTime);
        if (Physics2D.OverlapCircleAll(transform.position, 0.5f)
            .Any(x => x.CompareTag("Crystal")))
        {
            StartHitting();
        }
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
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + hitPosition, 0.2f);
    }
}
