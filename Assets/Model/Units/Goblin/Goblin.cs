using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin : MonoBehaviour
{
    public float speed;
    public float hittingRate = 1;
    public Vector3 infoPoint = Vector2.up;

    [SerializeField]
    private Animator _animator;

    private bool isHitting;
    private Vector3 movingVector;
    private float hittingTime;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        movingVector = Vector3.zero - transform.position;
        movingVector = movingVector.normalized;
        hittingTime = 1.0f / hittingRate;
    }

    private void Start()
    {
        _animator.SetFloat("hittingSpeed", hittingTime);
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
            CrystalManager.Instance.HitCrystal(1, transform.position + infoPoint);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Crystal")
        {
            _animator.SetTrigger("hit");
            isHitting = true;
            StartCoroutine(HittingRoutine());
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(transform.position + infoPoint, Vector2.one);
    }
}
